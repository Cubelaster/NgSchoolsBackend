using Microsoft.Extensions.Configuration;

namespace NgSchoolsBusinessLayer.Utilities.DbUtils.Contracts
{
    public interface IDbInitializer
    {
        void Initialize(IConfigurationRoot Configuration);
    }
}
