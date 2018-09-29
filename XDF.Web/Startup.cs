using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using XDF.Core.Helper.Ajax;
using XDF.Web.Middleware;

namespace XDF.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddXmlSerializerFormatters()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options =>
            {
                options.AddPolicy("any", builder =>
                {
                    builder.AllowAnyOrigin() //允许任何来源的主机访问
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials(); //指定处理cookie
                });
            });
            //services.AddSwaggerGen(c =>
            //{
            //    //配置第一个Doc
            //    c.SwaggerDoc("v1", new Info { Title = "My API_1", Version = "v1" });
            //    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "XDF.Web.XML"));
            //});
            //api参数验证
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new AjaxResultModel<string>
                        {
                            Msg = $"{e.Key}--{e.Value.Errors.First().ErrorMessage}"
                        }).ToArray();

                    return new BadRequestObjectResult(errors);
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            //错误中间件
            app.UseErrorHandling();
            //添加NLog
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");//读取Nlog配置文件
            ////添加Swagger
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = "swagger";
            //});
            //app.UseSwagger();
            
            app.UseMvc();
            app.UseStaticFiles();
        }
    }
}
