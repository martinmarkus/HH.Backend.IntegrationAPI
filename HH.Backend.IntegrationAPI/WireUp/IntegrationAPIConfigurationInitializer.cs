using HH.Backend.Common.Core.Constants;
using HH.Backend.Common.Utils.Assembly;

namespace HH.Backend.IntegrationAPI.WireUp
{
    public class IntegrationAPIConfigurationInitializer
    {
        public static string GetConfigurationPath()
        {
            var assembly = AssemblyUtils.GetByName($"{AssemblyConstants.BaseAssemblyName}.Web");
            string assemblyNamespace = assembly?.GetName()?.Name ?? string.Empty;
            string dir = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory() ?? string.Empty)?.FullName)?.FullName ?? string.Empty;

            return $"{dir}\\{AssemblyConstants.BaseAssemblyName}\\{assemblyNamespace}\\Configurations";
        }
    }
}
