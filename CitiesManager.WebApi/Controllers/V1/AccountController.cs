using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CitiesManager.WebApi.Controllers.V1
{
    /// <summary>
    /// Account Controller for Login and Logout
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")] 
    [AllowAnonymous] //not require authorization
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <param name="jwtService"></param>
        public AccountController(SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [HttpPost("[action]")]        
        public async Task<ActionResult<ApplicationUser>> PostRegister([FromBody]RegisterDTO registerDTO)
        {
            //validation
            if(!ModelState.IsValid)
            {
               string ErrorMessage = string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                
               return Problem(ErrorMessage);
            }

            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName,
                PhoneNumber = registerDTO.PhoneNumber,               
            };

            IdentityResult result = await _userManager.CreateAsync(applicationUser,registerDTO.Password);

            if (result.Succeeded)
            {
                //signIn user
                await _signInManager.SignInAsync(applicationUser,isPersistent: false);

                AuthenticationResponse authResponse = await _jwtService.CreateJwtToken(applicationUser);

                applicationUser.RefreshToken = authResponse.RefreshToken;
                applicationUser.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(applicationUser);

                return Ok(authResponse);
            }
            else
            {
                string error = string.Join("|", result.Errors.Select(e => e.Description)); //error 1|error 2
                return Problem(error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult>? IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Ok(true);

            return Ok(false);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> PostLogin([FromBody]LoginDTO loginDTO)
        {
            //validation
            if (!ModelState.IsValid)
            {
                string ErrorMessage = string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                return Problem(ErrorMessage);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password,isPersistent:false,lockoutOnFailure:false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDTO.Email);
                
                if (user == null)
                {
                    return NoContent();
                }

                //signIn user
                await _signInManager.SignInAsync(user, isPersistent: false);

                AuthenticationResponse authResponse = await _jwtService.CreateJwtToken(user);

                user.RefreshToken = authResponse.RefreshToken;
                user.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;
                await _userManager.UpdateAsync(user);

                return Ok(authResponse);

            }
            else
            {
                return Problem("Invalid email or Password");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> GetLogOut()
        {
            
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost("generate-new-jwt-token")]
         public async Task<IActionResult> GenerateNewAccessToken([FromBody]TokenModel token)
        {
            if (token == null)
                return BadRequest("Invalid client request");       

            ClaimsPrincipal? Principal = _jwtService.GetPrincipalFromJwtToken(token.Token);

            if (Principal == null)
            {
                BadRequest("invalid jwt access token");
            }

            string? email =  Principal.FindFirstValue(ClaimTypes.Email);

          ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if(user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpirationDateTime <= DateTime.Now)
            {
                return BadRequest("invalid refresh token");
            }

            AuthenticationResponse authResponse =await _jwtService.CreateJwtToken(user);

            user.RefreshToken = authResponse.RefreshToken;

            user.RefreshTokenExpirationDateTime = authResponse.RefreshTokenExpirationDateTime;

            await _userManager.UpdateAsync(user);

            return Ok(authResponse);
        }

    }
}
