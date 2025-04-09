using Newtonsoft.Json;
using RestWebAPI.Entities;

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

        public static Phone MapPhone(this Phone existingPhone, Phone updatedPhone)
        {
            existingPhone.Name = updatedPhone.Name;
            existingPhone.Data = updatedPhone.Data;
            return existingPhone;
        }
    }
}
