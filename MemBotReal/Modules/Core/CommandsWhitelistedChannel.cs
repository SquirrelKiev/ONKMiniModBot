using Discord.Commands;
using Discord.WebSocket;
using MemBotReal.Database;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace MemBotReal.Modules;

public class CommandsWhitelistedChannel : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
    {
        var dbService = services.GetRequiredService<DbService>();

        await using var dbContext = dbService.GetDbContext();

        if (context.Channel is SocketTextChannel channel)
        {
            var guildConfig = await dbContext.GetGuildConfig(channel.Guild.Id);

            if(!guildConfig.CasesAllowedChannels.Any())
                return PreconditionResult.FromError("Can't use this command.");
            if (guildConfig.CasesAllowedChannels.Contains(channel.Id))
                return PreconditionResult.FromSuccess();

            return PreconditionResult.FromError($"Move to <#{guildConfig.CasesAllowedChannels[0]}> \ud83d\udd2b.");
        }
        else
            return PreconditionResult.FromError("You must be in a guild to run this command.");
    }
}