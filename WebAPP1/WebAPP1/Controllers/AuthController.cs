using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebAPP1.AuthHelper;
using WebAPP1.Models;

namespace WebAPP1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private JwtSettings _jwtSettings;

        public AuthController(IOptionsSnapshot<JwtSettings> jwtSettingAccesser)
        {
            this._jwtSettings = jwtSettingAccesser.Value;
        }
        public IActionResult Get(string username,string pwd)
        {
            if(!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(pwd))
            {
                var claims = new[] 
                {
                    new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                    new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddMinutes(30)).ToUnixTimeSeconds()}"),
                    new Claim(ClaimTypes.Name,username)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Const.SecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(issuer: Const.Domain, audience: Const.Domain, claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: creds);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return BadRequest(new { Memory="username or password is incorrect" });
            }
        }

        public IActionResult Login(string username,string pwd)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(!(username =="changfutao" && pwd == "123456"))
            {
                return BadRequest("用户名或密码错误");
            }
            //建立使用者得Clamins声明,这会是JWT Payload得一部分
            var claimsIdentity=new ClaimsIdentity(new[] {
                new Claim(JwtRegisteredClaimNames.NameId,username),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            });
            //对称式加密JWT Signature
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            //建立JWT TokenHandler以及用于描述JWT得TokenDescriptor
            var tokenHandler= new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer=_jwtSettings.Issuer,
                Audience=_jwtSettings.Audience,
                Subject=claimsIdentity,
                Expires=DateTime.Now.AddMinutes(5),
                SigningCredentials= new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            };
            //生成所需要得JWT Token物件
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //生成序列化得JWT Token字符串
            var serializeToken = tokenHandler.WriteToken(securityToken);
            return new ContentResult() { Content=serializeToken};
        }
    }
}