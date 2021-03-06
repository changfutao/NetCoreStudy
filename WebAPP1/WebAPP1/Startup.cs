﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using WebAPP1.AuthHelper;
using WebAPP1.Models;
using static WebAPP1.SwaggerHelper.CustomApiVersion;

namespace WebAPP1
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private const string ApiName = "Blog.Core";
        public void ConfigureServices(IServiceCollection services)
        {
            #region 选项配置IOptionsMonitor、IOptions
            //1.常规选项配置
            services.Configure<MyOptions>(_configuration);
            //2.通过委托配置简单选项(设置默认值)
            //services.Configure<MySubject>(options => 
            //{
            //    options.option1 = "haha1";
            //    options.option2 = "haha2";
            //});

            //3.子选项配置
            services.Configure<MySubject>(_configuration.GetSection("mysubject"));
            #endregion

            #region Swagger UI Service
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                //遍历出全部的版本,做文档信息展示
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version => 
                {
                    c.SwaggerDoc(version, new Info
                    {
                        // {ApiName} 定义成全局变量，方便修改
                        Version = version,
                        Title = $"{ApiName} 接口文档",
                        Description = $"{ApiName} HTTP API " + version,
                        TermsOfService = "None",
                        Contact = new Contact { Name = "Blog.Core", Email = "Blog.Core@xxx.com", Url = "https://www.jianshu.com/u/94102b59cc2a" }
                    });
                    //按相对路径排序
                    c.OrderActionsBy(o => o.RelativePath);
                });
            

                #region 读取xml信息
                var xmlPath = Path.Combine(basePath, "WebAPP1.xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, "WebAPP1.Models.xml"); //这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
                #endregion

                #region Token绑定到ConfigureServices
                //添加header验证信息
                var security=new Dictionary<string, IEnumerable<string>> { { "Blog.Core", new string[] { } } };
                c.AddSecurityRequirement(security);
                //方案名称"Blog.Core"可自定义,上下一致即可
                c.AddSecurityDefinition("Blog.Core", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输)直接在下拉框中输入Bearer {token}(注意两者之间是一个空格)",
                    Name = "Authorization",//jwt默认的参数名称
                    In="header", //jwt默认存放Authorization信息的位置(请求头中)
                    Type="apiKey"
                });
                #endregion


            });
            #endregion

            //ASP.NET Core 依赖注入容器注册服务
            //.AddSingleton()
            //.AddTransient()
            //.AddScoped()
            //依赖注入的亮点
            //低耦合
            //提供了高测试性,使单元测试更加的容易


            #region JWT注入服务
            //1.
            //添加jwt验证:
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = true, //是否验证Issuer
            //                ValidateAudience = true, //是否验证Audience
            //                ValidateLifetime = true, //是否验证失效时间
            //                ClockSkew = TimeSpan.FromSeconds(30),
            //                ValidateIssuerSigningKey = true, //是否验证SecurityKey
            //                ValidAudience = Const.Domain, //Audience
            //                ValidIssuer = Const.Domain, //Issuer 这两项和前面签发jwt的设置一致
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey)) //拿到SecurityKey
            //            };
            //        }); 

            ////2
            ////读取JwtSettings配置
            //services.Configure<JwtSettings>(_configuration.GetSection("JwtSettings"));
            //var jwtSettings = new JwtSettings();
            //_configuration.Bind("jwtSettings", jwtSettings);
            ////设定用哪种方式验证Http Request是否合法
            //services.AddAuthentication(options =>
            //{
            //    //认证的配置
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            ////设定JWT Bearer Token的检查选项
            //.AddJwtBearer(o =>
            //{
            //    //jwt的配置
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer=true, //是否验证Issuer
            //        ValidIssuer = jwtSettings.Issuer,
            //        ValidateAudience =true, //是否验证Audience
            //        ValidAudience = jwtSettings.Audience,
            //        ValidateLifetime =true,//是否验证失效时间
            //        ClockSkew = TimeSpan.FromSeconds(30), //获取或设置验证时间时要应用的时钟偏差
            //        ValidateIssuerSigningKey =true, //是否验证SecurityKey
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            //    };
            //});

            #endregion

            #region MVC + GlobalExceptions
            //注入全局异常捕获
            services.AddMvc(options =>
            {
                //options.Filters.Add(typeof(GlobalExceptionsFilter))
                //配置输出xml格式
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            //.AddMvcCore()方法只会添加最核心的MVC服务
            //.AddMvc()方法添加了所有必需的MVC服务
            //.AddMvc()方法会在内部调用AddMvcCore()方法 
            #endregion

            #region 注入实体类
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
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
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiHelp V1");
                    //路径配置,设置为空,表示直接访问该文件
                    //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，
                    //这个时候去launchSettings.json中把"launchUrl": "swagger/index.html"去掉， 然后直接访问localhost:8001/index.html即可
                    c.RoutePrefix = "";

                });
                #endregion
            }
            else if (env.IsProduction() || env.IsStaging()) //生产环境和模拟环境
            {
                app.UseExceptionHandler("/Error");

                // 在非开发环境中，使用HTTP严格安全传输(or HSTS) 对于保护web安全是非常重要的。
                // 强制实施 HTTPS 在 ASP.NET Core，配合 app.UseHttpsRedirection
                //app.UseHsts();
            }
            //添加jwt验证中间件(使用验证权限的 Middleware)
            //app.UseAuthentication();

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
