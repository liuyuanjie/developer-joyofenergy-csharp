using System.Threading.Tasks;

namespace JOIEnergy.Domain.Interfaces
{
    public interface IDatabaseInitializer
    {
        Task Seed();
    }
}
