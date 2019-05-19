using System.Configuration;
using Topshelf;

namespace CalculatorLib.ServiceApp
{
    internal class Program
    {
        private static void Main()
        {
            var redisConStr = ConfigurationManager.ConnectionStrings["REDIS"].ConnectionString;
            var serviceName = nameof(CalculatorLib) + "-Service";
            HostFactory.Run(x =>
            {
                x.Service<CalculatorLibApp>(s =>
                {
                    s.ConstructUsing(name => new CalculatorLibApp());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.UseNLog();
                x.SetDescription(serviceName);
                x.SetDisplayName(serviceName);
                x.SetServiceName(serviceName);
            });
        }
    }
}