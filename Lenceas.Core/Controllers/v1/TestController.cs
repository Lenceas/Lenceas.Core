using AutoMapper;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    public class TestController : ControllerBase
    {
        #region 构造函数
        private readonly ITestServices _testServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;

        public TestController(ITestServices testServices, IMapper mapper, IHttpContextAccessor accessor)
        {
            _testServices = testServices;
            _mapper = mapper;
            _accessor = accessor;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex">当前页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        [HttpGet("GetPage")]
        public async Task<ApiResult<PageViewModels<TestViewModels>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModels<TestViewModels>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<TestViewModels>>(await _testServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList")]
        public async Task<ApiResult<List<TestViewModels>>> GetList()
        {
            var r = new ApiResult<List<TestViewModels>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<TestViewModels>>(await _testServices.GetList());
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<TestViewModels>> GetById(long id)
        {
            var r = new ApiResult<TestViewModels>();
            try
            {
                var entity = await _testServices.GetById(id);
                if (entity != null)
                {
                    r.msg = "查询成功";
                    r.data = _mapper.Map<TestViewModels>(entity);
                }
                else
                {
                    r.status = 404;
                    r.msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] TestViewModels model)
        {
            var r = new ApiResult<string>();
            try
            {
                r.status = await _testServices.AddAsync(new Test(model.Name)) > 0 ? 200 : 400;
                r.msg = r.status == 200 ? "添加成功" : "添加失败";
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ApiResult<string>> Update(long id, [FromBody] TestViewModels model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.status = 400;
                r.msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _testServices.IsExist(id);
            if (!isExist)
            {
                r.status = 404;
                r.msg = "未匹配到数据";
                return r;
            }
            try
            {
                r.status = await _testServices.UpdateAsync(t => t.Id == id, t => new Test() { Name = model.Name }) == 0 ? 200 : 400;
                r.msg = r.status == 200 ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ApiResult<string>> Delete(long id)
        {
            var r = new ApiResult<string>();
            try
            {
                var isExist = await _testServices.IsExist(id);
                if (isExist)
                {
                    r.status = await _testServices.DeleteById(id) > 0 ? 200 : 400;
                    r.msg = r.status == 200 ? "删除成功" : "删除失败";
                }
                else
                {
                    r.status = 400;
                    r.msg = "未匹配到数据";
                }
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 获取 MiniProfiler Html
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMiniProfilerHtml")]
        public ApiResult<string> GetMiniProfilerHtml()
        {
            var r = new ApiResult<string>();
            try
            {
                r.status = 200;
                r.msg = "查询成功";
                r.data = MiniProfiler.Current.RenderIncludes(_accessor.HttpContext).ToString();
            }
            catch (Exception ex)
            {
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }
        #endregion
    }
}
