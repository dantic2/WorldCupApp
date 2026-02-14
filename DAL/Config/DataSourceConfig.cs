namespace DAL.Config
{
    public enum DataSourceMode
    {
        Api,
        Json
    }

    public sealed class DataSourceConfig
    {
        public DataSourceMode Mode { get; set; } = DataSourceMode.Api;

        //public string JsonFolder { get; set; } = "Data";
        public string JsonFolder { get; set; } = "Resources\\Data";
    }
}
