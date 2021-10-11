using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Models
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
