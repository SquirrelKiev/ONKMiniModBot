using BotBase;
using Discord;
using Discord.Interactions;

namespace MemBotReal.Modules.Mute;

[InteractionsRequireModRole]
public class MuteModule(MuteService muteService) : BotModule
{
    [SlashCommand("mute", "Mutes the user.")]
    public async Task MuteSlash(IGuildUser offender, string duration = "inf", string reason = "No reason.")
    {
        await DeferAsync();

        await FollowupAsync(await muteService.MuteUser(await Context.Guild.GetUserAsync(Context.User.Id),
            offender,
            await Context.Interaction.GetOriginalResponseAsync(),
            duration, reason));
    }

    [SlashCommand("unmute", "Unmutes the user.")]
    public async Task UnmuteSlash(IGuildUser offender, string reason = "No reason.")
    {
        await DeferAsync();

        var mod = await Context.Guild.GetUserAsync(Context.User.Id);

        await FollowupAsync(await muteService.UnmuteUser(mod, offender, reason));
    }
}