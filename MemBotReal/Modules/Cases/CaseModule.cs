using BotBase;
using Discord;
using Discord.Interactions;
using Fergun.Interactive;
using MemBotReal.Database;

namespace MemBotReal.Modules.Cases;

[InteractionsRequireModRole]
[InteractionsWhitelistedChannel]
public class CaseModule(DbService dbService, CaseService caseService, InteractiveService interactiveService) : BotModule
{
    [SlashCommand("cases", "Lists last 5 warns/mutes a user has.")]
    public async Task ListCasesSlash(IGuildUser user)
    {
        await using var context = dbService.GetDbContext();

        await interactiveService.SendPaginatorAsync(await caseService.ListCases(context, Context.User, user),
            Context.Interaction, TimeSpan.FromMinutes(5));
    }

    [SlashCommand("cases-by", "Lists last 5 warns/mutes a specific mod made.")]
    public async Task ListCasesBySlash(IGuildUser mod)
    {
        await using var context = dbService.GetDbContext();

        await interactiveService.SendPaginatorAsync(await caseService.ListCasesBy(context, Context.User, mod),
            Context.Interaction, TimeSpan.FromMinutes(5));
    }
}