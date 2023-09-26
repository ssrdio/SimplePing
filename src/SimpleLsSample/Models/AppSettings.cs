namespace SimpleLsSample.Models
{
    public class AppSettings
    {
        public AppSettings()
        {
            StoreEndpoint = new ApiEndpoint();
        }
        public string BaseUrl { get; set; }

        public ApiEndpoint StoreEndpoint { get; set; }      

        public bool IgnoreEmails { get; set; }
    }
}
