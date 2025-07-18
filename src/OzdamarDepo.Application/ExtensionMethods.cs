using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OzdamarDepo.Application;
public static class ExtensionMethods
{
    public static string GetDisplayName(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field!.GetCustomAttribute<DisplayAttribute>();
        return attribute?.Name ?? value.ToString();
    }
}
