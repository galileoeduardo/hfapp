using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Entities;
using System.Security.Claims;
using HFApp.WEB.Services;
using HFApp.WEB.Models.Domain.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Xml;

namespace HFApp.WEB.Controllers
{
    public class FileController : Controller
    {
        private readonly HFDbContext _context;
        private readonly IFileServices _fileServices;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public FileController(HFDbContext context, IFileServices fileServices, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _fileServices = fileServices;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: FileEntities
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.UserEntities.FirstOrDefault(e => e.IdentityUserId.Equals(new Guid(currentUserID)));
            IEnumerable<FileEntity> hfDbContext;

            if (User.IsInRole("SuperUser") || User.IsInRole("Admin"))
            {
                hfDbContext = await _context.FileEntities.Include(f => f.MineTypes).Include(f => f.User).ToListAsync(); 
                
            } else
            {
                hfDbContext = await _context.FileEntities.Include(f => f.MineTypes).Include(f => f.User).Where(e => e.UserId.Equals(user.Id)).ToListAsync();
            }
            
            return View(hfDbContext);
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
        public IActionResult Create(FileDto model)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = _context.UserEntities.FirstOrDefault(e => e.IdentityUserId.Equals(new Guid(currentUserID)));
            model.UserId = (int)user.Id;

            return View(model);
        }

        // POST: FileEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Insert(FileDto model)
        {

            var arrName = model.File.FileName.Split('.');
            string origName = arrName.First();
            string ext = arrName.Last();
            string? strJson = string.Empty;

            model.Title = origName;

            var mine = await _context.MineTypesEntities.FirstOrDefaultAsync(e => e.Extension.EndsWith(ext));

            if (mine == null)
            {
                model.Errors.Add(new ErrorDto()
                {
                    Code = "Mine_Type_Not_Found",
                    Description = $"File type not allowed *.{ext}"
                });
                
                return View("Create", model);
            }
            int mineTypeId;
            model.MineTypesId = (int.TryParse(mine.Id.ToString(), out mineTypeId)) ? mineTypeId : 0;
            await _fileServices.UploadFileAsync(model.File.OpenReadStream(), $"{model.UID}.{ext}");

            if (mineTypeId == 13)
            {
                strJson = await _fileServices.DeserializeObject($"{model.UID}.{ext}");
            }

            if (ModelState.IsValid)
            {
                _context.Add(new FileEntity() { 
                    Title = model.Title,
                     Description = (model.Description == null) ? String.Empty : model.Description,
                     MineTypesId = model.MineTypesId,
                     UID = model.UID,
                     UserId = model.UserId,
                     JsonData = strJson
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(model);
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
            var fileEntity = await _context.FileEntities.Include(b => b.MineTypes).FirstOrDefaultAsync(e => e.Id.Equals(id));
            if (fileEntity != null)
            {
                _context.FileEntities.Remove(fileEntity);
                string filename = fileEntity.UID.ToString() + fileEntity.MineTypes.Extension;
                await _fileServices.DeleteFileAsync(filename.ToString());
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public HttpRequest GetRequest()
        {
            return Request;
        }

        [HttpGet]
        public async Task<ActionResult> Download(string fileName, string origFileName)
        {
            return await _fileServices.Download(fileName,origFileName);
        }
    }
}
