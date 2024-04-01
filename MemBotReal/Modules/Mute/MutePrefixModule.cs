using BotBase;
using Discord;
using Discord.Commands;
using Serilog;

namespace MemBotReal.Modules.Mute;

[CommandsRequireModRole]
public class MutePrefixModule(MuteService muteService) : PrefixModule
{
    [Command("mute")]
    [ParentModulePrefix(typeof(MuteModule))]
    public Task MuteCommand(IGuildUser offender) => MuteCommandImpl(offender);

    [Command("mute")]
    [ParentModulePrefix(typeof(MuteModule))]
    public Task MuteCommand(IGuildUser offender, [Remainder] string optionalDurationAndReason)
    {
        var split = optionalDurationAndReason.Split(' ', 2);

        string? duration = "inf";
        string reason = optionalDurationAndReason;
        if (split.Length >= 1)
        {
            var potentialDuration = split[0];

            if (DurationParser.TryParse(potentialDuration, out _))
            {
                Log.Debug("Farts");

                duration = potentialDuration;
                if (split.Length > 1)
                    reason = split[1];
                else
                    reason = "No reason.";
            }
        }

        return MuteCommandImpl(offender, duration, reason);
    }

    private async Task MuteCommandImpl(IGuildUser offender, string duration = "inf", string reason = "No reason.")
    {
        await DeferAsync();

        await ReplyAsync(await muteService.MuteUser(await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id),
            offender,
            Context.Message,
            duration, reason));
    }

    [Command("unmute")]
    [ParentModulePrefix(typeof(MuteModule))]
    public Task UnmuteCommand(IGuildUser offender) => UnmuteCommandImpl(offender);

    [Command("unmute")]
    [ParentModulePrefix(typeof(MuteModule))]
    public Task UnmuteCommand(IGuildUser offender, [Remainder] string reason) => UnmuteCommandImpl(offender, reason);

    private async Task UnmuteCommandImpl(IGuildUser offender, string reason = "No reason.")
    {
        await DeferAsync();

        var mod = await ((IGuild)Context.Guild).GetUserAsync(Context.User.Id);

        await ReplyAsync(await muteService.UnmuteUser(mod, offender, reason));
    }
}