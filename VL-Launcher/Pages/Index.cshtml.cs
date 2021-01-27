using System;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VL_Launcher.Pages
{
    public class IndexModel : PageModel
    {

        private static bool Updated = false;

        public void OnGet()
        {
            if (!Updated) {
                Updated = true;
                // TODO: Implementation
            }
        }
    }
}
