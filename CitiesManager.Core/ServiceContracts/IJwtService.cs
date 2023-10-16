using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user);
    }
}
