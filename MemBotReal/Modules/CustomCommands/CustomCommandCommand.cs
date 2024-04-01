using BotBase;
using Discord;
using Discord.Commands;
using Discord.Interactions;

namespace MemBotReal.Modules.CustomCommands;

// :pls:
public class CustomCommandCommand(CustomCommandService commandService) : PrefixModule
{
    [CommandsRequireModRole]
    [Command("add")]
    public async Task AddCommand(string name, string contents)
    {
        await DeferAsync();

        await ReplyAsync(await commandService.AddCustomCommand(await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id), name, contents));
    }

    [CommandsRequireModRole]
    [Command("remove")]
    public async Task RemoveCommand(string name)
    {
        await DeferAsync();

        await ReplyAsync(await commandService.RemoveCustomCommand(await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id), name));
    }

    [CommandsWhitelistedChannel]
    [Command("listcmds")]
    public async Task ListCommands()
    {
        await DeferAsync();

        await ReplyAsync(await commandService.ListCommands(Context.Guild));
    }
}