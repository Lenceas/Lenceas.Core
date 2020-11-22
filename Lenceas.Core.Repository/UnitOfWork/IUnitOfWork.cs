using Lenceas.Core.Model;
using System.Threading.Tasks;

namespace Lenceas.Core.Repository
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
