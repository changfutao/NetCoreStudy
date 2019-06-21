using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPP1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //请求地址就会是 http://xxx/api/User/GetUsers
        [Route("GetUsers")]
        public List<string>GetUsers()
        {
            List<string> list = new List<string>() { "111","222","333"};
            return list;
        }


    }
}