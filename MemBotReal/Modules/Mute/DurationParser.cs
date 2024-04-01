using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace MemBotReal.Modules.Mute;

public class DurationParser
{
    public static TimeSpan Parse(string input)
    {
        if (!TryParse(input, out TimeSpan result))
        {
            throw new ArgumentException("Input is not in a valid format.", nameof(input));
        }

        return result;
    }

    public static bool TryParse(string input, out TimeSpan timeSpan)
    {
        timeSpan = default;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var regex = new Regex(@"((?<days>\d+)d)?((?<hours>\d+)h)?((?<minutes>\d+)m)?((?<seconds>\d+)s)?", RegexOptions.IgnoreCase);
        var match = regex.Match(input);

        if (!match.Success)
        {
            return false;
        }

        int days = GetIntValue(match, "days");
        int hours = GetIntValue(match, "hours");
        int minutes = GetIntValue(match, "minutes");
        int seconds = GetIntValue(match, "seconds");

        timeSpan = new TimeSpan(days, hours, minutes, seconds);

        // stopgap
        if (days == 0 && hours == 0 && minutes == 0 && seconds == 0)
            return false;

        return true;
    }

    public static string ToString(TimeSpan timeSpan)
    {
        // Building the string representation based on the presence of days, hours, minutes, and seconds.
        // Only include a component if its value is greater than 0.
        string result = "";
        
        if (timeSpan.Days > 0)
            result += $"{timeSpan.Days}d";
        if (timeSpan.Hours > 0)
            result += $"{timeSpan.Hours}h";
        if (timeSpan.Minutes > 0)
            result += $"{timeSpan.Minutes}m";
        if (timeSpan.Seconds > 0)
            result += $"{timeSpan.Seconds}s";

        return result;
    }

    private static int GetIntValue(Match match, string groupName)
    {
        return int.TryParse(match.Groups[groupName].Value, out int result) ? result : 0;
    }
}