namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailHelper _emailHelper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper, IEmailHelper emailHelper)
        {
            _emailHelper = emailHelper;
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerdto)
        {
            if( await UserExists(registerdto.Username)) return BadRequest("User Name is Taken");
            
            var user = _mapper.Map<AppUser>(registerdto);

            user.UserName = registerdto.Username.ToLower();

            

            var result = await _userManager.CreateAsync(user, registerdto.Password);
             if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if(!roleResult.Succeeded) return BadRequest(result.Errors);

            if(result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Email", new { token, email = user.Email }, Request.Scheme);
                _emailHelper.SendEmail(registerdto.Email,confirmationLink, token, "Confirm Email");
            }
             
            return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };         
        }

         [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

            var emailStatus = await _userManager.IsEmailConfirmedAsync(user);

            if(emailStatus == false)
            return BadRequest("Email is unconfirmed please confirm it first");
            
            if(user == null) return Unauthorized("Invalid UserName");

           var result = await _signInManager.CheckPasswordSignInAsync(user,loginDTO.Password,false);

           if(!result.Succeeded) return Unauthorized();
            
           return new UserDto
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x =>x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            }; 
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }

        [HttpPost("forgotpassword/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email.Trim());
            if(user != null)
            {
                var emailStatus = await _userManager.IsEmailConfirmedAsync(user);
                if(emailStatus == false)
                    return BadRequest("Email is unconfirmed please confirm it first");
                
                if(emailStatus == true)
                {   
                      var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                      var confirmationLink = Url.Action("ResetPassword", "ResetEmail", new { token, email = user.Email }, Request.Scheme);
                      _emailHelper.SendEmail(email.Trim(), confirmationLink, token, "Reset Password");
                }
            }
            else
            {
                    return BadRequest("Invalid Email address");
            }

            return Ok(true);
        }

        [HttpPost("resetPasswordConfirmation")]
        public async Task<ContentResult> ResetPasswordConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync("sonalindewar@gmail.com");
            if(user == null)
            {
                return new ContentResult{
                    ContentType ="text/html",
                    Content ="<html><body>Invalid Email address</body></html>"
                };
            }
         
            return new ContentResult{
                    ContentType ="text/html",
                    Content ="<html><body>Your email address got confirmed you can login in Applicaiton</body></html>"
            };    
        }

        
    }
}

    