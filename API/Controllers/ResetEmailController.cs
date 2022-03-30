using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ResetEmailController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public ResetEmailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            // var user = await _userManager.FindByEmailAsync("sonalindewar@gmail.com");
            // if(user == null)
            // {
            // }
         
            return RedirectToAction("ResetPassword", "Password", new ResetPasswordViewModel{Email= email, Token= token});
        }
        
    }
}