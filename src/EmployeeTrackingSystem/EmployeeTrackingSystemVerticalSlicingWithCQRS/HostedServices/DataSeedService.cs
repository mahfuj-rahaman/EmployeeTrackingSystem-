namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.HostedServices
{
    public class DataSeedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        public DataSeedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<DataInitailizer>();
                await seeder.SeedAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
