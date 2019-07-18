using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using WebAPP1.Models;

namespace WebAPP1.Controllers
{
    public class TOptionsController : Controller
    {
        private MyOptions myOptions;
        private MySubject _mysubject;
        public TOptionsController(
            IOptionsMonitor<MyOptions>optionsMonitor,
            IOptions<MySubject> options)
        {
            this.myOptions = optionsMonitor.CurrentValue;
            this._mysubject = options.Value;
            
        }
        
        public string Index()
        {
            return this._mysubject.option1+" "+this._mysubject.option2;
        }

        public string Usual()
        {
            return this.myOptions.optiona + " " + this.myOptions.optionb;
        }
        [HttpGet]
        public IActionResult TIndex()
        {
            return View();
        }
    }
}