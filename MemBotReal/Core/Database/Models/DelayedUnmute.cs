namespace MemBotReal.Database.Models;

public class DelayedUnmute
{
    public required ulong GuildId { get; set; }
    public required ulong OffenderId { get; set; }
    public required DateTimeOffset UnmuteWhen { get; set; }
}