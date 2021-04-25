using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Renci.SshNet;
using myWebApp.Services;

namespace myWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly LightService lightService;


        public IndexModel(ILogger<IndexModel> logger, LightService lightService)
        {
            _logger = logger;
            this.lightService = lightService;
        }

        public void OnGet()
        {
            ViewData["LightState"] = lightService.GetLightState();
            ViewData["AutoLight"] = lightService.GetAutoLight();
            Console.WriteLine(DateTime.Now);

            
        }
        
        public IActionResult OnPostToggle(bool enabled)
        {
            this.lightService.SetAutoLight(enabled);
            return RedirectToPage();
        }
        
        
        public IActionResult OnPost(bool on)
        {
            lightService.dataSender(on);
            return RedirectToPage();   
        }

        
    }
}
