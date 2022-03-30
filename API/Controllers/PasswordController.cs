using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PasswordController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public PasswordController(UserManager<AppUser> userManager)
        {
                _userManager = userManager;
        }

        [HttpGet("action")]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            return View(model);
        }

        [HttpPost("[action]")]    
        public async Task<IActionResult> UpdatePassword([FromBody]ResetPasswordViewModel model)
        {
            var user = await  _userManager.FindByEmailAsync(model.Email.Trim());

            if(user == null)
            {
                return BadRequest("Bad Request");
            }
            else if(!user.EmailConfirmed)
            {
                return BadRequest("Your email address is not confirmed yet to access this service");
            }

            var resetPassword = 
                        await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            
            if(!resetPassword.Succeeded)
            {
                return BadRequest("Unable to reset your password please try again");
            }

            return Ok();
        }

    }
}