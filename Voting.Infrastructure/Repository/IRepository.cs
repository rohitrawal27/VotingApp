using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Domain;

namespace Voting.Infrastructure.Repository
{
    public interface IRepository<T>
    {
        public Task<List<T>> Get();
        public Task<T> Get(int id);
        public Task Create(T model);
        public Task Update(T model);
        public Task Delete(T model);
    }
}
