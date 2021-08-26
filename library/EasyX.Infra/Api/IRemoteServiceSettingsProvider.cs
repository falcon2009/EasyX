namespace EasyX.Infra.Api
{
    public interface IRemoteServiceSettingsProvider
    {
        IRemoteServiceSettings GetHttpClientSettings(string key);
    }
}
