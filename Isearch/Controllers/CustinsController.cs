using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Isearch.services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using OfficeOpenXml;
using Nest;
using Microsoft.AspNetCore.Authorization;

namespace Isearch.Controllers
{
    [AllowAnonymous]
    public class CustinsController : Controller
    {
        private readonly IsearchContext _context;
        private readonly IHostingEnvironment env; 
        private ElasticClient client = null;
        public CustinsController(IsearchContext context, IHostingEnvironment env)
        {
            client = new ElasticClient(
                   new ConnectionSettings(new Uri("http://ip.lxgreg.cn:19200")).BasicAuthentication("elastic", "duan1212")
                   );
            _context = context;
            this.env = env;
        }

        // GET: Custins
        public IActionResult Index()
        {

            var url = "http://ip.lxgreg.cn:19200/custin1/doc/_search";
            var data = req.Get(url)["hits"]["hits"];


            var arr = new JArray();

            foreach(var item in data)
            {
                arr.Add(item["_source"]);
            }

            return View(arr);

            //return View(await _context.Custins.ToListAsync());
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IList<IFormFile> files)
        {

            var query = client.Search<Dictionary<string, string>>(s => s.Index("custin2").Size(20000)).Documents;
             
            var BadList = new List<Dictionary<string, dynamic>>();
            long size = 0;
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');
                filename = env.WebRootPath + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                FileInfo xls = new FileInfo(filename);
                using (ExcelPackage package = new ExcelPackage(xls))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    //var List = new List<Dictionary<string, dynamic>>();
                    for(int i = 2; i <= worksheet.Dimension.Rows; i++)
                    {
                        Dictionary<string, dynamic> obj = new Dictionary<string, dynamic>();
                        try
                        {
                            obj = makeObj(worksheet, i);

                            var cuname = obj["公司"];
                            var name = obj["姓名"];
                            var mobile = obj["手机"];

                            var dup = query.Where(c => c.Keys.Contains("公司")).Where(c => 
                            c["公司"] == cuname
                            && c["姓名"]==obj["姓名"]
                            && c["手机"] ==obj["手机"]
                            );
                            if (dup.Count() > 0)
                            {
                                var bobj = BadObj(worksheet, i);
                                bobj.Add("错误原因", "检测到重复");
                                BadList.Add(bobj);
                            }
                            else
                            {
                                    var result = req.Post(obj, "http://ip.lxgreg.cn:19200/custin2/doc");
                            }
                        }
                        catch(Exception ex) {
                            var bobj = BadObj(worksheet, i);
                            bobj.Add("错误原因", ex.Message);
                            BadList.Add(bobj);
                        }

                        //查重
                        //


                    }
                }
            }



            return Json(BadList);
        }

        bool testvoid(object c)
        {

            return false;
        }

        private static Dictionary<string, dynamic> makeObj(ExcelWorksheet worksheet, int i)
        {
            var obj = new Dictionary<string, dynamic>();

            for (int o = 1; o <= worksheet.Dimension.Columns; o++)
            {

                var target = worksheet.Cells[i, o];
                var title = worksheet.Cells[1, o].Text;
                var value = target.Text;
                if (title == "名单日期")
                {
                    var cdate = DateTime.Parse(value);
                    value = cdate.ToString("yyyy-MM-dd");
                }
                obj.Add(title, value);
            }

            return obj;
        }

        private static Dictionary<string, dynamic> BadObj(ExcelWorksheet worksheet, int i)
        {
            var obj = new Dictionary<string, dynamic>();

            for (int o = 1; o <= worksheet.Dimension.Columns; o++)
            {

                var target = worksheet.Cells[i, o];
                var title = worksheet.Cells[1, o].Text;
                var value = target.Text;
                obj.Add(title, value);
            }

            return obj;
        }
        // GET: Custins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var custin = await _context.Custins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custin == null)
            {
                return NotFound();
            }

            return View(custin);
        }

        // GET: Custins/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Custins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,客户名称,电子邮件,地址,名单来源,名单日期,回访情况")] Custin custin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(custin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(custin);
        }

        // GET: Custins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var custin = await _context.Custins.FindAsync(id);
            if (custin == null)
            {
                return NotFound();
            }
            return View(custin);
        }

        // POST: Custins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,客户名称,电子邮件,地址,名单来源,名单日期,回访情况")] Custin custin)
        {
            if (id != custin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(custin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustinExists(custin.Id))
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
            return View(custin);
        }

        // GET: Custins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var custin = await _context.Custins
                .FirstOrDefaultAsync(m => m.Id == id);
            if (custin == null)
            {
                return NotFound();
            }

            return View(custin);
        }

        // POST: Custins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var custin = await _context.Custins.FindAsync(id);
            _context.Custins.Remove(custin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustinExists(int id)
        {
            return _context.Custins.Any(e => e.Id == id);
        }
    }
}
