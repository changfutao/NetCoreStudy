using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPP1.AuthHelper.OverWrite;
using WebAPP1.Common;
using WebAPP1.Models;

namespace WebAPP1.AuthHelper
{
    public class JwtHelper
    {
        public static string IssueJwt(TokenModelJwt tokenModel)
        {
            string audience = AppSettings.app(new string[] { "JwtSettings", "Audience" });
            string issuer = AppSettings.app(new string[] { "JwtSettings", "Issuer" });
            string secretKey = AppSettings.app(new string[] { "JwtSettings", "SecretKey" });

            List<Claim> claims = new List<Claim>()
            {
                //JWT ID，Token 的唯一识别码
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                // Issued At，Token 的建立時間
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                // Not Before，定義在什麼時間之前，不可用
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //这个就是过期时间，目前是过期1000秒，可自定义，注意JWT有自己的缓冲过期时间
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                //Issuer，發送 Token 的發行者
                new Claim(JwtRegisteredClaimNames.Iss,issuer),
                //Audience，接收 Token 的使用者
                new Claim(JwtRegisteredClaimNames.Aud,audience)
            };

            //可以将一个用户得多个角色全部赋予
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken=jwtHandler.ReadJwtToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

            var tm = new TokenModelJwt()
            {
                Uid=long.Parse(jwtToken.Id),
                Role=role != null?role.ToString():""
            };
            return tm;
        }
    }
}
