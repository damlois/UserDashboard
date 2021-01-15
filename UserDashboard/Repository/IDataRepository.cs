using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDashboard.Models;

namespace UserDashboard.Repository
{
    public interface IDataRepository
    {
        Task<List<User>> GetMultipleUsers(int amount, string gender, int pageNumber);
        Task<User> GetSingleUser();
        User FindUser(string email);
    }
}
