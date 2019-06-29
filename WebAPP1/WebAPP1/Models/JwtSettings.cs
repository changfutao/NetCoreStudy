using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPP1.Models
{
    public class JwtSettings
    {
        /// <summary>
        /// Token颁发者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// TOken给哪些客户端使用
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 加密Key
        /// </summary>
        public string SecretKey { get; set; }
    }
}
