using AutoMapper;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Mvc;
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

        public TestController(ITestServices testServices, IMapper mapper)
        {
            _testServices = testServices;
            _mapper = mapper;
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 查询列表
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
                r.success = false;
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 查询单条
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
                    r.msg = "未匹配到数据";
                    r.success = false;
                }
            }
            catch (Exception ex)
            {
                r.success = false;
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] TestViewModels model)
        {
            var r = new ApiResult<string>();
            try
            {
                var entity = new Test(model.Name);
                r.success = await _testServices.AddAsync(entity) > 0;
                r.msg = r.success ? "添加成功" : "添加失败";
            }
            catch (Exception ex)
            {
                r.success = false;
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
                r.msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _testServices.IsExist(id);
            if (!isExist)
            {
                r.msg = "未匹配到数据";
                return r;
            }
            try
            {
                r.success = await _testServices.UpdateAsync(t => t.Id == id, t => new Test() { Name = model.Name }) == 0;
                r.msg = r.success ? "更新成功" : "更新失败";
            }
            catch (Exception ex)
            {
                r.success = false;
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
            var isExist = await _testServices.IsExist(id);
            try
            {
                if (isExist)
                {
                    r.success = await _testServices.DeleteById(id) > 0;
                    r.msg = r.success ? "删除成功" : "删除失败";
                }
                else
                {
                    r.msg = "未匹配到数据";
                    r.success = false;
                }
            }
            catch (Exception ex)
            {
                r.success = false;
                r.status = 500;
                r.msg = ex.Message;
            }
            return r;
        }
        #endregion
    }
}
