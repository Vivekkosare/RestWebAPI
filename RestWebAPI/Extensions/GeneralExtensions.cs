using System.Text.Json;

namespace RestWebAPI.Extensions
{
    public static class GeneralExtensions
    {
        public static T GetDeserializedObject<T>(this string content)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
            return JsonSerializer.Deserialize<T>(content, options);
        }
    }
}
