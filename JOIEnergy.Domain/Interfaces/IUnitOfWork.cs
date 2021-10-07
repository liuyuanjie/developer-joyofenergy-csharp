using System.Threading;
using System.Threading.Tasks;

namespace JOIEnergy.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
