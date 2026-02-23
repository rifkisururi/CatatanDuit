using CatatanDuit.Api.Models;

namespace CatatanDuit.Api.Security;

public interface IJwtTokenGenerator
{
    string CreateToken(User user);
}
