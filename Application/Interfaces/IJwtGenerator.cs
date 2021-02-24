using Domain;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        string CreateToken(Domain.Entities.User user);
    }
}