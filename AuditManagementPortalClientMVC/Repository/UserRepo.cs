using AuditManagementPortalClientMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditManagementPortalClientMVC.Repository
{
    public class UserRepo : IUserRepo
    {
        public List<User> GetUsers()
        {
            List<User> users = new List<User>()
            {
            new User{Name = "Akanksha", Password ="Akanksha123" },
            new User{Name = "Bhavana", Password ="Bhavana123" },
            new User{Name = "Durga", Password ="Durga123" },
            new User{Name = "Medhini", Password ="Medhini123" },
            new User{Name = "Niya", Password ="Niya123" },
            new User{Name = "Sumedha", Password ="Sumedha123" },
            new User{Name = "Venkat", Password ="Venkat123" }
            };
            return users;
        }
        public bool CheckUser(User user)
        {
            List<User> users = GetUsers();
            foreach (User usr in users)
            {
                if (user.Name == usr.Name && user.Password == usr.Password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
