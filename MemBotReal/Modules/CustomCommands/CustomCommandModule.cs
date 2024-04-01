using BotBase;
using Discord.Interactions;

namespace MemBotReal.Modules.CustomCommands;

[Group("commands", "Stuff related to custom commands.")]
public class CustomCommandModule(CustomCommandService commandService) : BotModule
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
        await DeferAsync();
        await FollowupAsync(await commandService.ListCommands(Context.Guild));
    }
}