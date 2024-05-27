using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voting.Domain;

namespace Voting.Infrastructure.Repository
{
    public class CandidateRepository : ICandidateRepository
    {
        private VotingContext _context;

        public CandidateRepository(VotingContext context)
        {
            _context = context;
        }

        public async Task Create(Candidate model)
        {
            await _context.Candidates.AddAsync(model);
            _context.SaveChanges();
        }

        public Task Delete(Candidate model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Candidate>> Get()
        {
            return await _context.Candidates.ToListAsync();
        }

        public Task<Candidate> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Candidate model)
        {
            throw new NotImplementedException();
        }
    }
}
