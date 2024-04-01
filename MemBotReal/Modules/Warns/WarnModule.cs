using BotBase;
using Discord;
using Discord.Interactions;

namespace MemBotReal.Modules.Warns;

[InteractionsRequireModRole]
public class WarnModule(WarnService warnService) : BotModule
{
    [SlashCommand("warn", "Warns a user with a necessary supplied reason.")]
    public async Task WarnSlash(IGuildUser offender, string reason)
    {
        await DeferAsync();

        await FollowupAsync(await warnService.WarnUser(await Context.Guild.GetUserAsync(Context.User.Id),
            offender,
            reason,
            await GetOriginalResponseAsync()));
    }
}