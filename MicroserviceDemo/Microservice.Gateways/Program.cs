using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace WebApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. 加载 ocelot.json 配置文件
            // reloadOnChange: true 表示如果我们在运行修改了配置，无需重启即可生效
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            // 2. 注册 Ocelot 服务
            builder.Services.AddOcelot();

            var app = builder.Build();

            // 3. 启用 Ocelot 中间件
            // Wait() 是必须的，因为 Ocelot 的管道是异步构建的
            app.UseOcelot().Wait();

            app.Run();
        }
    }
}