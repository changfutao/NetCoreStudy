using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebAPP1
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 中间件
            //app.Use(async (context, next) =>
            //{
            //    //第一步
            //    logger.LogInformation("MW1:传入请求");
            //    await next();  //调用下一个中间件
            //    //第五步
            //    logger.LogInformation("MW1:传入响应");
            //});

            //app.Use(async (context, next) =>
            //{
            //    //第二步
            //    logger.LogInformation("MW2:传入请求");
            //    await next();  //调用下一个中间件
            //    //第四步
            //    logger.LogInformation("MW2:传入响应");
            //});
            //app.Run 终端中间件
            //app.Run(async (context) => 
            //{
            //    //第三步
            //    await context.Response.WriteAsync("MW3:处理请求并生成响应");
            //    logger.LogInformation("MW3 处理请求并生成响应");
            //});

            #endregion

            #region 静态文件中间件
            //1.ASP.NET Core默认不支持 静态文件的服务
            //2.默认的静态服务文件夹为wwwroot
            //3.要使用静态文件,必须使用UseStaticFiles()中间件
            //4.要定义默认文件,必须使用UseDefaultFiles()中间件
            //5.默认支持的文件列表: index.html index.html default.html default.htm
            //6.UseDefaultFiles()必须注册在UseStaticFiles()前面
            //7.UseFileServer结合了UseStaticFiles、UseDefaultFiles和UseDirectoryBrowser中间件的功能
            //UseFileServer 结合了 UseStaticFiles 、UseDefaultFiles 和 UseDirectoryBrowser中间件的功能(生产环境不推荐使用)
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("a.html");

            app.UseFileServer(fileServerOptions);

            #region 默认清空所有index.html index.htm default.html default.htm,去找a.html
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("a.html");
            ////默认去找a.html
            //app.UseDefaultFiles(defaultFilesOptions); 
            #endregion

            //添加默认文件中间件(必须放在UseStaticFiles前面)
            //index.html index.htm default.html default.htm
            //app.UseDefaultFiles();

            //添加静态文件中间件
            //app.UseStaticFiles(); 
            #endregion

            app.Run(async (context) => 
            {
               await context.Response.WriteAsync("hello world");
            });

      


        }
    }
}
