using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MemBotReal.Database;
using Microsoft.Extensions.DependencyInjection;

namespace MemBotReal.Modules;

public class InteractionsRequireModRoleAttribute : PreconditionAttribute
{
    public override async Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
    {
        var dbService = services.GetRequiredService<DbService>();

        await using var dbContext = dbService.GetDbContext();

        if (context.User is SocketGuildUser user)
        {
            var guildConfig = await dbContext.GetGuildConfig(user.Guild.Id);

            if (user.GuildPermissions.ManageGuild || user.Roles.Any(x => x.Id == guildConfig.ModRole))
                return PreconditionResult.FromSuccess();
            else
                // ResidentSleeper joke but w/e
                return PreconditionResult.FromError("Not a magical girl.");
        }
        else
            return PreconditionResult.FromError("You must be in a guild to run this command.");
    }
}