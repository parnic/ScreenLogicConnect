using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length >= 2)
            {
                await DoRemoteConnect(args[0], args[1]);
            }
            else
            {
                await DoLocalConnect();
            }
        }

        static async Task DoLocalConnect()
        {
            var servers = await ScreenLogicConnect.FindUnits.Find();
            if (servers != null)
            {
                foreach (var server in servers)
                {
                    if (server.IsValid)
                    {
                        await ConnectToUnit(server);
                        break;
                    }
                }
            }

            if (servers == null || servers.Count == 0 || !servers.Any(x => x.IsValid))
            {
                Console.WriteLine("No local units found.");
            }
        }

        static async Task DoRemoteConnect(string systemName, string systemPassword)
        {
            var unit = await ScreenLogicConnect.RemoteConnect.GetGatewayInfo(systemName);
            if (unit?.IsValid == true)
            {
                await ConnectToUnit(unit, systemPassword);
            }
            else
            {
                Console.WriteLine("Unable to connect to remote unit.");
            }
        }

        static async Task ConnectToUnit(ScreenLogicConnect.EasyTouchUnit server, string? systemPassword = null)
        {
            var connection = new ScreenLogicConnect.UnitConnection();
            if (!await connection.ConnectTo(server, systemPassword))
            {
                Console.WriteLine("Login failed");
                return;
            }

            var status = await connection.GetPoolStatus();
            if (status == null)
            {
                Console.WriteLine("Unable to get pool status.");
                return;
            }

            var config = await connection.GetControllerConfig();
            if (config == null)
            {
                Console.WriteLine("Unable to get controller config.");
                return;
            }

            var degSymbol = config.DegC == 1 ? "C" : "F";
            Console.WriteLine($"Air temp: {status.AirTemp} degrees {degSymbol}");
            var currTempList = status.CurrentTemp;
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
                Console.WriteLine($"Pool temp: {poolTemp} degrees {degSymbol}{(status.IsPoolActive() ? "" : " (Last)")}");
            }
            else
            {
                Console.WriteLine("Couldn't get pool temperature.");
            }
            if (spaTemp != 0)
            {
                Console.WriteLine($"Spa temp: {spaTemp} degrees {degSymbol}{(status.IsSpaActive() ? "" : " (Last)")}");
            }
            else
            {
                Console.WriteLine("Couldn't get spa temperature.");
            }
            Console.WriteLine($"ORP: {status.ORP}");
            Console.WriteLine($"pH: {status.PH / 100.0f:0.00}");
            Console.WriteLine($"Salt: {status.SaltPPM * 50} PPM");
            Console.WriteLine($"Saturation: {status.Saturation / 100.0f}");
        }
    }
}
