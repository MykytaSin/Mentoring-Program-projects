using DAL.Models;
using EventApi.DTO;
using EventApi.Helpers;
using EventApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventApi.Services
{
    public class UserService: IUserService
    {
        private readonly MyAppContext _context;

        public UserService(MyAppContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateNewUser(InitialUser userData)
        {

            var userRole = await _context.Usersroles.FirstOrDefaultAsync(r => r.Rolename == Constants.CustomerUserRole);

            if (userRole == null)
            {
                await _context.Usersroles.AddAsync(new Usersrole() { Rolename = "Customer" });
                await _context.SaveChangesAsync();

                userRole = await _context.Usersroles.FirstOrDefaultAsync(r => r.Rolename == Constants.CustomerUserRole);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email || u.Username == userData.Username);

            if (user is null)
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
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<CurrentUser> GetCurrentUser()
        {
            return await Task.Run(() => new CurrentUser()
            {
                UserId = 1
            });
        }


        public async Task<bool> SetManagerStatusForUser(string userEmail)
        {

            var userRole = await _context.Usersroles.FirstAsync(r=>r.Rolename == Constants.ManagerUserRole);

            if (userRole == null)
            {
                _context.Usersroles.Add(new Usersrole() { Rolename = Constants.ManagerUserRole });
                await _context.SaveChangesAsync();
                userRole = await _context.Usersroles.FirstAsync(r => r.Rolename == Constants.ManagerUserRole);
            }

            try
            {
                var dbUser = await _context.Users.FirstAsync(u => u.Email == userEmail);
                dbUser.Roleid = userRole.Roleid;
                await _context.SaveChangesAsync();
                return true;
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
