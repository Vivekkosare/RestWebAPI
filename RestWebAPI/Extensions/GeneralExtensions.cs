using Newtonsoft.Json;

namespace RestWebAPI.Extensions
{
    public static class GeneralExtensions
    {
        public static T GetDeserializedObject<T>(this string content)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.DeserializeObject<T>(content, settings);
        }
    }
}
