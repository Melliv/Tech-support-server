using System.Text.Json;

namespace TestProject.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public static async Task<TValue?> DeserializeWithWebDefaults<TValue>(HttpContent content)
    {
        var json = await content.ReadAsStringAsync();
        var obj = JsonSerializer.Deserialize<TValue>(json, JsonSerializerOptions);
        return obj;
    }

    public static string? SerializeWithWebDefaults<TValue>(TValue obj)
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }
}