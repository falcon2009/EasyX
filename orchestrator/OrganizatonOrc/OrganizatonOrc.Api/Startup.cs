using EasyX.Crud.Api.Model;
using EasyX.Http;
using EasyX.Infra;
using EasyX.Infra.Api;
using EasyX.Infra.Core;
using EasyX.Infra.Extension;
using EasyX.WebApi.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Organization.Proxy;
using Organization.Share;
using Organization.Share.Key;
using OrganizationOrc.Share;
using OrganizatonOrc.Api.Service;
using Person.Proxy;
using Person.Share;
using Person.Share.Key;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using UAC.Proxy;
using UAC.Share.Key;

namespace OrganizatonOrc.Api
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

            services.AddControllers();
            services.AddSwaggerGen(SetSwagger);

            #region service
            services.AddHttpContextAccessor();
            services.AddSingleton(SetTypeResolver());
            services.AddRemoteServiceSettingsProviderFromConfig(Configuration, "RemoteService");
            services.AddTransient<IHttpService, StandardHttpClient>();
            services.AddTransient<IOrchestratorService<OrganizationKey>, OrganizationOrgService>();
            services.AddHttpClientToServiceAndConfigure<IModelService<OrganizationKey>, OrganizationServiceProxy>("organization");
            services.AddHttpClientToServiceAndConfigure<IModelService<EmployeeKey>, EmployeeServiceProxy>("organization");
            services.AddHttpClientToServiceAndConfigure<IModelService<PersonKey>, PersonServiceProxy>("person");
            services.AddHttpClientToServiceAndConfigure<IModelService<PersonContactKey>, PersonContactServiceProxy>("person");
            services.AddHttpClientToServiceAndConfigure<IModelService<UserKey>, UserServiceProxy>("uac");
            services.AddHttpClientToServiceAndConfigure<IModelService<RoleKey>, RoleServiceProxy>("uac");
            #endregion;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(SetSwaggerUI);
            }
            app.UseMiddleware<JsonExceptionMiddleware>();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region private
        private static void SetSwagger(SwaggerGenOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option), Constant.Errors.OptionNull);
            }

            AssemblyName assemblyName = Assembly.GetEntryAssembly().GetName();
            string version = assemblyName.Version.ToString();
            string title = assemblyName.Name;

            option.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = version });

            string orgWebApiDocPath = Path.Combine(AppContext.BaseDirectory, "Organization.Api.xml");
            string orgShareDocPath = Path.Combine(AppContext.BaseDirectory, "Organization.Share.xml");
            string webApiDocPath = Path.Combine(AppContext.BaseDirectory, "OrganizationOrc.Api.xml");
            string shareDocPath = Path.Combine(AppContext.BaseDirectory, "OrganizationOrc.Share.xml");
            string systemDataCoreDocPath = Path.Combine(AppContext.BaseDirectory, "EasyX.Data.Core.xml");
            if (File.Exists(orgWebApiDocPath))
            {
                option.IncludeXmlComments(orgWebApiDocPath);
            }
            if (File.Exists(orgShareDocPath))
            {
                option.IncludeXmlComments(orgShareDocPath);
            }
            if (File.Exists(webApiDocPath))
            {
                option.IncludeXmlComments(webApiDocPath);
            }
            if (File.Exists(shareDocPath))
            {
                option.IncludeXmlComments(shareDocPath);
            }
            if (File.Exists(systemDataCoreDocPath))
            {
                option.IncludeXmlComments(systemDataCoreDocPath);
            }
        }
        private static void SetSwaggerUI(SwaggerUIOptions option)
        {
            if (option == null)
            {
                throw new ArgumentNullException(nameof(option), Constant.Errors.OptionNull);
            }

            option.SwaggerEndpoint("/swagger/v1/swagger.json", "Person.Api");
        }
        private ITypeResolver SetTypeResolver()
        {
            TypeResolver typeResolver = new TypeResolver();
            foreach (Type item in PersonShareModelList.ShareModelList)
            {
                typeResolver.RegisterContract(item);
            }
            foreach (Type item in OrganizationShareModelList.ShareModelList)
            {
                typeResolver.RegisterContract(item);
            }
            foreach (Type item in OrganizationOrcShareModelList.ShareModelList)
            {
                typeResolver.RegisterContract(item);
            }
            return typeResolver;
        }
        #endregion
    }
}
