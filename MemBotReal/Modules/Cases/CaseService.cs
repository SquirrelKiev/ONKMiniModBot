using System.Text;
using BotBase;
using Discord;
using LinqToDB.EntityFrameworkCore;
using MemBotReal.Database;
using MemBotReal.Database.Models;
using MemBotReal.Modules.Mute;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MemBotReal.Modules.Cases;

[Inject(ServiceLifetime.Singleton)]
public class CaseService
{
    public enum CaseType
    {
        Warn,
        Mute
    }

    public async Task AddMuteCase(BotDbContext context,
        IGuildUser offender,
        IGuildUser mod,
        TimeSpan duration,
        string reason,
        IMessage invokeMessage)
    {
        try
        {
            var msg =
                $"You have been muted on the {offender.Guild.Name} server " +
                $"{(duration == default ? "indefinitely" : $"for {DurationParser.ToString(duration)}")} reason: **{reason}**";

            await offender.SendMessageAsync(msg);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "shit fucked when sending {offenderId} a dm, {exceptionMsg}", offender.Id, ex.Message);
        }

        await AddCase(context,
            CaseType.Mute,
            offender,
            mod,
            $"User muted {(duration == default ? "indefinitely" : $"for {DurationParser.ToString(duration)}")}",
            reason,
            $"[Cmd invoke happened here](https://discord.com/channels/{offender.GuildId}/{invokeMessage.Channel.Id}/{invokeMessage.Id}) " +
            $"in <#{invokeMessage.Channel.Id}>");
    }

    public async Task<MessageContents> ListCases(BotDbContext context, IGuildUser offender)
    {
        var cases = (await GetCases(context, offender).ToArrayAsync());

        var embed = new EmbedBuilder()
            .WithTitle("Moderation action")
            .WithColor(0x48daf7);

        var desc = new StringBuilder();

        foreach (var aCase in cases.TakeLast(5).Reverse())
        {
            desc.AppendLine($"Case {aCase.Id} ({aCase.CaseType})")
                .AppendLine($"-*responsible: <@{aCase.ModId}>*")
                .AppendLine($"-*offender: <@{aCase.OffenderId}>*")
                .AppendLine("**-Reason:**")
                .AppendLine("```")
                .AppendLine(aCase.Reason)
                .AppendLine("```");
        }

        if (cases.Length == 0)
            desc.AppendLine("No cases.");

        embed.WithDescription(desc.ToString());
        embed.WithFooter($"Total infractions: {cases.Length}");

        return new MessageContents(embed, new ComponentBuilder());
    }

    public IQueryable<Case> GetCases(BotDbContext context, IGuildUser offender)
    {
        return context.Cases.Where(x =>
            x.GuildId == offender.GuildId && x.OffenderId == offender.Id);
    }

    public IQueryable<Case> GetWarnCases(BotDbContext context, IGuildUser offender)
    {
        return GetCases(context, offender).Where(x => x.CaseType == CaseType.Warn);
    }

    public async Task AddWarnCase(BotDbContext context, IGuildUser mod, IGuildUser offender, string reason, IMessage invokeMessage)
    {
        var existingCases = await GetWarnCases(context, offender).CountAsync();

        await AddCase(context,
            CaseType.Warn,
            offender,
            mod,
            $"User warned ({existingCases + 1} time(s))",
            reason,
            $"[Cmd invoke happened here](https://discord.com/channels/{offender.GuildId}/{invokeMessage.Channel.Id}/{invokeMessage.Id}) " +
            $"in <#{invokeMessage.Channel.Id}>");
    }

    public async Task AddCase(BotDbContext context,
        CaseType caseType,
        IGuildUser offender,
        IGuildUser mod,
        string header,
        string reason,
        string extraInfo = "",
        string logReason = "")
    {
        var config = await context.GetGuildConfig(offender.GuildId);

        var theCase = new Case()
        {
            CaseType = caseType,
            GuildId = offender.GuildId,
            ModId = mod.Id,
            OffenderId = offender.Id,
            Reason = reason
        };

        context.Add(theCase);

        var logChannel = await offender.Guild.GetTextChannelAsync(config.LoggingChannel);

        Color color = Color.Blue;

        switch (caseType)
        {
            case CaseType.Warn:
                color = 0xfa8507;
                break;
            case CaseType.Mute:
                color = 0xbf5b30;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(caseType), caseType, null);
        }

        await LogCase(offender, mod, header, reason, extraInfo, logChannel, color, logReason);
    }

    public static async Task LogCase(IGuildUser offender, IGuildUser mod, string header, string reason, string extraInfo,
        ITextChannel logChannel, Color color, string logReason = "")
    {
        List<EmbedFieldBuilder> fields =
        [
            new EmbedFieldBuilder()
                .WithIsInline(true)
                .WithName("Moderator")
                .WithValue($"<@{mod.Id}>\n{mod.Username}#{mod.DiscriminatorValue}"),
            new EmbedFieldBuilder()
                .WithIsInline(true)
                .WithName("Offender")
                .WithValue($"<@{offender.Id}>\n{offender.Username}#{offender.DiscriminatorValue}"),
            new EmbedFieldBuilder()
                .WithIsInline(true)
                .WithName("Reason")
                .WithValue(logReason == "" ? reason : logReason)
        ];

        if (extraInfo != "")
        {
            fields.Add(new EmbedFieldBuilder()
                .WithName("Extra info")
                .WithValue(extraInfo));
        }

        var embed = new EmbedBuilder()
                .WithFields(fields)
                .WithColor(color)
                .WithCurrentTimestamp()
                .WithAuthor(new EmbedAuthorBuilder().WithName(header).WithIconUrl(offender.GetDisplayAvatarUrl()))
            ;

        await logChannel.SendMessageAsync(embed: embed.Build());
    }
    }