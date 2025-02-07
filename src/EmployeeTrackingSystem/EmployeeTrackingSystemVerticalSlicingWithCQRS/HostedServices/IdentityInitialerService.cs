namespace EmployeeTrackingSystemVerticalSlicingWithCQRS.HostedServices
{
    public class IdentityInitialerService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        public IdentityInitialerService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IdentityInitailizer>();
                await seeder.SeedAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
