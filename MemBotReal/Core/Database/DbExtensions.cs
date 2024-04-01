using MemBotReal.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MemBotReal.Database;

public static class DbExtensions
{
    public static async Task<GuildConfig> GetGuildConfig(this BotDbContext context, ulong guildId)
    {
        var guildConfig = await context.GuildConfigs.FirstOrDefaultAsync(x => x.GuildId == guildId);

        if (guildConfig != null) return guildConfig;

        guildConfig = new GuildConfig()
        {
            GuildId = guildId
        };

        context.Add(guildConfig);

        return guildConfig;
    }

    public static async Task<CustomCommand?> GetCustomCommand(this BotDbContext context, ulong guildId, string name)
    {
        var command = await context.CustomCommands.FirstOrDefaultAsync(x => x.GuildId == guildId && x.Name == name);

        return command;
    }
}