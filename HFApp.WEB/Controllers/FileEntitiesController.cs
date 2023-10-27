using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HFApp.WEB.Data;
using HFApp.WEB.Models.Domain.Entities;

namespace HFApp.WEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileEntitiesController : ControllerBase
    {
        private readonly HFDbContext _context;

        public FileEntitiesController(HFDbContext context)
        {
            _context = context;
        }

        // GET: api/FileEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FileEntity>>> GetFileEntities()
        {
          if (_context.FileEntities == null)
          {
              return NotFound();
          }
            return await _context.FileEntities.ToListAsync();
        }

        // GET: api/FileEntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FileEntity>> GetFileEntity(int? id)
        {
          if (_context.FileEntities == null)
          {
              return NotFound();
          }
            var fileEntity = await _context.FileEntities.FindAsync(id);

            if (fileEntity == null)
            {
                return NotFound();
            }

            return fileEntity;
        }
    }
}
