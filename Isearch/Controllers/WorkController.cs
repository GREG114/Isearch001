using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;
using Isearch.services;

namespace Isearch.Controllers
{
    public class WorkController : Controller
    {
        private readonly string url = "http://isearch.f3322.net:19200/work/work/";
        private readonly IsearchContext _context;
        private readonly elkctl elkctl;

        public WorkController(IsearchContext context)
        {
            elkctl = new elkctl();
            _context = context;
        }

        // GET: Work
        public async Task<IActionResult> Index()
        {
            return View(await _context.ITWork.ToListAsync());
        }

        // GET: Work/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTWork = await _context.ITWork
                .FirstOrDefaultAsync(m => m.Id == id);
            if (iTWork == null)
            {
                return NotFound();
            }

            return View(iTWork);
        }

        // GET: Work/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Work/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Workclass,Creator,Target,Content,Status,satisfied,Create_at,Update_at,Finish_at")] ITWork iTWork)
        {
            if (ModelState.IsValid)
            {
                if (iTWork.Status == "完成")
                {
                    iTWork.Finish_at = DateTime.Now;
                }
                _context.Add(iTWork);
                await _context.SaveChangesAsync();
                iTWork.Create_at =  iTWork.Create_at.AddHours(-8);
                iTWork.Update_at=  iTWork.Update_at.AddHours(-8);
                if (iTWork.Status == "完成")
                {
                    iTWork.Finish_at= iTWork.Finish_at.AddHours(-8);
                }
                elkctl.putwithid(url + iTWork.Id, iTWork);
                //elkctl.postObj(url, iTWork);
                return RedirectToAction(nameof(Index));
            }
            return View(iTWork);
        }

        // GET: Work/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTWork = await _context.ITWork.FindAsync(id);
            if (iTWork == null)
            {
                return NotFound();
            }
            return View(iTWork);
        }

        // POST: Work/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Workclass,Creator,Target,Content,Status,satisfied,Create_at,Update_at,Finish_at")] ITWork iTWork)
        {
            if (id != iTWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(iTWork);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ITWorkExists(iTWork.Id))
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
            return View(iTWork);
        }

        // GET: Work/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var iTWork = await _context.ITWork
                .FirstOrDefaultAsync(m => m.Id == id);
            if (iTWork == null)
            {
                return NotFound();
            }

            return View(iTWork);
        }

        // POST: Work/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var iTWork = await _context.ITWork.FindAsync(id);
            _context.ITWork.Remove(iTWork);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ITWorkExists(string id)
        {
            return _context.ITWork.Any(e => e.Id == id);
        }
    }
}
