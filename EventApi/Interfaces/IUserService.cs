using EventApi.DTO;

namespace EventApi.Interfaces
{
    public interface IUserService
    {
        public Task<bool> CreateNewUser(InitialUser userData);

    }
}
