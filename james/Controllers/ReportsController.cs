using james.Models.DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace james.Controllers
{
    public class ReportsController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ReportsController(IWebHostEnvironment hostingEnvironment, DbContextOptions<DBContext> _dbOptions) : base(_dbOptions, hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            this.dbOption = _dbOptions;
        }

        public IActionResult Likes()
        {
            return View();
        }
        public IActionResult HiddenAlbum()
        {
            return View();
        }
        public IActionResult Streaming()
        {
            return View();
        }
        public IActionResult VideoCalls()
        {
            return View();
        }
        public IActionResult Following()
        {
            return View();
        }
        public IActionResult BillingReport()
        {
            return View();
        }
        public IActionResult Videos()
        {
            return View();
        }
        public IActionResult WithdrawalDiamonds()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }
    }
}
