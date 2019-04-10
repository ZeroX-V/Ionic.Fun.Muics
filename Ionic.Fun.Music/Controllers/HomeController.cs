using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ionic.Fun.Music.Models;
using Ionic.Fun.MusicSearch.Provider;
using Ionic.Fun.MusicSearch.Model;

namespace Ionic.Fun.Music.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search(string  keyword) {
            MusicProviders provider = MusicProviders.Instance;
            List<MergedSong> list = provider.SearchSongs(keyword, 1, 20);
            foreach (var item in list)
            {
                foreach (var item2 in item.items)
                {
                    item2.url=provider.getDownloadUrl(item2);
                }
            }
            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
