﻿using System.Text;
using BotBase;
using Discord;
using Fergun.Interactive;
using Fergun.Interactive.Pagination;
using MemBotReal.Database;
using MemBotReal.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace MemBotReal.Modules.CustomCommands;

[Inject(ServiceLifetime.Singleton)]
public class CustomCommandService(DbService dbService)
{
    public async Task<MessageContents> AddCustomCommand(IGuildUser commandOwner, string name, string contents)
    {
        await using var context = dbService.GetDbContext();

        if (await context.GetCustomCommand(commandOwner.GuildId, name) != null)
        {
            return new MessageContents(
                "Command already exists. Please remove the command before adding one with this name.");
        }

        context.CustomCommands.Add(new CustomCommand()
        {
            GuildId = commandOwner.GuildId,
            OwnerId = commandOwner.Id,
            Name = name,
            Contents = contents
        });

        await context.SaveChangesAsync();

        var embed = new EmbedBuilder().WithDescription($"Successfully added custom command `{name}`.")
            .WithColor(0xff3eb3);
        return new MessageContents(embed, new ComponentBuilder());
    }

    public async Task<MessageContents> RemoveCustomCommand(IGuildUser commandExecutor, string name)
    {
        await using var context = dbService.GetDbContext();

        var command = await context.GetCustomCommand(commandExecutor.GuildId, name);
        if (command == null)
        {
            return new MessageContents("Command doesn't exist.", embed: null, new ComponentBuilder());
        }

        if (command.OwnerId != commandExecutor.Id && !commandExecutor.GuildPermissions.ManageGuild)
        {
            return new MessageContents(new EmbedBuilder().WithDescription(
                $"Soul gem mismatch. Please call <@{command.OwnerId}> to finish the procedure."), new ComponentBuilder());
        }

        context.CustomCommands.Remove(command);

        await context.SaveChangesAsync();

        return new MessageContents(new EmbedBuilder().WithDescription($"Successfully removed command `{command.Name}`.").WithColor(0xff3eb3),
            new ComponentBuilder());
    }

    public async Task<LazyPaginator> ListCommands(IUser executor, IGuild guild)
    {
        await using var context = dbService.GetDbContext();

        var commands = await context.CustomCommands.Where(x => x.GuildId == guild.Id).ToArrayAsync();
        const int maxPerPage = 20;

        var paginator = new LazyPaginatorBuilder()
            .AddUser(executor)
            .WithPageFactory(PageFactory)
            .WithFooter(PaginatorFooter.PageNumber)
            .WithMaxPageIndex(commands.Length / maxPerPage)
            .WithDefaultEmotes()
            .WithActionOnCancellation(ActionOnStop.DisableInput)
            .Build();

        return paginator;

        Task<PageBuilder> PageFactory(int page)
        {
            var desc = new StringBuilder();
            foreach (var command in commands.Skip(page * maxPerPage).Take(maxPerPage))
            {
                desc.AppendLine($"- `{command.Name}`");
            }

            var eb = new PageBuilder().WithTitle($"Custom commands for {guild.Name} ({commands.Length} total)")
                .WithDescription(desc.ToString())
                .WithColor(0x865892);

            return Task.FromResult(eb);
        }
    }

    public async Task<string> CommandsAsJson(IGuild guild)
    {
        await using var context = dbService.GetDbContext();

        var commands = await context.CustomCommands.Where(x => x.GuildId == guild.Id).ToArrayAsync();

        var json = JsonConvert.SerializeObject(commands, Formatting.Indented);

        return json;
    }
}