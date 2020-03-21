using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Demo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Octokit;
using Octokit.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        //Da le mismo resultado usar el attributo o la validacion en el Action del Controller
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var model = new ReposModel();

            if (User.Identity.IsAuthenticated)
            {
                model.GitHubAvatar = User.FindFirst(a => a.Type == "urn:github:avatar")?.Value;
                model.GitHubLogin = User.FindFirst(a => a.Type == "urn:github:login")?.Value;
                model.GitHubName = User.FindFirst(a => a.Type == ClaimTypes.Name)?.Value;
                model.GitHubUrl = User.FindFirst(a => a.Type == "urn:github:url")?.Value;

                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"), new InMemoryCredentialStore(new Credentials(accessToken)));

                model.Repositories = await github.Repository.GetAllForCurrent();
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> SignOut() 
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
