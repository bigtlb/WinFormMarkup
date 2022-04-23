using System.Linq;
using System.Text;

namespace WikiDocumentGenerator.Extensions;

public static class StringExtensions
{
    public static string CleanLines(this string lines,
        bool blockIndent = false,
        bool reduceIndent = true,
        bool trimLineFeeds = true)
    {
        if (trimLineFeeds) lines = lines.TrimStart('\r', '\n').TrimEnd('\r', '\n', ' ');
        var split = lines.Replace("\r", "").Split('\n');
        var minIndent = reduceIndent ? split.Min(l => l.Length - l.TrimStart().Length) : 0;
        var sb = new StringBuilder();
        foreach (var l in split)
        {
            if (blockIndent) sb.Append("    ");
            sb.AppendLine(l[minIndent..]);
        }

        return sb.ToString().TrimEnd();
    }
}