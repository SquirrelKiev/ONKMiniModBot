using System.Text;
using BotBase;
using Discord;
using MemBotReal.Database;
using MemBotReal.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

    public async Task<MessageContents> ListCommands(IGuild guild)
    {
        await using var context = dbService.GetDbContext();

        var commands = await context.CustomCommands.Where(x => x.GuildId == guild.Id).ToArrayAsync();

        var desc = new StringBuilder();
        foreach (var command in commands.Take(50))
        {
            desc.AppendLine($"- `{command.Name}`");
        }

        if (commands.Length == 0)
        {
            desc.AppendLine("No custom commands.");
        }

        return new MessageContents(new EmbedBuilder().WithTitle($"Custom commands for {guild.Name}")
            .WithDescription(desc.ToString())
            .WithFooter($"Total of {commands.Length} commands.")
            .WithColor(0x865892), new ComponentBuilder());
    }
}