using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Isearch.Models;
using LxGreg.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Isearch.Controllers
{
    public class ADController : Controller
    {
        [AllowAnonymous]
        public IActionResult ChangePasswd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Chan2gePasswd([Bind("userid,pd,newpd")] pdc c)
        {

            return Json(c.userid);
        }

        // POST: OpLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePasswd([Bind("userid,pd,newpd,newpd2")] pdc c)
        {

            if (c.newpd != c.newpd2)
            {
                return Json("两次新密码不一致，清重新输入");
            }

            AdHelper adHelper = new AdHelper("radmin", "rer0y%");
            string result = adHelper.resetpassword(c.userid,c.pd, c.newpd); 


            return Json(result);
        }

        public IActionResult ApiCU([FromBody]adu adu)
        {
            AdHelper ad = new AdHelper("radmin","rer0y%");
            var re = ad.CreateNewUser(adu.login, adu.name, adu.depart,adu.ddid, adu.email);
            return Json(re);
        }


        
    }
}