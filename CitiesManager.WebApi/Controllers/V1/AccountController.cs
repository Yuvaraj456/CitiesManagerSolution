using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public AccountController(SignInManager<ApplicationUser> signInManager,RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

                return Ok(applicationUser);
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
                
                return Ok(new { personName = user.PersonName, email = user.Email});

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


    }
}
