using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AppSettingValueType
{
    String,
    Number,
    Boolean
}