using BotBase;
using Discord;
using Discord.Commands;
using MemBotReal.Database;

namespace MemBotReal.Modules.Cases;

[CommandsRequireModRole]
[CommandsWhitelistedChannel]
public class CasePrefixModule(DbService dbService, CaseService caseService) : PrefixModule
{
    [Command("cases")]
    [ParentModulePrefix(typeof(CaseModule))]
    public async Task CasesCommand(IGuildUser user)
    {
        await DeferAsync();

        await using var context = dbService.GetDbContext();

        await ReplyAsync(await caseService.ListCases(context, user));
    }
}