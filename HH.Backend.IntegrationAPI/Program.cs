using HH.Backend.Common.Web.WireUp;

namespace HH.Backend.IntegrationAPI
{
    public class Program
    {
        public static void Main(string[] args) =>
            BaseAppInitializer.StartWithDefaultBuilderWithLogging<Startup>(args);
    }
}
