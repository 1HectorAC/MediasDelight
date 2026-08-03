
using System.Text;
using System.Text.Json;

namespace MediasDelight.Web.Services;

public class GeminiService
{
    private readonly HttpClient _http;

    public GeminiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> GenerateTextAsync(string prompt)
    {
        var body = new { model="gemini-3.6-flash", input = prompt};
        //var json = JsonSerializer.Serialize(body);

        var response = await _http.PostAsync(
            "interactions",
            JsonContent.Create(body)
        );

        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        var result = doc.RootElement
            .GetProperty("steps")[1]
            .GetProperty("content")[0]
            .GetProperty("text")
            .GetString();

        //Console.WriteLine(result);
        return result ?? "";
    }
}