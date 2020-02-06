using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Isearch.Models;

namespace Isearch.Controllers
{
    [Authorize]
    public class KnowlagesController : Controller
    {
        private readonly IsearchContext _context;
        private readonly IHostingEnvironment env;

        public KnowlagesController(IsearchContext context, IHostingEnvironment env)
        {
            this.env = env;
            _context = context;
        }

        // GET: Knowlages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Knowlage.ToListAsync());
        }

        // GET: Knowlages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knowlage = await _context.Knowlage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (knowlage == null)
            {
                return NotFound();
            }

            return View(knowlage);
        }

        // GET: Knowlages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Knowlages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Detail,CreateTime,ModifyTime")] Knowlage knowlage)
        {
            if (ModelState.IsValid)
            {
                knowlage.username = User.Identity.Name;
                _context.Add(knowlage);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(knowlage);
        }

        // GET: Knowlages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knowlage = await _context.Knowlage.FindAsync(id);
            if (knowlage == null)
            {
                return NotFound();
            }
            return View(knowlage);
        }

        // POST: Knowlages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Detail,CreateTime,ModifyTime")] Knowlage knowlage)
        {
            if (id != knowlage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(knowlage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KnowlageExists(knowlage.Id))
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
            return View(knowlage);
        }

        // GET: Knowlages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knowlage = await _context.Knowlage
                .FirstOrDefaultAsync(m => m.Id == id);
            if (knowlage == null)
            {
                return NotFound();
            }

            return View(knowlage);
        }

        // POST: Knowlages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knowlage = await _context.Knowlage.FindAsync(id);
            _context.Knowlage.Remove(knowlage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnowlageExists(int id)
        {
            return _context.Knowlage.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult OnPostUpload()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                var rError = new
                {
                    uploaded = false,
                    url = string.Empty
                };
                return Json(rError);
            }
            var formFile = files[0];
            var upFileName = formFile.FileName;
            //大小，格式校验....
            var fileName = Guid.NewGuid() + Path.GetExtension(upFileName);
            var saveDir = env.WebRootPath;
            var savePath = env.WebRootPath + $@"\upload\{fileName}";
            var previewPath = "/upload/" + fileName;//

            bool result = true;
            try
            {
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }
                using (FileStream fs = System.IO.File.Create(savePath))
                {
                    formFile.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch
            {
                result = false;
            }
            var rUpload = new
            {
                url = result ? previewPath : string.Empty
            };
            return Json(rUpload);

        }
    }
}
