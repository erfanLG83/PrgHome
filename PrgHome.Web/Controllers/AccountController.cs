using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrgHome.DataLayer.IdentityClasses;
using PrgHome.DataLayer.IdentityServices;
using PrgHome.Web.Models;

namespace PrgHome.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signIn;
        private readonly IAppUserManager _userManager;
        public AccountController(SignInManager<AppUser> signIn, IAppUserManager userManager)
        {
            _userManager = userManager;
            _signIn = signIn;
        }
        [Route("{Controller}/{Action}")]
        [Route("{Action}")]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginUserDto { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync(userDto.UserName);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(userDto.UserName);
                }
                if (user != null)
                {
                    var result = await _signIn.PasswordSignInAsync(user, userDto.Password, true, user.LockoutEnabled);
                    if (result.Succeeded)
                    {
                        if (userDto.ReturnUrl != null)
                            return Redirect(userDto.ReturnUrl);
                        return Redirect("/");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("", "حساب شما قفل شده است . لطفا بعد چند 5 دقیقه دوباره تلاش کنید");
                    }
                    else if (result.IsNotAllowed)
                    {
                        ModelState.AddModelError("", "شما مجاز به ورود نیستید");
                    }
                    else
                    {
                        ModelState.AddModelError("", "کلمه عبور اشتباه است");
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"حسابی با نام کاربری یا ایمیل '{userDto.UserName}' پیدا نشد !");
                }
            }
            return View(userDto);
        }
        [Route("Signout")]
        public async Task<IActionResult> SignOut()
        {
            await _signIn.SignOutAsync();
            return Redirect("/");
        }
    }
}