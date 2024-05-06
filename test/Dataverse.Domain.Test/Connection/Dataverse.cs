using Dataverse.Http.Connector.Core.Domains.Dataverse.Connection;

namespace Dataverse.Domain.Test.Connection
{
    public static class Dataverse
    {
        public static DataverseConnection Connection
        {
            get
            {
                return new()
                {
                    TenantId = Guid.Empty,
                    ClientId = Guid.Empty,
                    ClientSecret = string.Empty,
                    Resource = string.Empty,
                    ConnectionName = string.Empty,
                };
            }
        }
    }
}
