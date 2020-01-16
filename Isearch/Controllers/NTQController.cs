using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Isearch.Controllers
{
    [Authorize]
    public class NTQController : Controller
    {
        private readonly IsearchContext _context;

        public NTQController(IsearchContext context)
        {
            _context = context;
        }



        [Authorize(Roles = "质量及安全管理部")]
        // GET: NTQ
        public async Task<IActionResult> Index(DateTime order,string bangong)
        {
            if(bangong!=null&&order != null)
            {
                //要实现按公司、日期显示
                var x = await _context.NTQ.Where(c => c.updatetime > order && c.所在办公区域 == bangong).ToListAsync();
                return View(x);
            }
            return View(await _context.NTQ.ToListAsync());
        }

        // GET: NTQ/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nTQ = await _context.NTQ
                .FirstOrDefaultAsync(m => m.姓名 == id);
            if (nTQ == null)
            {
                return NotFound();
            }

            return View(nTQ);
        }

        // GET: NTQ/Create
        public IActionResult Create()
        {
            return View();
        }

       


        // POST: NTQ/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("姓名,总体满意度,所在办公区域,部门,无线稳定性,SSID,无线下载速度,无线打开速度,无线可访问性,有线稳定性,有线下载速度,有线打开速度,有线可访问性,updatetime,是否使用有线")] NTQ nTQ)
        {
            if (ModelState.IsValid)
            {
                nTQ.姓名 = User.Identity.Name;
                var test = _context.NTQ.Find(nTQ.姓名);

                nTQ.updatetime = DateTime.Now;
                if (test != null)
                {
                    _context.Remove(test);
                    //await _context.SaveChangesAsync();
                }
                _context.Add(nTQ);
                await _context.SaveChangesAsync();
                try { goelkonce();  } catch { }
               
                return RedirectToAction(nameof(finish));
            }
            return View(nTQ);
        }
        public IActionResult finish()
        {
            return View();
        }
        // GET: NTQ/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nTQ = await _context.NTQ.FindAsync(id);
            if (nTQ == null)
            {
                return NotFound();
            }
            return View(nTQ);
        }

        // POST: NTQ/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("姓名,总体满意度,部门,无线稳定性,无线下载速度,无线打开速度,无线可访问性,有线稳定性,有线下载速度,有线打开速度,有线可访问性,updatetime")] NTQ nTQ)
        {
            if (id != nTQ.姓名)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nTQ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NTQExists(nTQ.姓名))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(nTQ);
        }

        // GET: NTQ/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nTQ = await _context.NTQ
                .FirstOrDefaultAsync(m => m.姓名 == id);
            if (nTQ == null)
            {
                return NotFound();
            }

            return View(nTQ);
        }

        // POST: NTQ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nTQ = await _context.NTQ.FindAsync(id);
            _context.NTQ.Remove(nTQ);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NTQExists(string id)
        {
            return _context.NTQ.Any(e => e.姓名 == id);
        }


        public void putwithid(string url, object obj)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            var objstr = JsonConvert.SerializeObject(obj);
            request.ContentType = "application/json;charset=UTF-8";
            byte[] byteData = Encoding.UTF8.GetBytes(objstr);
            int length = byteData.Length;
            request.ContentLength = length;
            Stream writer = request.GetRequestStream();
            writer.Write(byteData, 0, length);
            writer.Close();
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd().ToString();
            //   var obj1 = JObject.Parse(responseString);
        }
        [AllowAnonymous]
        public IActionResult goelk()
        {
            string url = "http://192.168.1.71:9200/ntq/ntq/";
            foreach(var ntq in _context.NTQ)
            {
                string eurl = url + ntq.姓名.Substring(4);
                putwithid(eurl, ntq);
            }
            return Json("ok");
        }

        public string goelkonce()
        {
            string url = "http://192.168.1.71:9200/ntq/ntq/";
            foreach (var ntq in _context.NTQ)
            {
                string eurl = url + ntq.姓名.Substring(4);
                putwithid(eurl, ntq);
            }
            return "ok";
        }
    }
}
