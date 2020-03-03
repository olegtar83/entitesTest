using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using EntitiesTest.Hubs;
using EntitiesTest.Domain.Entities;

namespace EntitiesTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntityHubController : ControllerBase
    {
        private ConcurrentDictionary<Guid, Entity> _store = new ConcurrentDictionary<Guid, Entity>();

        private readonly IHubContext<EntityHub> _entityHubContext;

        public EntityHubController(IHubContext<EntityHub> entityHubContext)
        {
            _entityHubContext = entityHubContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Entity>> Get()
        {
            return Ok(_store.Values);
        }

    }

}