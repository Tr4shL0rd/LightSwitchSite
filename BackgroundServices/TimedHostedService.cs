using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using myWebApp.Services;


namespace myWebApp.BackgroundServices 
{ 

    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly LightService lightService;


        public TimedHostedService(ILogger<TimedHostedService> logger, LightService lightService)
        {
            _logger = logger;
            this.lightService = lightService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));
            Task.Run(() => lightService.TestConnection());
            
            return Task.CompletedTask;

        }

        private void DoWork(object state)
        {
            var now = DateTime.Now;
            
            
            if (now.Hour == 10 && now.Minute == 02 && now.Second >= 0 && now.Second <= 10)
            {
                lightService.resetIP();
            }

            if (this.lightService.GetAutoLight() == false)
            {
                return;
            }
                    //autolight\\
            if (now.Hour >= 20 && now.Minute == 30 && now.Second >= 0 && now.Second <= 10 && lightService.GetLightState() == true)
            {
                this.lightService.TurnOff();
            }
            else if (now.Hour >= 11 && now.Hour < 21 && lightService.GetLightState() == false)
            {
                this.lightService.TurnOn();
            }
                
        

        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}




