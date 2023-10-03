using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            DateTime expirationValue = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:Expiration_Minutes"]));

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()), //Subject (user id)
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //JWT unique ID
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),

            }
        }
    }
}
