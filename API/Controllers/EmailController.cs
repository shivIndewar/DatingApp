using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class EmailController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public EmailController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email.Trim());
            if(user == null)
            {
                return new ContentResult{
                    ContentType ="text/html",
                    Content ="<html><body>Unable To Confrim your email kindly try again !!</body></html>"
                };
            }
            else{
                    var confirmEmail = await _userManager.ConfirmEmailAsync(user,token);
                if(confirmEmail.Succeeded)
                {
                        return new ContentResult{
                    ContentType ="text/html",
                    Content ="<html><body>Your email address got confirmed you can login in Applicaiton</body></html>"
                    };
                }

            }

            return new ContentResult{
                    ContentType ="text/html",
                    Content ="<html><body>Unable To Confrim your email kindly try again !!</body></html>"
                };
        }
 
    }
}