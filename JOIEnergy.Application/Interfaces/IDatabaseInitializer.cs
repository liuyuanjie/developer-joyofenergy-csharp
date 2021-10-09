using System.Threading.Tasks;

namespace JOIEnergy.Application.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task Seed();
    }
}
