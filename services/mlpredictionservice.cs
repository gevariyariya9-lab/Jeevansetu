using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Text.Json;

namespace JeevanSetu.Services;

public class MlPredictionService
{
    private readonly HttpClient _httpClient;

    public MlPredictionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<MlPredictionResponse> PredictAsync(
        MlPredictionRequest request,
        CancellationToken cancellationToken = default)
    {
        ValidateRequest(request);

        using var response = await _httpClient.PostAsJsonAsync(
            "predict",
            request,
            cancellationToken);

        var responseBody = await response.Content.ReadAsStringAsync(
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"ML service returned HTTP {(int)response.StatusCode}: " +
                responseBody);
        }

        var result = JsonSerializer.Deserialize<MlPredictionResponse>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return result
            ?? throw new InvalidOperationException(
                "The ML service returned an empty response.");
    }

    private static void ValidateRequest(MlPredictionRequest request)
    {
        if (request.Recency < 0 ||
            request.Frequency < 1 ||
            request.Monetary < 0 ||
            request.Time < 1)
        {
            throw new ArgumentException(
                "Input values are outside the permitted range.");
        }

        if (request.Frequency > request.Time)
        {
            throw new ArgumentException(
                "Frequency cannot exceed Time.");
        }

        if (request.Monetary != request.Frequency * 250)
        {
            throw new ArgumentException(
                "Monetary must equal Frequency multiplied by 250.");
        }
    }
}

public class MlPredictionRequest
{
    [JsonPropertyName("Recency")]
    public int Recency { get; set; }

    [JsonPropertyName("Frequency")]
    public int Frequency { get; set; }

    [JsonPropertyName("Monetary")]
    public int Monetary { get; set; }

    [JsonPropertyName("Time")]
    public int Time { get; set; }
}

public class MlPredictionResponse
{
    public int Prediction { get; set; }
    public double Probability { get; set; }
    public string Meaning { get; set; } = string.Empty;
    public string Notice { get; set; } = string.Empty;
}
