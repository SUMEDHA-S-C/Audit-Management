using AuditManagementPortalClientMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditManagementPortalClientMVC.Repository
{
    public interface IUserRepo
    {
        public bool CheckUser(User user);
        public List<User> GetUsers();
    }
}
