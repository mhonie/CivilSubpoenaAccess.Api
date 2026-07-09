using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CivilSubpoenaAccess.Api.Serialization;


/// <summary>
/// When a POCO has a property that stores JSON, write out the JSON string with nice indentations.
/// </summary>
public class IndentingConverter : JsonConverter<string>
{
    public override void WriteJson
    (
        JsonWriter writer, 
        
        string? value, 
        
        JsonSerializer serializer
    )
    {
        try
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                writer.WriteValue(value);

                return;
            }

            var parsedToken = JToken.Parse(value);

            var indentedToken = parsedToken.ToString(Formatting.Indented);

            writer.WriteRawValue(indentedToken);
        }
        catch
        {
            writer.WriteValue(value);
        }
    }

    public override string? ReadJson
    (
        JsonReader reader,

        Type objectType,

        string? existingValue,

        bool hasExistingValue,

        JsonSerializer serializer
    )
    {
        return reader.Value?.ToString();
    }
}