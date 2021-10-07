namespace JOIEnergy.Application.Exception
{
    public class ApiException : System.Exception
    {
        public ApiException() : base()
        {
        }

        public ApiException(string message) : base(message)
        {
        }
    }
}
