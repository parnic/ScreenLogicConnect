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
                    if (server.isValid)
                    {
                        await ConnectToUnit(server);
                        break;
                    }
                }
            }

            if (servers == null || servers.Count == 0 || !servers.Any(x => x.isValid))
            {
                Console.WriteLine("No local units found.");
            }
        }

        static async Task DoRemoteConnect(string systemName, string systemPassword)
        {
            var unit = await ScreenLogicConnect.RemoteConnect.GetGatewayInfo(systemName);
            if (unit.isValid)
            {
                await ConnectToUnit(unit, systemPassword);
            }
            else
            {
                Console.WriteLine("Unable to connect to remote unit.");
            }
        }

        static async Task ConnectToUnit(ScreenLogicConnect.EasyTouchUnit server, string systemPassword = null)
        {
            var connection = new ScreenLogicConnect.UnitConnection();
            if (!await connection.ConnectTo(server, systemPassword))
            {
                Console.WriteLine("Login failed");
                return;
            }

            var status = await connection.GetPoolStatus();
            var config = await connection.GetControllerConfig();
            var degSymbol = config.m_DegC == 1 ? "C" : "F";
            Console.WriteLine($"Air temp: {status.m_AirTemp} degrees {degSymbol}");
            var currTempList = status.m_CurrentTemp;
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
            Console.WriteLine($"ORP: {status.m_ORP}");
            Console.WriteLine($"pH: {status.m_PH / 100.0f:0.00}");
            Console.WriteLine($"Salt: {status.m_SaltPPM * 50} PPM");
            Console.WriteLine($"Saturation: {status.m_Saturation / 100.0f}");
        }
    }
}
