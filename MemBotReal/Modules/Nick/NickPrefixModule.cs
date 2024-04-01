using BotBase;
using Discord;
using Discord.Commands;
using Serilog;

namespace MemBotReal.Modules.Nick;

[CommandsRequireModRole]
public class NickPrefixModule(NickService nickService) : PrefixModule
{
    [Command("nick")]
    public async Task NickCommand(IGuildUser user, [Remainder] string nickname)
    {
        await DeferAsync();

        if (nickname.Length > 32)
        {
            await ReplyAsync("Nickname is too long!");
            return;
        }

        await ReplyAsync(await nickService.ChangeNick(await ((IGuild)Context.Guild).GetUserAsync(user.Id), user, nickname, Context.Message));
    }
}