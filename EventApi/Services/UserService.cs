using DAL.Interfaces;
using DAL.Models;
using EventApi.DTO;
using EventApi.Interfaces;

namespace EventApi.Services
{
    public class UserService: IUserService
    {
        IUnitOfWork _unitOfWork;
        // тут должен быть еще логгер , но пока не нужно


        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            
        }

        //this is for testing purposes
        public async Task<bool> CreateNewUser(InitialUser userData)
        {

            var userRepo = _unitOfWork.Repository<User>();
            var userRoleRepo = _unitOfWork.Repository<Usersrole>();

            var userRole = await userRoleRepo.GetByConditionAsync(x => x.Rolename == "Customer");
            if (userRole == null)
            {
                using var _ = userRoleRepo.AddAsync(new Usersrole()
                {
                    Rolename = "Customer"
                });
                userRole = await userRoleRepo.GetByConditionAsync(x => x.Rolename == "Customer");
            }

            try
            {
                return await userRepo.GetByConditionAsync(x => x.Email == userData.Email || x.Username == userData.Username).ContinueWith(task =>
                {
                    if (task.Result != null)
                    {
                        return false;
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            Email = userData.Email,
                            Firstname = userData.Firstname,
                            Lastname = userData.Lastname,
                            Passwordhash = userData.Passwordhash,
                            Username = userData.Username,
                            Roleid = userRole.Roleid
                        };
                        userRepo.AddAsync(newUser);
                        _unitOfWork.SaveChangesAsync();
                        return true;
                    }
                });
            }
            catch
            {
                Console.WriteLine("something go wrong");
                return false;
            }

        }

        public async Task<bool> SetManagerStatusForUser(string userEmail)
        {
            var userRepo = _unitOfWork.Repository<User>();
            var userRoleRepo = _unitOfWork.Repository<Usersrole>();

            var userRole = await userRoleRepo.GetByConditionAsync(x => x.Rolename == "Manager");
            if (userRole == null)
            {
                using var _ = userRoleRepo.AddAsync(new Usersrole()
                {
                    Rolename = "Manager"
                });
                userRole = await userRoleRepo.GetByConditionAsync(x => x.Rolename == "Manager");
            }

            try
            {
                var dbUser = await userRepo.GetByConditionAsync(x => string.Equals(userEmail, x.Email));
                dbUser.Roleid = userRole.Roleid;
                await userRepo.UpdateAsync(dbUser);
                return true;
            }
            catch
            {
                Console.WriteLine("something go wrong");
                return false;
            }
        }
    }
}
