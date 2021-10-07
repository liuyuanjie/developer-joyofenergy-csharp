namespace JOIEnergy.Application
{
    public class ConnectionSettings
    {
        public ConnectionSettings(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
    }
}
