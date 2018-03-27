using System;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var servers = await ScreenLogicConnect.FindUnits.Find();
            if (servers != null)
            {
                foreach (var server in servers)
                {
                    var connection = new ScreenLogicConnect.UnitConnection();
                    await connection.ConnectTo(server);
                    var status = connection.GetPoolStatus();
                    var config = connection.GetControllerConfig();
                    var degSymbol = config.getM_DegC() == 1 ? "C" : "F";
                    Console.WriteLine($"Air temp: {status.getM_AirTemp()} degrees {degSymbol}");
                    var currTempList = status.getM_CurrentTemp();
                    int poolTemp = 0;
                    int spaTemp = 0;
                    if (currTempList.Length > 0)
                    {
                        poolTemp = currTempList[0];
                    }
                    if (currTempList.Length > 1)
                    {
                        spaTemp = currTempList[1];
                    }
                    if (poolTemp != 0)
                    {
                        Console.WriteLine($"Pool temp: {poolTemp} degrees {degSymbol}{(status.isPoolActive() ? "" : " (Last)")}");
                    }
                    else
                    {
                        Console.WriteLine("Couldn't get pool temperature.");
                    }
                    if (spaTemp != 0)
                    {
                        Console.WriteLine($"Spa temp: {spaTemp} degrees {degSymbol}{(status.isSpaActive() ? "" : " (Last)")}");
                    }
                    else
                    {
                        Console.WriteLine("Couldn't get spa temperature.");
                    }
                    Console.WriteLine($"ORP: {status.getM_ORP()}");
                    Console.WriteLine($"pH: {status.getM_PH() / 100.0f:0.00}");
                    Console.WriteLine($"Salt: {status.getM_SaltPPM() * 50} PPM");
                    Console.WriteLine($"Saturation: {status.getM_Saturation() / 100.0f}");

                    break;
                }
            }
        }
    }
}
