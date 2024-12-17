using DTOLayer.Version;

namespace ServiceLayer.Version
{
    public class VersionService
    {
        public string GetVersion()
        {
            return VersionApiRestDTO.versionStr;
        }
    }
}
