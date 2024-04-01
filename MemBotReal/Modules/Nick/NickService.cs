using BotBase;
using Discord;
using MemBotReal.Database;
using MemBotReal.Modules.Cases;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace MemBotReal.Modules.Nick;

[Inject(ServiceLifetime.Singleton)]
public class NickService(DbService dbService)
{
    public async Task<MessageContents> ChangeNick(IGuildUser mod, IGuildUser user, string nickname, IMessage invokeMessage)
    {
        await using var context = dbService.GetDbContext();

        var guildConfig = await context.GetGuildConfig(user.GuildId);

        try
        {
            await user.ModifyAsync(x => x.Nickname = nickname);

            var loggingChannel = await user.Guild.GetTextChannelAsync(guildConfig.LoggingChannel);

            await CaseService.LogCase(user, mod, $"Changed nickname to {nickname}", "N/A",
                "[Cmd invoke happened here](https://discord.com/channels/{offender.GuildId}/{invokeMessage.Channel.Id}/{invokeMessage.Id}) " +
                $"in <#{invokeMessage.Channel.Id}>", loggingChannel, Color.DarkGreen);

            try
            {
                await user.SendMessageAsync($"Your nickname on the {mod.Guild.Name} server was changed to {nickname}.");
            }
            catch (Exception ex)
            {
                // swallow ex
            }

            var embed = new EmbedBuilder().WithDescription(
                $"Successfully set <@{user.Id}>'s nickname to {nickname}.").WithColor(0xff3eb3);

            return new MessageContents(embed, new ComponentBuilder());
        }
        catch (Exception ex)
        {
            var embed = new EmbedBuilder().WithDescription("Failed to set nickname.").WithColor(0xff3eb3);

            Log.Warning(ex, "Failed to set nickname for {user}, {ex}", user.Id, ex.Message);

            return new MessageContents(embed, new ComponentBuilder());
        }
    }
}