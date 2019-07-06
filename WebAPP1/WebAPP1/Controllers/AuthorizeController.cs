using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPP1.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;
using WebAPP1.Models;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPP1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        private JwtSettings _jwtSetting;

        public AuthorizeController(IOptionsSnapshot<JwtSettings> jwtSettingAccesser)
        {
            this._jwtSetting = jwtSettingAccesser.Value;
        }
        [Route("Token")]
        public IActionResult Token(LoginViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            //验证用户名和密码
            if(!(viewModel.UserName =="changfutao" && viewModel.PassWord=="123456"))
            {
                return BadRequest();
            }

            //生成Token
            //claims是什么，用来干什么???
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,"changfutao"),
                new Claim(ClaimTypes.Role,"admin")
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.SecretKey));
            //key 和 算法名称
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token=new JwtSecurityToken(_jwtSetting.Issuer,
                                 _jwtSetting.Audience,
                                 claims,
                                 DateTime.Now,
                                 DateTime.Now.AddMinutes(5),
                                 creds);

            //

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }
}