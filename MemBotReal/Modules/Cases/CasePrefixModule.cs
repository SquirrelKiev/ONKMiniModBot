using BotBase;
using Discord;
using Discord.Commands;
using Fergun.Interactive;
using MemBotReal.Database;

namespace MemBotReal.Modules.Cases;

[CommandsRequireModRole]
[CommandsWhitelistedChannel]
public class CasePrefixModule(DbService dbService, CaseService caseService, InteractiveService interactiveService) : PrefixModule
{
    [Command("cases")]
    [ParentModulePrefix(typeof(CaseModule))]
    public async Task CasesCommand(IGuildUser user)
    {
        await using var context = dbService.GetDbContext();

        await interactiveService.SendPaginatorAsync(await caseService.ListCases(context, Context.User, user),
            Context.Channel, TimeSpan.FromMinutes(5));
    }

    [Command("casesby")]
    [ParentModulePrefix(typeof(CaseModule))]
    public async Task CasesByCommand(IGuildUser mod)
    {
        await using var context = dbService.GetDbContext();

        await interactiveService.SendPaginatorAsync(await caseService.ListCasesBy(context, Context.User, mod),
            Context.Channel, TimeSpan.FromMinutes(5));
    }
}