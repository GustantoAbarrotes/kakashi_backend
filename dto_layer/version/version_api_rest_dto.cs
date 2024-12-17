namespace DTOLayer.Version
{
    public class VersionApiRestDTO
    {
        private const int year = 2024;
        private const int month = 12;
        private const int day = 17;
        private const int compile = 00;
        public static string versionStr
        {
            get => string.Format("{0}.{1}.{2}:{3}", 
                    (year % 100).ToString()
                    , month.ToString("00")
                    , day.ToString("00")
                    , compile.ToString("00")
                );
        }
    }
}
