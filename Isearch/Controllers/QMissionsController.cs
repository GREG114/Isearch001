using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Isearch.Models;

namespace Isearch.Controllers
{
    public class QMissionsController : Controller
    {
        private readonly IsearchContext _context;

        public QMissionsController(IsearchContext context)
        {
            _context = context;
        }

        // GET: QMissions
        public async Task<IActionResult> Index()
        {
            return View(await _context.qMissions.ToListAsync());
        }

        // GET: QMissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qMission = await _context.qMissions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qMission == null)
            {
                return NotFound();
            }

            return View(qMission);
        }

        // GET: QMissions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: QMissions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Detail,DateRange,RunTime,DateModel,VerifyMethod,CreateBy,FinishBy,CreateTime,FinishTime,Status")] QMission qMission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qMission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qMission);
        }

        // GET: QMissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qMission = await _context.qMissions.FindAsync(id);
            if (qMission == null)
            {
                return NotFound();
            }
            return View(qMission);
        }

        // POST: QMissions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Detail,DateRange,RunTime,DateModel,VerifyMethod,CreateBy,FinishBy,CreateTime,FinishTime,Status")] QMission qMission)
        {
            if (id != qMission.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qMission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QMissionExists(qMission.Id))
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
            return View(qMission);
        }

        // GET: QMissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qMission = await _context.qMissions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qMission == null)
            {
                return NotFound();
            }

            return View(qMission);
        }

        // POST: QMissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qMission = await _context.qMissions.FindAsync(id);
            _context.qMissions.Remove(qMission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QMissionExists(int id)
        {
            return _context.qMissions.Any(e => e.Id == id);
        }
    }
}
