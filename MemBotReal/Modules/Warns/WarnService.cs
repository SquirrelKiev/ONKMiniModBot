using BotBase;
using Discord;
using MemBotReal.Database;
using MemBotReal.Modules.Cases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MemBotReal.Modules.Warns;

[Inject(ServiceLifetime.Singleton)]
public class WarnService(DbService dbService, CaseService caseService)
{
    public async Task<MessageContents> WarnUser(IGuildUser mod, IGuildUser offender, string reason, IMessage invokeMessage)
    {
        await using var context = dbService.GetDbContext();

        await caseService.AddWarnCase(context, mod, offender, reason, invokeMessage);

        var existingCases = await caseService.GetWarnCases(context, offender).CountAsync();

        var embed = new EmbedBuilder()
            .WithDescription(reason)
            .WithFooter($"Number of warnings: {existingCases + 1}");

        await context.SaveChangesAsync();

        return new MessageContents($"**<@{offender.Id}> has been warned, reason:**", embed.Build(), new ComponentBuilder());
    }
}