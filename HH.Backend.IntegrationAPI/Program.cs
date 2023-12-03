using HH.Backend.Common.Web.WireUp;
using HH.Backend.IntegrationAPI.WireUp;

namespace HH.Backend.IntegrationAPI
{
    public class Program
    {
        public static void Main(string[] args) =>
            BaseAppInitializer.StartWithDefaultBuilderWithLogging<Startup>(args, () => IntegrationAPIConfigurationInitializer.GetConfigurationPath());
    }
}
