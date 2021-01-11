using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserDashboard.Models;

namespace UserDashboard.Repository
{
    public interface IDataRepository
    {
        Task<List<User>> GetMultipleUsers(int amount);
        Task<User> GetSingleUser();
    }
}
