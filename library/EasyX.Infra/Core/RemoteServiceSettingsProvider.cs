using EasyX.Infra.Api;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace EasyX.Infra.Core
{
    public sealed class RemoteServiceSettingsProvider : IRemoteServiceSettingsProvider
    {
        public static IRemoteServiceSettingsProvider GetRemoteServiceProviderFromConfiguration(IConfigurationSection configurationSection)
        {
            if (configurationSection == null)
            {
                return null;
            }

            IEnumerable<IRemoteServiceSettings> settingArray = configurationSection.Get<RemoteServiceSettings[]>();

            return GetProviderFromEnumerable(settingArray);
        }
        public static IRemoteServiceSettingsProvider GetProviderFromEnumerable(IEnumerable<IRemoteServiceSettings> settingArray)
        {
            if (settingArray == null)
            {
                return null;
            }

            RemoteServiceSettingsProvider httpClientSettingsProvider = new RemoteServiceSettingsProvider();
            foreach (IRemoteServiceSettings setting in settingArray)
            {
                httpClientSettingsProvider.AddSettings(setting);
            }

            return httpClientSettingsProvider;
        }

        private readonly ConcurrentDictionary<string, IRemoteServiceSettings> settingsStorage = new ConcurrentDictionary<string, IRemoteServiceSettings>();

        public IRemoteServiceSettings GetHttpClientSettings(string key)
        {
            return settingsStorage.GetValueOrDefault(key);
        }

        #region private
        private void AddSettings(IRemoteServiceSettings setting)
        {
            if (setting != null && !settingsStorage.ContainsKey(setting.Name))
            {
                settingsStorage.TryAdd(setting.Name, setting);
            }
        }
        #endregion
    }
}
