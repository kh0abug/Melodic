using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melodic.Application.ExtensionMethods;

public static class TruncateString
{
    public static string Truncate(this string value, int maxLength, string truncationSuffix = "…")
    {
        return value.Length > maxLength
            ? value.Substring(0, maxLength) + truncationSuffix
            : value;
    }
}
