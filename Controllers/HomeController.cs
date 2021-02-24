using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogWithAuthentication.Models;

namespace BlogWithAuthentication
{
    public class HomeController : Controller
    {
        DataAccess dbAccess = new DataAccess();
        List<PostModel> posts = new List<PostModel>();
        public HomeController()
        {

        }

        public IActionResult Index()
        {
            posts = dbAccess.GetPosts().OrderByDescending(p => DateTime.Parse(p.TimeStamp)).ToList(); 
            return View(posts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PostModel post)
        {
            if (ModelState.IsValid)
            {
                dbAccess.CreateRecord(post);
                posts = dbAccess.GetPosts().OrderByDescending(p => DateTime.Parse(p.TimeStamp)).ToList();
            }

            return View("~/Views/Home/Index.cshtml", posts);
        }

        [HttpPost]
        public IActionResult Delete(PostModel post)
        {
            if (ModelState.IsValid)
            {
                dbAccess.DeleteRecord(post);
                posts = dbAccess.GetPosts().OrderByDescending(p => DateTime.Parse(p.TimeStamp)).ToList();
            }

            return View("~/Views/Home/Index.cshtml", posts);
        }

        [HttpPost]
        public IActionResult UpdateOnUpdatePage(PostModel post)
        {
            return View("~/Views/Home/Update.cshtml", post);
        }

        [HttpPost]
        public IActionResult Update(PostModel post)
        {
            if (ModelState.IsValid)
            {
                dbAccess.UpdateRecord(post);
                posts = dbAccess.GetPosts().OrderByDescending(p => DateTime.Parse(p.TimeStamp)).ToList();
            }

            return View("~/Views/Home/Index.cshtml", posts);
        }
    }
}
