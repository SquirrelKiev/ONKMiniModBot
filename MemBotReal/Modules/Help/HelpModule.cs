using BotBase;
using Discord.Interactions;

namespace MemBotReal.Modules.Help;

public class HelpModule(HelpService helpService) : BotModule
{
    [SlashCommand("help", "Information about the commands.")]
    [InteractionsWhitelistedChannel]
    public async Task HelpSlash()
    {
        await RespondAsync(helpService.GetMessageContents());
    }
}