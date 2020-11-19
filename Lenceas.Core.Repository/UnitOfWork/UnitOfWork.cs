using Lenceas.Core.Model;
using System.Threading.Tasks;

namespace Lenceas.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MySqlContext _context;

        public UnitOfWork(MySqlContext context)
        {
            _context = context;
        }

        public MySqlContext GetDbContext()
        {
            return _context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
