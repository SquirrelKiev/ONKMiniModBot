using BotBase;
using Discord;
using Discord.Interactions;
using MemBotReal.Database;

namespace MemBotReal.Modules.Cases;

[InteractionsRequireModRole]
[InteractionsWhitelistedChannel]
public class CaseModule(DbService dbService, CaseService caseService) : BotModule
{
    [SlashCommand("cases", "Lists last 5 warns/mutes a user has.")]
    public async Task ListCasesSlash(IGuildUser user)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        await FollowupAsync(await caseService.ListCases(context, user));
    }
}