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
                    break;
                }
            }
        }
    }
}
