using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Domain;

namespace Voting.Infrastructure.Repository
{
    public class VoterRepository : IVoterRepository
    {
        private VotingContext _context;

        public VoterRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task Create(Voter model)
        {
            await _context.Voters.AddAsync(model);
            _context.SaveChanges();
        }

        public Task Delete(Voter model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Voter>> Get()
        {
            return await _context.Voters.ToListAsync();
        }

        public Task<Voter> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Voter model)
        {
            throw new NotImplementedException();
        }
    }
}
