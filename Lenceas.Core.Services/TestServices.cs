using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Lenceas.Core.Repository;

namespace Lenceas.Core.Services
{
    public class TestServices : BaseServices<Test>, ITestServices
    {
        public TestServices(IBaseRepository<Test> baseDal) : base(baseDal)
        {
        }
    }
}
