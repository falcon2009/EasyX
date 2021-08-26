using EasyX.Infra.Api;
using EasyX.Infra.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;

namespace EasyX.Infra.Extension
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddRemoteServiceSettingsProviderFromConfig(this IServiceCollection serviceCollection, IConfiguration configuration, string sectionName)
        {
            return serviceCollection.AddSingleton(RemoteServiceSettingsProvider.GetRemoteServiceProviderFromConfiguration(configuration.GetSection(sectionName)));
        }

        public static IHttpClientBuilder AddHttpClientToServiceAndConfigure<TInterface, TImplementation>(this IServiceCollection serviceCollection, string name) where TInterface : class where TImplementation: class, TInterface
        {
            return serviceCollection.AddHttpClient<TInterface, TImplementation>(name, (serviceProvider, client) =>
                             {
                                 IRemoteServiceSettingsProvider remoteServiceSettingsProvider = serviceProvider.GetRequiredService<IRemoteServiceSettingsProvider>();
                                 client.BaseAddress = new Uri(remoteServiceSettingsProvider.GetHttpClientSettings(name).Address);
                             })
                             .ConfigurePrimaryHttpMessageHandler(SetBaseHttpClientHandler);
        }

        #region private
        private static HttpMessageHandler SetBaseHttpClientHandler(IServiceProvider serviceProvider)
        {
            HttpClientHandler httpClientHandler = new()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            return httpClientHandler;
        }
        #endregion
    }
}
