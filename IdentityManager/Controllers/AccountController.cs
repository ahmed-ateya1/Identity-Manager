using IdentityManager.Models.IdntityEntity;
using IdentityManager.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new ApplicationUser()
            {
                Name = registerViewModel.Name,
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };
            var result = await _userManager.CreateAsync(user , registerViewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            AddError(result);
            return View(registerViewModel);
        }
        [HttpGet]
        public IActionResult Login( string returnURL = null)
        {
            var login = new LoginViewModel();
            ViewBag.ReturnUrl = returnURL;
            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginView , string returnURL = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnURL;
                return View(loginView);
            }

            var result = await _signInManager.PasswordSignInAsync(loginView.Email, loginView.Password,isPersistent: loginView.RememberMe,lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if(returnURL != null && Url.IsLocalUrl(returnURL))
                {
                    return LocalRedirect(returnURL);
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ReturnUrl = returnURL;
            ModelState.AddModelError(String.Empty, "Email or password not correct");
            return View(loginView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Register");
        }
        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
