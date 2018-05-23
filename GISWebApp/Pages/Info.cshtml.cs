using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GISCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Pages
{
    [Authorize(Roles = "admin")]
    public class InfoModel : PageModel
    {
        private readonly GISContext _context;

        public InfoModel(GISContext context)
        {
            _context = context;
        }

        public Info Info { get; set; }

        public async Task OnGetAsync()
        {
            Info = await this.GetInfoAsync();
        }

        public async Task<IActionResult> OnGetGetInfoAsync()
        {
            var data = await this.GetInfoAsync();

            return await Task.FromResult(new JsonResult(data));
        }

        private async Task<Info> GetInfoAsync()
        {
            var info = await _context.Infoes.LastOrDefaultAsync() ?? new GISCore.Info();

            return new Info
            {
                Temp = info.Temp + GISConst.Temp,
                Hum = info.Hum + GISConst.Hum,
                NH3 = info.NH3 + GISConst.NH3,
                Light = info.Light + GISConst.Light,
                Time = info.Time.ToShortDateString() + " " + info.Time.ToLongTimeString()
            };
        }
    }

    public class Info
    {
        public string Temp { get; set; }
        public string Hum { get; set; }
        public string NH3 { get; set; }
        public string Light { get; set; }
        public string Time { get; set; }
    }
}
