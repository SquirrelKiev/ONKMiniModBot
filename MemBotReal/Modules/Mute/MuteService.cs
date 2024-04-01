using BotBase;
using Discord;
using Discord.WebSocket;
using MemBotReal.Database;
using MemBotReal.Modules.Cases;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.ComponentModel;
using MemBotReal.Database.Models;

namespace MemBotReal.Modules.Mute;

[Inject(ServiceLifetime.Singleton)]
public class MuteService(CaseService caseService, DbService dbService)
{
    public async Task<MessageContents> MuteUser(IGuildUser mod, IGuildUser offender, IMessage invokeMessage, string duration = "inf", string reason = "No reason.")
    {
        await using var context = dbService.GetDbContext();

        var guildConfig = await context.GetGuildConfig(mod.GuildId);

        if (guildConfig.MuteRole == 0ul)
        {
            return new MessageContents("Mute role not set on guild!");
        }

        if (guildConfig.LoggingChannel == 0ul)
        {
            return new MessageContents("No logging channel set.");
        }

        var muteRole = guildConfig.MuteRole;

        if (offender.RoleIds.Contains(muteRole))
        {
            var errorEmbed = new EmbedBuilder().WithDescription($"<@{offender.Id}> is already muted")
                .WithColor(0x753b34);

            return new MessageContents(errorEmbed, new ComponentBuilder());
        }

        bool isInf = duration == "inf";
        TimeSpan muteTimeSpan = default;
        if (!isInf)
        {
            if (!DurationParser.TryParse(duration, out muteTimeSpan))
            {
                return new MessageContents("Not a valid duration!");
            }
        }

        await offender.AddRoleAsync(muteRole);

        if (muteTimeSpan != default)
            AddDelayedUnmute(context, offender, muteTimeSpan);

        await caseService.AddMuteCase(context, offender, mod, muteTimeSpan, reason, invokeMessage);

        await context.SaveChangesAsync();

        string message = $"<@{offender.Id}> is now muted from text channels{(isInf ? "" : $" for {DurationParser.ToString(muteTimeSpan)}")}.";

        var embed = new EmbedBuilder().WithDescription(message).WithColor(0x6786da);
        return new MessageContents(embed, new ComponentBuilder());
    }

    private static void AddDelayedUnmute(BotDbContext context, IGuildUser offender, TimeSpan muteTimeSpan)
    {
        var now = DateTimeOffset.UtcNow;

        var delayedUnmute = new DelayedUnmute
        {
            GuildId = offender.GuildId,
            OffenderId = offender.Id,
            UnmuteWhen = now + muteTimeSpan
        };

        context.DelayedUnmutes.Add(delayedUnmute);
    }

    public async Task<MessageContents> UnmuteUser(IGuildUser mod, IGuildUser offender, string reason = "")
    {
        await using var context = dbService.GetDbContext();

        var val = await UnmuteUser(context, mod, offender, reason);
        await context.SaveChangesAsync();

        return val;
    }

    public async Task<MessageContents> UnmuteUser(BotDbContext context, IGuildUser mod, IGuildUser offender, string reason = "")
    {
        var guildConfig = await context.GetGuildConfig(offender.GuildId);

        if (guildConfig.MuteRole == 0ul)
        {
            return new MessageContents("Mute role not set on guild!");
        }

        if (guildConfig.LoggingChannel == 0ul)
        {
            return new MessageContents("No logging channel set.");
        }

        var muteRole = guildConfig.MuteRole;

        if (!offender.RoleIds.Contains(muteRole))
        {
            return new MessageContents("User is not muted", embed: null, new ComponentBuilder());
        }

        await offender.RemoveRoleAsync(guildConfig.MuteRole);

        var logChannel = await offender.Guild.GetTextChannelAsync(guildConfig.LoggingChannel);

        await CaseService.LogCase(offender,
            mod,
            "User unmuted",
            reason == ""
                ? "No reason provided."
                : reason,
            "",
            logChannel,
            0x62f07e);

        try
        {
            var msg =
                $"You have been unmuted on the {offender.Guild.Name} server. {(reason == "" ? "" : $"Reason: **{reason}**")}";

            await offender.SendMessageAsync(msg);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "shit fucked when sending {offenderId} a dm, {exceptionMsg}", offender.Id, ex.Message);
        }

        var delayedUnmute =
            context.DelayedUnmutes.FirstOrDefault(x => x.GuildId == offender.GuildId && x.OffenderId == offender.Id);

        if (delayedUnmute != null)
            context.DelayedUnmutes.Remove(delayedUnmute);

        string message = $"<@{offender.Id}> has been unmuted.";

        var embed = new EmbedBuilder().WithDescription(message).WithColor(0x76dfe3);
        return new MessageContents(embed, new ComponentBuilder());
    }
}