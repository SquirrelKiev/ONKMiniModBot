using BotBase.Modules.ConfigCommand;

namespace MemBotReal.Modules.ConfigCommand.Pages;

public abstract class ConfigPage : ConfigPageBase<ConfigPage.Page>
{
    public enum Page
    {
        Help,
        Prefix
    }
}
