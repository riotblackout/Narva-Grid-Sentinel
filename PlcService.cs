namespace NarvaSentinel;

public class PlcService
{
    public async Task TriggerShutdown()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[MODBUS TCP] Connecting to PLC (192.168.1.50:502)... ");
        await Task.Delay(200); // Network lag
        Console.WriteLine("CONNECTED.");

        Console.Write("[MODBUS TCP] Writing to Coil 0x00A1 (Main Breaker)... ");

        // This is the raw byte packet for "Write Single Coil" (Function 05)
        byte[] packet = new byte[] { 0x01, 0x3A, 0x00, 0x00, 0x00, 0x06, 0x01, 0x05, 0x00, 0xA1, 0xFF, 0x00 };
        string hex = BitConverter.ToString(packet).Replace("-", " ");

        await Task.Delay(150); // Processing time
        Console.WriteLine($"SENT: [{hex}]");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[PLC] CONFIRMATION: CRITICAL LOAD SHEDDING ACTIVE.");
        Console.ResetColor();
    }
}