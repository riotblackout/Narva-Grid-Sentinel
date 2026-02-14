using System.Text.Json.Serialization;

namespace NarvaSentinel;

// 1. The Wrapper (The box the data comes in)
public class EleringResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("data")]
    public Dictionary<string, List<PriceData>> Data { get; set; }
}

// 2. The Price Point (The actual numbers)
public class PriceData
{
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; } // Unix Time (Machine language)

    [JsonPropertyName("price")]
    public decimal Price { get; set; } // The Money (EUR/MWh)
}