using AutoMapper;
using Lenceas.Core.Common;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Controllers
{
    /// <summary>
    /// 系统管理员接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    [Authorize]
    public class AdministratorController : ControllerBase
    {
        #region 构造函数
        private readonly IAdministratorServices _administratorServices;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _cache;
        public AdministratorController(IAdministratorServices administratorServices, IMapper mapper, IHttpContextAccessor accessor, IMemoryCache memoryCache, IDistributedCache cache)
        {
            _administratorServices = administratorServices;
            _mapper = mapper;
            _accessor = accessor;
            _memoryCache = memoryCache;
            _cache = cache;
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
        [AllowAnonymous]
        public async Task<ApiResult<PageViewModels<AdministratorViewModels>>> GetPage(int pageIndex = 1, int pageSize = 10)
        {
            var r = new ApiResult<PageViewModels<AdministratorViewModels>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<AdministratorViewModels>>(await _administratorServices.GetPage(pageIndex, pageSize)).AsPageViewModel(pageIndex, pageSize);
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
        [AllowAnonymous]
        public async Task<ApiResult<List<AdministratorViewModels>>> GetList()
        {
            var r = new ApiResult<List<AdministratorViewModels>>();
            try
            {
                r.msg = "查询成功";
                r.data = _mapper.Map<List<AdministratorViewModels>>(await _administratorServices.GetList());
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
        [AllowAnonymous]
        public async Task<ApiResult<AdministratorViewModels>> GetById(long id)
        {
            var r = new ApiResult<AdministratorViewModels>();
            try
            {
                var entity = await _administratorServices.GetById(id);
                if (entity != null)
                {
                    r.msg = "查询成功";
                    r.data = _mapper.Map<AdministratorViewModels>(entity);
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
        public async Task<ApiResult<string>> Add([FromBody] AdministratorViewModels model)
        {
            var r = new ApiResult<string>();
            var isNew = await _administratorServices.GetEntity(t => t.Account.Equals(model.Account)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "账号已存在";
                return r;
            }
            try
            {
                r.status = await _administratorServices.AddAsync(new Administrator(model.Account, MD5Helper.MD5Encrypt32(model.Password))) > 0 ? 200 : 400;
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
        public async Task<ApiResult<string>> Update(long id, [FromBody] AdministratorViewModels model)
        {
            var r = new ApiResult<string>();
            if (!id.Equals(model.Id))
            {
                r.status = 400;
                r.msg = "传入Id与实体Id不一致";
                return r;
            }
            var isExist = await _administratorServices.IsExist(id);
            if (!isExist)
            {
                r.status = 404;
                r.msg = "未匹配到数据";
                return r;
            }
            var isNew = await _administratorServices.GetEntity(t => t.Account.Equals(model.Account)) == null;
            if (!isNew)
            {
                r.status = 400;
                r.msg = "账号已存在";
                return r;
            }
            try
            {
                r.status = await _administratorServices.UpdateAsync(t => t.Id == id, t => new Administrator() { Account = model.Account, Password = MD5Helper.MD5Encrypt32(model.Password), UpdateTime = DateTime.Now.ToLocalTime() }) == 0 ? 200 : 400;
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
                var isExist = await _administratorServices.IsExist(id);
                if (isExist)
                {
                    r.status = await _administratorServices.DeleteById(id) > 0 ? 200 : 400;
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
        #endregion
    }
}
