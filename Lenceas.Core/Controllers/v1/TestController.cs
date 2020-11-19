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
        private readonly ITestServices _testServices;
        private readonly IMapper _mapper;

        public TestController(ITestServices testServices, IMapper mapper)
        {
            _testServices = testServices;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResult<List<TestViewModels>>> Get()
        {
            return new ApiResult<List<TestViewModels>>()
            {
                msg = "查询成功",
                data = _mapper.Map<List<TestViewModels>>(await _testServices.Select())
            };
        }

        /// <summary>
        /// 查询单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ApiResult<TestViewModels>> Get(long id = 0)
        {
            var r = new ApiResult<TestViewModels>();
            var entity = await _testServices.GetEntity(t => t.Id == id);
            if (entity != null)
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<TestViewModels>(entity);
            }
            else
            {
                r.msg = "未匹配到数据";
                r.status = 404;
                r.success = false;
            }
            return r;
        }

        // POST api/<TestController>
        [HttpPost]
        public async Task<ApiResult<string>> Add([FromBody] TestViewModels model)
        {
            var r = new ApiResult<string>();
            var entity = new Test(model.Name);
            try
            {
                r.success = await _testServices.Insert(entity) == 1;
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
            var entity = await _testServices.GetEntity(t => t.Id == id);
            if (entity == null)
            {
                r.msg = "未匹配到数据";
                return r;
            }
            entity.Name = model.Name;
            try
            {
                r.success = await _testServices.Update(entity) == 1;
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
        public async Task<ApiResult<string>> Delete(long id = 0)
        {
            var r = new ApiResult<string>();
            var entity = await _testServices.GetEntity(t => t.Id == id);
            if (entity != null)
            {
                try
                {
                    r.success = await _testServices.Delete(t => t.Id == id) == 1;
                    r.msg = r.success ? "删除成功" : "删除失败";
                }
                catch (Exception ex)
                {
                    r.success = false;
                    r.status = 500;
                    r.msg = ex.Message;
                }
            }
            else
            {
                r.msg = "未匹配到数据";
                r.success = false;
            }
            return r;
        }
    }
}
