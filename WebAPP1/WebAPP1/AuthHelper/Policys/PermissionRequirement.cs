using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP1.AuthHelper.Policys
{
    /// <summary>
    /// 必要参数类,类似一个订单信息
    /// 继承IAuthorizationRequirement 用于设计自定义权限处理器PermissionHandler
    /// 因为AuthorizationHandler中的泛型参数TRequirement 必须继承 IAuthorizationRequirement
    /// </summary>
    public class PermissionRequirement
    {
        public PermissionRequirement(string deniedAction, List<PermissionItem> permissions, string claimType, string issuer, string audience, SigningCredentials signingCredentials, TimeSpan expiration)
        {
            ClaimType = claimType;
            DeniedAction = deniedAction;
            Permissions = permissions;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }

        /// <summary>
        /// 用户权限集合,一个订单包含了很多详情
        /// 同理,一个网站的认证发行中，也有很多权限详情(这里是Role和URL的关系)
        /// </summary>
        public List<PermissionItem> Permissions { get; set; }
        /// <summary>
        /// 无权限action
        /// </summary>
        public string DeniedAction { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        public string loginPath { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}
