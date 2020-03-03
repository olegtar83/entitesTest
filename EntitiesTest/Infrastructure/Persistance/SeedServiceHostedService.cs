using EntitiesTest.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntitiesTest.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace EntitiesTest.Infrastructure.Persistance
{
    public class SeedServiceHostedService : BackgroundService
    {
        private readonly ILogger<SeedServiceHostedService> _logger;
        const double MIN_VALUE = -1;
        const double MAX_VALUE = 1;
        const double PERCISSION = 5;
        const int PARAMETERS = 20;

        public SeedServiceHostedService(IServiceProvider services,
            ILogger<SeedServiceHostedService> logger,
            IHubContext<EntityHub> entityHubContext)
        {
            Services = services;
            _logger = logger;
            _entityHubContext = entityHubContext;
        }

        public IServiceProvider Services { get; }
        private readonly IHubContext<EntityHub> _entityHubContext;


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service running.");

            await DoWork(stoppingToken);
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is working.");

            using (var scope = Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                while (!stoppingToken.IsCancellationRequested)
                {
                    var name = nameof(Entity) + (context.Entities.Count() + 1);
                    var random = new Random(Environment.TickCount);
                    var entity = new Entity
                    {
                        Name = name,
                        Parameters = Enumerable.Range(0, PARAMETERS)
                        .Select(i => random
                            .Next((int)-Math.Pow(10, PERCISSION), (int)Math.Pow(10, PERCISSION)) / Math.Pow(10, PERCISSION))
                        .ToList()
                    };
                    await context.AddAsync(entity);
                    await context.SaveChangesAsync(stoppingToken);
                    await _entityHubContext.Clients.All.SendAsync("entityAdded", entity);
                    await Task.Delay(10000, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Consume Scoped Service Hosted Service is stopping.");

            await Task.CompletedTask;
        }
    }
}
