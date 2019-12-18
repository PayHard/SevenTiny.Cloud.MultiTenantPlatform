﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System.Diagnostics;

namespace SevenTiny.Cloud.DevelopmentWeb.Controllers
{
    public class HomeController : WebControllerBase
    {
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            return Redirect("/Application/Select");
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
