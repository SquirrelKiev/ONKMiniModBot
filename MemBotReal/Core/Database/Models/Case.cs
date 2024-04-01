using BotBase.Database;
using MemBotReal.Modules.Cases;

namespace MemBotReal.Database.Models;

public class Case : DbModel
{
    public required CaseService.CaseType CaseType { get; set; }
    public required ulong GuildId { get; set; }
    public required ulong OffenderId { get; set; }
    public required ulong ModId { get; set; }
    public required string Reason { get; set; }
}