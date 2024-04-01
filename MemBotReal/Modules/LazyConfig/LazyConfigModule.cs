using BotBase;
using Discord;
using Discord.Interactions;
using MemBotReal.Database;

namespace MemBotReal.Modules.LazyConfig;

[Group("config", "Config stuff")]
[RequireUserPermission(GuildPermission.ManageGuild)]
public class LazyConfigModule(DbService dbService) : BotModule
{
    [SlashCommand("set-mute-role", "Sets the role for mutes.")]
    public async Task MuteRoleSlash(IRole role)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        var config = await context.GetGuildConfig(Context.Guild.Id);

        config.MuteRole = role.Id;

        await context.SaveChangesAsync();

        await FollowupAsync(new MessageContents("Set mute role"));
    }

    [SlashCommand("set-mod-role", "Sets the mod role.")]
    public async Task ModRoleSlash(IRole role)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        var config = await context.GetGuildConfig(Context.Guild.Id);

        config.ModRole = role.Id;

        await context.SaveChangesAsync();

        await FollowupAsync(new MessageContents("Successfully set mod role"));
    }

    [SlashCommand("set-logging-channel", "Sets the channel to mod actions to.")]
    public async Task LoggingChannelSlash(ITextChannel channel)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        var config = await context.GetGuildConfig(Context.Guild.Id);

        config.LoggingChannel = channel.Id;

        await context.SaveChangesAsync();

        await FollowupAsync(new MessageContents("Successfully set logging channel"));
    }

    [SlashCommand("set-cases-channels", "Sets the channels to allow cases to be run in. Comma separated ids.")]
    public async Task CasesChannelsSlash(string channels)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        var config = await context.GetGuildConfig(Context.Guild.Id);

        var channelsArray = channels.Split(',').Select(ulong.Parse);

        config.CasesAllowedChannels = channelsArray.ToList();

        await context.SaveChangesAsync();

        await FollowupAsync(new MessageContents("Successfully set allowed channels"));
    }
}