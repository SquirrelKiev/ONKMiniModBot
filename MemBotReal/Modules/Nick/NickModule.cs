using BotBase;
using Discord;
using Discord.Interactions;

namespace MemBotReal.Modules.Nick;

[InteractionsRequireModRole]
public class NickModule(NickService nickService) : BotModule
{
    [SlashCommand("nickname", "Changes the nickname of the user.")]
    public async Task NickSlash(IGuildUser user, [MaxLength(32)] string nickname)
    {
        await DeferAsync();

        await FollowupAsync(await nickService.ChangeNick(await Context.Guild.GetUserAsync(Context.User.Id), user,
            nickname, await GetOriginalResponseAsync()));
    }
}