using Serilog;

namespace WFS
{
    public static class Program
    {
        const string COMPANY_NAME = "RoyalPizza";
        const string PRODUCT_NAME = "WFS"; // weather fetch service

        public static void Main(string[] args)
        {
#if DEBUG
            string dataPath = AppDomain.CurrentDomain.BaseDirectory;
#else
            string dataPath = @$"C:\ProgramData\{COMPANY_NAME}\{PRODUCT_NAME}";
            // TODO: implement user for this app, and ensure it only has access to the data path
            // TODO: ensure data path exists
#endif
            string logsPath = Path.Combine(dataPath, "logs/logs.txt");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(logsPath, rollingInterval: RollingInterval.Day, shared: true)
                .Enrich.FromLogContext()
                .CreateLogger();

            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSerilog();
            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<ForcastWorker>();

            var host = builder.Build();
            host.Run();
        }
    }
}