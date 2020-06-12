using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;
using System.IO;
using OfficeOpenXml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace Isearch.Controllers
{
    
    public class TFBController : Controller
    {
        private readonly IsearchContext _context;
        private readonly IHostingEnvironment hostingEnv;

        public TFBController(IsearchContext context, IHostingEnvironment env)
        {
            _context = context;
            hostingEnv = env;
        }

        public IActionResult getpx(string str)
        {
            return Json(_context.Trainings.Where(c => c.整合信息.Contains(str)));
        }

        public IActionResult getxls()
        {
            var xlsdate = _context.TrainingFeedBacks
                .Include(p => p.Training);
            string sWebRootFolder = hostingEnv.WebRootPath;
            string sFileName = DateTime.Now.ToString("yyyyMMddhhmmss") + "培训反馈表.xlsx";
            string filepath = sWebRootFolder + "\\" + sFileName;
            ToCXls(xlsdate, filepath);
            FileContentResult ret = File(System.IO.File.ReadAllBytes(filepath), "application/ms-excel", sFileName);
            System.IO.File.Delete(filepath);
            return ret;
        }
        public void ToCXls(IQueryable<TrainingFeedBack> tfbs, string filepath)
        {
            FileInfo xls = new FileInfo(filepath);
            ExcelPackage package = new ExcelPackage(xls);
            ExcelWorksheet ws = package.Workbook.Worksheets.Add("培训反馈记录");//创建worksheet  
            ws.Column(1).Width = 18;
            ws.Column(2).Width = 18;
            ws.Column(3).Width = 18;
            ws.Column(4).Width = 18;
            ws.Column(5).Width = 18;
            ws.Column(6).Width = 18;
            ws.Cells[1, 1].Value = "序号";
            ws.Cells[1, 2].Value = "培训日期";
            ws.Cells[1, 3].Value = "课程名";
            ws.Cells[1, 4].Value = "讲师";
            ws.Cells[1, 5].Value = "培训时长";
            ws.Cells[1, 6].Value = "培训参与方式";

            for (int i = 7; i < 22; i++)
            {
                ws.Column(i).Width = 18;           
                ws.Cells[1, i].Value = "选项" + (i-6);
            }

            ws.Column(21).Width = 18;
            ws.Cells[1, 21].Value = "填表人姓名";

            ws.Column(22).Width = 32;
            ws.Column(23).Width = 32;
            ws.Column(24).Width = 32;

            int realrow = 2;
            foreach (var tfb in tfbs)
            {
                ws.Cells[realrow, 1].Value = realrow - 1;
                ws.Cells[realrow, 2].Value = tfb.Training.培训时间.ToShortDateString();
                ws.Cells[realrow, 3].Value = tfb.Training.课程名称;
                ws.Cells[realrow, 4].Value = tfb.Training.培训讲师;
                ws.Cells[realrow, 5].Value = tfb.真实培训时间;
                ws.Cells[realrow, 6].Value = tfb.fb31;
                

                for (int o = 1; o < 12; o++)
                {
                    string tagname = "fb" + o;
                    var test = tfb.GetType().GetProperty(tagname).GetValue(tfb);
                    string finalvalue = "";
                    switch (test)
                    {
                        case 1:
                            finalvalue = "差";
                            break;
                        case 2:
                            finalvalue = "一般"; break;
                        case 3:
                            finalvalue = "好"; break;
                        case 4: finalvalue = "很好"; break;
                        default: finalvalue = "出现意外"; break;

                    }
                    ws.Cells[realrow, o+6].Value = finalvalue;
                }


                ws.Cells[realrow, 17].Value = tfb.fb16;
                ws.Cells[realrow, 18].Value = tfb.fb17;
                ws.Cells[realrow, 19].Value = tfb.fb18;
                ws.Cells[realrow, 20].Value = tfb.fb19;
                ws.Cells[realrow, 21].Value = tfb.fb20;
                realrow++;
            }
            package.Save();
        }
        // GET: TFB
        public async Task<IActionResult> Index(string str)
        {
            var isearchContext = _context.TrainingFeedBacks.Include(t => t.Training);
            if (str == null)
            {
                return View(await isearchContext.ToListAsync());
            }
            return View(await isearchContext.Where(c=>c.Training.整合信息.Contains(str)).ToListAsync());
        }
        // GET: TFB/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingFeedBack = await _context.TrainingFeedBacks
                .Include(t => t.Training)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingFeedBack == null)
            {
                return NotFound();
            }

            return View(trainingFeedBack);
        }

        [AllowAnonymous]
        // GET: TFB/Create
        public IActionResult Create()
        {
            ViewData["TrainingID"] = new SelectList(_context.Trainings.Where(c=>c.关闭==false), "Id", nameof(Training.整合信息));
            return View();
        }

        // POST: TFB/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("真实培训时间,Id,fb1,fb2,fb3,fb4,fb5,fb6,fb7,fb8,fb9,fb10,fb11,fb12,fb13,fb14,fb15,fb16,fb17,fb18,fb19,fb20,fb31,fb22,fb23,fb24,fb25,fb26,fb27,fb28,fb29,fb30,TrainingID")] TrainingFeedBack trainingFeedBack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingFeedBack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(finish));
                
            }
            ViewData["TrainingID"] = new SelectList(_context.Trainings, "Id", "Id", trainingFeedBack.TrainingID);
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult finish()
        {
            return View();
        }
        // GET: TFB/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingFeedBack = await _context.TrainingFeedBacks.FindAsync(id);
            if (trainingFeedBack == null)
            {
                return NotFound();
            }
            ViewData["TrainingID"] = new SelectList(_context.Trainings, "Id", "Id", trainingFeedBack.TrainingID);
            return View(trainingFeedBack);
        }

        // POST: TFB/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("真实培训时间,Id,fb1,fb2,fb3,fb4,fb5,fb6,fb7,fb8,fb9,fb10,fb11,fb12,fb13,fb14,fb15,fb16,fb17,fb18,fb19,fb20,fb31,fb22,fb23,fb24,fb25,fb26,fb27,fb28,fb29,fb30,TrainingID")] TrainingFeedBack trainingFeedBack)
        {
            if (id != trainingFeedBack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingFeedBack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingFeedBackExists(trainingFeedBack.Id))
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
            ViewData["TrainingID"] = new SelectList(_context.Trainings, "Id", "Id", trainingFeedBack.TrainingID);
            return View(trainingFeedBack);
        }

        // GET: TFB/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingFeedBack = await _context.TrainingFeedBacks
                .Include(t => t.Training)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingFeedBack == null)
            {
                return NotFound();
            }

            return View(trainingFeedBack);
        }

        // POST: TFB/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingFeedBack = await _context.TrainingFeedBacks.FindAsync(id);
            _context.TrainingFeedBacks.Remove(trainingFeedBack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingFeedBackExists(int id)
        {
            return _context.TrainingFeedBacks.Any(e => e.Id == id);
        }
    }
}
