namespace kTVCSS.Models
{
    public class WebConfig
    {
        /// <summary>
        /// Application config file
        /// </summary>
        public static WebConfig Instance { get; set; } = new WebConfig();

        /// <summary>
        /// DB connection string
        /// </summary>
        public string DbConnectionString { get; set; }
    }
}
