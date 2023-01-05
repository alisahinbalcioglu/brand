namespace BrandService;

class Program
{
    public static void Main(string[] args)
    {
        var host = new HostBuilder()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();
                webBuilder.UseIISIntegration();
                webBuilder.UseStartup<Startup>();
            })
            .Build();
        
        host.Run();
    }
}