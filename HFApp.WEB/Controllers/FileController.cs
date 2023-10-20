using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Entities;

namespace HFApp.WEB.Controllers
{
    public class FileController : Controller
    {
        private readonly HFDbContext _context;

        public FileController(HFDbContext context)
        {
            _context = context;
        }

        // GET: FileEntities
        public async Task<IActionResult> Index()
        {
            var hFDbContext = _context.FileEntities.Include(f => f.MineTypes).Include(f => f.User);
            return View(await hFDbContext.ToListAsync());
        }

        // GET: FileEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FileEntities == null)
            {
                return NotFound();
            }

            var fileEntity = await _context.FileEntities
                .Include(f => f.MineTypes)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileEntity == null)
            {
                return NotFound();
            }

            return View(fileEntity);
        }

        // GET: FileEntities/Create
        public IActionResult Create()
        {
            ViewData["MineTypesId"] = new SelectList(_context.MineTypesEntities, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.UserEntities, "Id", "Id");
            return View();
        }

        // POST: FileEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UID,Title,Description,MineTypesId,UserId,Id")] FileEntity fileEntity)
        {
            ModelState.Remove("User");
            ModelState.Remove("MineTypes");
            if (ModelState.IsValid)
            {
                _context.Add(fileEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MineTypesId"] = new SelectList(_context.MineTypesEntities, "Id", "Id", fileEntity.MineTypesId);
            ViewData["UserId"] = new SelectList(_context.UserEntities, "Id", "Id", fileEntity.UserId);
            return View(fileEntity);
        }

        // GET: FileEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.FileEntities == null)
            {
                return NotFound();
            }

            var fileEntity = await _context.FileEntities.FindAsync(id);
            if (fileEntity == null)
            {
                return NotFound();
            }
            ViewData["MineTypesId"] = new SelectList(_context.MineTypesEntities, "Id", "Id", fileEntity.MineTypesId);
            ViewData["UserId"] = new SelectList(_context.UserEntities, "Id", "Id", fileEntity.UserId);
            return View(fileEntity);
        }

        // POST: FileEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("UID,Title,Description,MineTypesId,UserId,Id")] FileEntity fileEntity)
        {
            if (id != fileEntity.Id)
            {
                return NotFound();
            }

            ModelState.Remove("User");
            ModelState.Remove("MineTypes");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileEntityExists(fileEntity.Id))
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
            ViewData["MineTypesId"] = new SelectList(_context.MineTypesEntities, "Id", "Id", fileEntity.MineTypesId);
            ViewData["UserId"] = new SelectList(_context.UserEntities, "Id", "Id", fileEntity.UserId);
            return View(fileEntity);
        }

        // GET: FileEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FileEntities == null)
            {
                return NotFound();
            }

            var fileEntity = await _context.FileEntities
                .Include(f => f.MineTypes)
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fileEntity == null)
            {
                return NotFound();
            }

            return View(fileEntity);
        }

        // POST: FileEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.FileEntities == null)
            {
                return Problem("Entity set 'HFDbContext.FileEntities'  is null.");
            }
            var fileEntity = await _context.FileEntities.FindAsync(id);
            if (fileEntity != null)
            {
                _context.FileEntities.Remove(fileEntity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileEntityExists(int? id)
        {
          return (_context.FileEntities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
