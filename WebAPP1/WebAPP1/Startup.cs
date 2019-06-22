using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace WebAPP1
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //注入MVC
            services.AddMvc(options=> {
                //配置输出xml格式
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            //.AddMvcCore()方法只会添加最核心的MVC服务
            //.AddMvc()方法添加了所有必需的MVC服务
            //.AddMvc()方法会在内部调用AddMvcCore()方法

            #region Swagger
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version ="v0.1.0",
                    Title="Net Core Study",
                    Description="测试",
                    TermsOfService="None",
                    Contact= new Swashbuckle.AspNetCore.Swagger.Contact { Name = "Blog.Core", Email = "Blog.Core@xxx.com", Url = "https://www.jianshu.com/u/94102b59cc2a" }
                });

                var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "WebAPP1.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "WebAPP1.Models.xml"); //这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
            });
            #endregion

            //ASP.NET Core 依赖注入容器注册服务
            //.AddSingleton()
            //.AddTransient()
            //.AddScoped()
            //依赖注入的亮点
            //低耦合
            //提供了高测试性,使单元测试更加的容易
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILogger<Startup> logger)
        {
            //开发环境(Development) 集成环境(Integration) 测试环境(testing) QA验证 模拟环境 生产环境
            if (env.IsDevelopment())
            {
                // 在开发环境中，使用异常页面，这样可以暴露错误堆栈信息，所以不要放在生产环境。
                //必须尽可能的在管道中提早注入 UseDeveloperExceptionPage 中间件，可以拦截异常
                //异常展示包含 Stack Trace,Query String Cookies 和HTTP Headers
                //用于自定义异常页面,可以使用DeveloperExceptionPageOptions对象
                app.UseDeveloperExceptionPage();

                #region Swagger(文档展示)
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    //路径配置,设置为空,表示直接访问该文件
                    //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                    //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
                    c.RoutePrefix = "";

                });
                #endregion
            }
            else if(env.IsProduction() || env.IsStaging()) //生产环境和模拟环境
            {
                app.UseExceptionHandler("/Error");

                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();
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
            app.UseStaticFiles();

            //1.ASP.NET Core默认不支持 静态文件的服务
            //2.默认的静态服务文件夹为wwwroot
            //3.要使用静态文件,必须使用UseStaticFiles()中间件
            //4.要定义默认文件,必须使用UseDefaultFiles()中间件
            //5.默认支持的文件列表: index.html index.html default.html default.htm
            //6.UseDefaultFiles()必须注册在UseStaticFiles()前面
            //7.UseFileServer结合了UseStaticFiles、UseDefaultFiles和UseDirectoryBrowser中间件的功能
            //UseFileServer 结合了 UseStaticFiles 、UseDefaultFiles 和 UseDirectoryBrowser(目录浏览不可以开放给用户)中间件的功能(生产环境不推荐使用)

            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("a.html");

            //app.UseFileServer(fileServerOptions);

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

            //app.Run(async (context) =>
            //{
            //    //await context.Response.WriteAsync("hello world");
            //    //获取当前运行环境的变量值(操作系统的环境变量 < launchSettings.json 优先级 ,如果两个都没有默认是Production)
            //    await context.Response.WriteAsync("Hosting Environment:" + env.EnvironmentName);
            //});


            #region 设置允许跨域
            //设置允许跨域
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
                builder.WithOrigins("http://localhost:6688");
            });
            #endregion

            //使用cookie
            app.UseCookiePolicy();
            //返回错误码
            app.UseStatusCodePages(); //把错误码返回前台,比如404

            //添加MVC中间件(默认模板: '{controller=Home}/{action=Index}/{id?}')
            app.UseMvcWithDefaultRoute();

      


        }
    }
}
