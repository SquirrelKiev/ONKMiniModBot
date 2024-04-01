using BotBase;
using Discord;

namespace MemBotReal.Modules.Help;

[Inject(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton)]
public class HelpService
{
    public MessageContents GetMessageContents()
    {
        var embed = new EmbedBuilder()
            .WithDescription(
@"### :star: Mute / Unmute

`Syntax: ,mute [user] [length] [reason]`
Supply a **#d#h#m#s** for a timed mute. 

Examples:
`,mute @user optional reason` - will mute the user indefinitely
`,mute USER_ID` - can also use id instead of mention
`,mute @user 2h30m Optional reason goes here`
`,unmute @user` - will unmute the user
`,unmute @user Optional reason` - will unmute the user with a reason

### :star: Warn

`Syntax: ,warn <user> <reason>`
**Warn a user with a necessary supplied reason.**
`,warn @user Reason goes here` (reason has to be supplied)
`,warn user_id Reason goes here`

### :star: Cases
`Syntax: ,cases <offender>` - Check the past mutes and warns for a user
`Syntax: ,casesby <mod>`- Check the past mutes and warns made by a mod

### :star:Custom Commands

`Syntax: ,add <name> <contents>`
**Add a custom command**
,add suisei suisei.png

`Syntax: ,remove <name>`
**Remove a custom command**

**List Commands**
`,listcmds` - Gives a list of all the custom commands.

### :star:Nicknames

`Syntax: ,nick <user> <nickname>` - Changes the nickname of that user. Nickname must be under 32 characters.")
            .WithColor(0xff3eb3);

        return new MessageContents(embed, new ComponentBuilder());
    }
}