#nullable enable
using Mochineko.Relent.Result;
using Newtonsoft.Json;

namespace Mochineko.Relent.Extensions.NewtonsoftJson
{
    /// <summary>
    /// A JSON serializer that uses Newtonsoft.Json as <see cref="IResult{T}"/> interfaces.
    /// </summary>
    public static class RelentJsonSerializer
    {
        /// <summary>
        /// Serializes an object to JSON string.
        /// </summary>
        /// <param name="serialized"></param>
        /// <param name="formatting"></param>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IResult<string> Serialize<T>(
            T serialized,
            Formatting formatting = Formatting.Indented,
            JsonSerializerSettings? settings = null)
        {
            try
            {
                var json = JsonConvert.SerializeObject(
                    serialized,
                    formatting,
                    settings);

                if (!string.IsNullOrEmpty(json))
                {
                    return Results.Succeed(json);
                }
                else
                {
                    return Results.FailWithTrace<string>(
                        $"Failed to serialize because serialized JSON of {typeof(T)} was null or empty.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<string>(
                    $"Failed to serialize {typeof(T)} to JSON because of {exception}.");
            }
        }

        /// <summary>
        /// Deserializes a JSON string to an object.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="settings"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IResult<T> Deserialize<T>(
            string json,
            JsonSerializerSettings? settings = null)
        {
            if (string.IsNullOrEmpty(json))
            {
                return Results.Fail<T>(
                    $"Failed to deserialize because JSON string was null or empty.");
            }
            
            try
            {
                var requestBody = JsonConvert.DeserializeObject<T>(
                    json,
                    settings);

                if (requestBody != null)
                {
                    return Results.Succeed(requestBody);
                }
                else
                {
                    return Results.FailWithTrace<T>(
                        $"Failed to deserialize because deserialized object of {typeof(T)} was null.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<T>(
                    $"Failed to deserialize {typeof(T)} from JSON because of {exception}.");
            }
        }
    }
}