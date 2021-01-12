using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebdevPeriod3.Entities;
using WebdevPeriod3.Interfaces;

namespace WebdevPeriod3.Services
{
    public class UserRepository : BaseRepository, IUserRespoitory
    {
        private readonly IUserCommandText _commandText;

        public UserRepository(IConfiguration configuration, IUserCommandText commandText) : base(configuration)
        {
            _commandText = commandText;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryAsync<User>(_commandText.GetAllUsers);
                return query;
            });
        }

        public async ValueTask<User> GetById(int id)
        {
            return await WithConnection(async conn =>
            {
                var query = await conn.QueryFirstOrDefaultAsync<User>(_commandText.GetUserById, new { Id = id });
                return query;
            });
        }

        public async Task AddUser(User entity)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.AddUser,
                    new { Username = entity.Username, Password = entity.Password });
            });
        }

        public async Task UpdateUser(User entity, int id)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.UpdateUser,
                    new { Username = entity.Username, Password = entity.Password, Id = id });
            });
        }
        public async Task RemoveUser(int id)
        {
            await WithConnection(async conn =>
            {
                await conn.ExecuteAsync(_commandText.RemoveUser, new { Id = id });
            });
        }
    }
}
