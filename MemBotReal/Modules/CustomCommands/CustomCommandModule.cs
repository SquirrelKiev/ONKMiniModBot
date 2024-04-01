using BotBase;
using Discord.Interactions;
using Fergun.Interactive;

namespace MemBotReal.Modules.CustomCommands;

[Group("commands", "Stuff related to custom commands.")]
public class CustomCommandModule(CustomCommandService commandService, InteractiveService interactiveService) : BotModule
{
    [InteractionsRequireModRole]
    [SlashCommand("add", "Adds a custom command.")]
    public async Task AddCommand(string name, string contents)
    {
        await DeferAsync();

        await FollowupAsync(await commandService.AddCustomCommand(await Context.Guild.GetUserAsync(Context.User.Id), name, contents));
    }

    [InteractionsRequireModRole]
    [SlashCommand("remove", "Removes a custom command (if you own it).")]
    public async Task RemoveCommand(string name)
    {
        await DeferAsync();

        await FollowupAsync(await commandService.RemoveCustomCommand(await Context.Guild.GetUserAsync(Context.User.Id), name));
    }

    [InteractionsWhitelistedChannel]
    [SlashCommand("list", "Lists (almost) all the custom commands.")]
    public async Task ListCommands()
    {
        var paginator = await commandService.ListCommands(Context.User, Context.Guild);

        await interactiveService.SendPaginatorAsync(paginator, Context.Interaction, TimeSpan.FromMinutes(5));
    }
}