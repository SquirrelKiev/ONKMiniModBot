using BotBase;
using Discord.Commands;

namespace MemBotReal.Modules.Help;

public class HelpPrefixModule(HelpService helpService) : PrefixModule
{
    [Command("help")]
    [CommandsWhitelistedChannel]
    public async Task HelpCommand()
    {
        await ReplyAsync(helpService.GetMessageContents());
    }
}