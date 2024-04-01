using BotBase;
using Discord;

namespace MemBotReal.Modules.Help;

[Inject(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
public class HelpService
{
    public MessageContents GetMessageContents()
    {
        var embed = new EmbedBuilder()
            .WithDescription("### ⭐ Mute / Unmute" +
            "\n\n" +
            "`Syntax: ,mute [user] [length] [reason]`\n" +
            "Supply a **#d#h#m#s** for a timed mute. \n\n" +
            "Examples:\n" +
            "`,mute @user optional reason` - will mute the user indefinitely\n" +
            "`,mute USER_ID` - can also use id instead of mention\n" +
            "`,mute @user 2h30m Optional reason goes here`\n" +
            "`,unmute @user` - will unmute the user\n" +
            "`,unmute @user Optional reason` - will unmute the user with a reason\n\n" +
            "### ⭐ Warn\n\n" +
            "`Syntax: ,warn <user> <reason>`\n" +
            "**Warn a user with a necessary supplied reason.**\n" +
            "`,warn @user Reason goes here` (reason has to be supplied)\n" +
            "`,warn user_id Reason goes here`\n\n" +
            "### ⭐ Cases\n" +
            "Check the past mutes and warns for a user\n" +
            "`Syntax: ,cases <offender>`\n\n" +
            "### ⭐Custom Commands\n\n" +
            "`Syntax: ,add <name> <contents>`\n" +
            "**Add a custom command**\n" +
            ",add suisei suisei.png\n\n" +
            "`Syntax: ,remove <name>`\n" +
            "**Remove a custom command**\n\n" +
            "**List Commands**\n" +
            "`,listcmds` - Gives a list of all the custom commands.")
            .WithColor(0xff3eb3);

        return new MessageContents(embed, new ComponentBuilder());
    }
}