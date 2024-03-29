﻿using EntitiesTest.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc;

namespace EntitiesTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EntityController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Entities);
        }
    }
}