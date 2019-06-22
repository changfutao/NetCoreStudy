using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPP1.Models;

namespace WebAPP1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        //请求地址就会是 http://xxx/api/User/GetUsers
        [Route("GetUsers")]
        [HttpGet]
        public ObjectResult GetUsers()
        {
            List<string> list = new List<string>() { "111","222","333"};
            return new ObjectResult(list);
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        [Route("GetStr")]
        [HttpGet]
        public string GetStr(int id,string a)
        {
            return id.ToString()+" "+a;
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("AddUser")]
        [HttpPost]
        public IActionResult AddUser([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Json(new { isSuccess=true});
            }
        }

    }
}