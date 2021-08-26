using EasyX.Infra.Api;

namespace EasyX.Infra.Core
{
    public class RemoteServiceSettings : IRemoteServiceSettings
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
