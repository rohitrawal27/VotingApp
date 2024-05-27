using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Domain;

namespace Voting.Infrastructure.Repository
{
    public class CandidateVoterRepository : ICandidateVoterRepository
    {
        private readonly VotingContext _context;

        public CandidateVoterRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task Create(CandidatesVoter model)
        {
            await _context.CandidatesVoters.AddAsync(model);
            _context.SaveChanges();
        }

        public Task Delete(CandidatesVoter model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CandidatesVoter>> Get()
        {
            return await _context.CandidatesVoters.ToListAsync();
        }

        public Task<CandidatesVoter> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(CandidatesVoter model)
        {
            throw new NotImplementedException();
        }
    }
}
