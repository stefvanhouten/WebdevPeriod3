using System.Collections.Generic;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;

namespace WebdevPeriod3.Interfaces
{
    public interface IUserRespoitory
    {
        ValueTask<User> GetById(int id);
        Task AddUser(User entity);
        Task UpdateUser(User entity, int id);
        Task RemoveUser(int id);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
