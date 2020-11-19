using AutoMapper;
using Lenceas.Core.Model;

namespace Lenceas.Core.Extensions.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<Test, TestViewModels>();
            CreateMap<TestViewModels, Test>();
        }
    }
}
