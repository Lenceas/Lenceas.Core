using Lenceas.Core.Common;
using Lenceas.Core.IServices;
using Lenceas.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Lenceas.Core.Extensions.CustomApiVersion;

namespace Lenceas.Core.Controllers
{
    /// <summary>
    /// 权限相关接口
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [CustomRoute(ApiVersions.v1)]
    public class AuthController : ControllerBase
    {
        #region 构造函数
        private readonly IAdministratorServices _administratorServices;
        public AuthController(IAdministratorServices administratorServices)
        {
            _administratorServices = administratorServices;
        }
        #endregion

        /// <summary>
        /// 获取JWT的方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetJwtToken")]
        public async Task<ApiResult<TokenInfoViewModel>> GetJwtToken(string name = "", string pwd = "")
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return new ApiResult<TokenInfoViewModel>()
                {
                    status = 400,
                    msg = "用户名或密码不能为空",
                };
            }
            var administrator = await _administratorServices.GetEntity(t => t.Account.Equals(name) && t.Password.Equals(MD5Helper.MD5Encrypt32(pwd)));
            if (administrator != null)
            {
                //创建声明数组
                var claims = new Claim[] {
                    new Claim(ClaimTypes.Name,administrator.Account),
                    new Claim(JwtRegisteredClaimNames.Email,administrator.Email),
                    new Claim(JwtRegisteredClaimNames.Sub,administrator.Id.ToString())
                    };
                //读取配置文件
                var symmetricKeyAsBase64 = AppSecretConfig.Audience_Secret_String;
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);
                var Issuer = AppSettings.app(new string[] { "Audience", "Issuer" });
                var Audience = AppSettings.app(new string[] { "Audience", "Audience" });
                var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: signingCredentials
                );
                //生成token
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);
                //打包返回前台
                var responseJson = new TokenInfoViewModel
                {
                    token = encodedJwt,
                    expires_in = TimeSpan.FromSeconds(60 * 60).TotalSeconds,
                    token_type = "Bearer"
                };
                return new ApiResult<TokenInfoViewModel>()
                {
                    status = 200,
                    msg = "认证成功",
                    data = responseJson
                };
            }
            else
            {
                return new ApiResult<TokenInfoViewModel>()
                {
                    status = 401,
                    msg = "认证失败",
                };
            }
        }
    }
}
