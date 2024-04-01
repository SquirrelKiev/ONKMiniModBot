using BotBase;
using Discord;
using Discord.Commands;

namespace MemBotReal.Modules.Warns;

[CommandsRequireModRole]
public class WarnPrefixModule(WarnService warnService) : PrefixModule
{
    [Command("warn")]
    public async Task WarnCommand(IGuildUser offender, [Remainder] string reason)
    {
        await DeferAsync();

        await ReplyAsync(await warnService.WarnUser(await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id),
            offender,
            reason,
            Context.Message));
    }
}