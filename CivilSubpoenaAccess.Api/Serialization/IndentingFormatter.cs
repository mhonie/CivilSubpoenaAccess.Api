using Newtonsoft.Json;

namespace CivilSubpoenaAccess.Api.Serialization;


/// <summary>
/// When an entire POCO is written out, write it out with nice indentations.
/// </summary>
public abstract class IndentingFormatter
{
    private readonly JsonSerializerSettings NullHandler = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    protected string ToJson(object value)
    {
        string prettyJson = 
            
            JsonConvert.SerializeObject(value, Formatting.Indented, NullHandler);

        return prettyJson;
    }
}