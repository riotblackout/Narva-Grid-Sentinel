using System.Net.Http.Json;

namespace NarvaSentinel;

public class GridService
{
    private static readonly HttpClient _http = new HttpClient();

    // The Master Endpoint (returns prices for the whole day)
    private const string API_URL = "https://dashboard.elering.ee/api/nps/price";

    public async Task<decimal> GetCurrentPrice()
    {
        try
        {
            Console.Write("[NETWORK] Pinging Elering Grid (EE)... ");

            // 1. GET THE RAW DATA
            var response = await _http.GetFromJsonAsync<EleringResponse>(API_URL);

            // 2. CHECK IF DATA IS VALID
            if (response != null && response.Success && response.Data.ContainsKey("ee"))
            {
                var prices = response.Data["ee"];

                // 3. FIND "RIGHT NOW"
                // We convert current UTC time to Unix Timestamp to match the API
                long nowUnix = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();

                // Find the price block that covers the current hour
                // (Timestamp is the start of the hour, so we look for: Timestamp <= NOW < Timestamp + 3600)
                var currentBlock = prices.FirstOrDefault(p => p.Timestamp <= nowUnix && p.Timestamp + 3600 > nowUnix);

                if (currentBlock != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("CONNECTED.");
                    Console.ResetColor();
                    return currentBlock.Price;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARN: Data valid, but no price found for this hour.");
            Console.ResetColor();
            return -1;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FAILED. {ex.Message}");
            Console.ResetColor();
            return -999; // Network Failure Code
        }
    }
}