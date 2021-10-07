using System.Data;

namespace JOIEnergy.Application
{
    public interface IConnection
    {
        IDbConnection OpenConnection();
    }
}
