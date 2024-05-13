namespace AssetManagementSystemUI
{
    public class HelperAPI
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://localhost:7221"); 
            return Client;
        }
    }
}
