using NarvaSentinel;

// 1. CONFIGURATION
GridService elering = new GridService();

// The "Pain Threshold" for Factories
const decimal PRICE_CEILING = 100.0m;

Console.WriteLine("==============================================");
Console.WriteLine("   NARVA GRID SENTINEL (LIVE PROTOCOL)");
Console.WriteLine($"   TARGET: Nord Pool Estonia (EE)");
Console.WriteLine($"   THRESHOLD: {PRICE_CEILING} EUR/MWh");
Console.WriteLine("==============================================\n");

// 2. THE INFINITE LOOP
while (true)
{
    // INSIDE YOUR WHILE LOOP

    // 1. Get the exact Universal Time (UTC)
    DateTime utcNow = DateTime.UtcNow;

    // 2. Find the Estonia Time Zone (Windows ID: "E. Europe Standard Time")
    // This automatically handles the switch between Winter (+2) and Summer (+3)
    TimeZoneInfo estoniaZone = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");

    // 3. Convert
    DateTime estoniaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, estoniaZone);

    // 4. Format
    string timeNow = estoniaTime.ToString("HH:mm:ss");

    // A. FETCH REAL DATA
    decimal currentPrice = await elering.GetCurrentPrice();

    // B. LOGIC GATES
    if (currentPrice > 0)
    {
        Console.Write($"[{timeNow}] MARKET PRICE: {currentPrice} EUR/MWh --> ");

        if (currentPrice > PRICE_CEILING)
        {
            // C. DANGER MODE
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("CRITICAL SPIKE. SHUTDOWN SIGNAL SENT.");
            PlcService plc = new PlcService();
            await plc.TriggerShutdown();

            // (Here is where we would trigger the Modbus Coil)
            Console.ResetColor();
        }
        else
        {
            // D. SAFE MODE
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("STABLE. PRODUCTION ACTIVE.");
            Console.ResetColor();
        }
    }
    else if (currentPrice == -999)
    {
        Console.WriteLine($"[{timeNow}] ...Retrying Connection...");
    }

    Console.WriteLine("----------------------------------------------");

    // E. WAIT
    // Don't spam the API. Check every 10 seconds for the demo.
    await Task.Delay(10000);
}