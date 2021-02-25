using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class AdministratorServices : BaseServices<Administrator>, IAdministratorServices
    {
        public AdministratorServices(IBaseRepository<Administrator> baseDal) : base(baseDal)
        {
        }
    }
}
