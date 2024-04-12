using System.Text;

namespace TestProject.Helpers;

public static class HttpClientExtensions
{
    public static HttpContent ObjToHttpContent<TEntity>(this HttpClient client, TEntity obj)
    {
        var data = JsonHelper.SerializeWithWebDefaults(obj)!;
        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
        return content;
    }
}