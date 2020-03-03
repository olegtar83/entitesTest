using EntitiesTest.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace EntitiesTest.Hubs
{
    public class EntityHub : Hub
    {
        public async Task EntityAdded(Entity entity)
        {
            await Clients.All.SendAsync("entityAdded", entity);
        }
    }
}
