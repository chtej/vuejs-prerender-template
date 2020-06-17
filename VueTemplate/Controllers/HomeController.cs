using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueTemplate.Models;
using VueTemplate.Services;

namespace VueTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpaPrerenderService _prerenderer;

        public HomeController(ISpaPrerenderService prerenderer)
        {
            _prerenderer = prerenderer;
        }
        public async Task<IActionResult> Index()
        {
            var prerenderResult = await _prerenderer.Prerender(new SpaPrerenderRequest
            {
                Domain = $"{Request.Scheme}://{Request.Host.Value}",
                Path = $"{Request.Path.Value}{Request.QueryString.Value}"
            });

            if (prerenderResult.IsNotFound)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
         
            return View(prerenderResult);
        }
    }
}
