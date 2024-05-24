using Microsoft.AspNetCore.Mvc.Rendering;
using Voting.Models;

namespace Voting.ViewModels
{
    public class CandidatesVoteDetails
    {
        public int VoterId { get; set; } = 0;
        public int CandidateId { get; set; } = 0;
        public List<SelectListItem> CandidateList { get; set; }
        public List<SelectListItem> VoterList { get; set; }
        public List<CandidatesVoter> CandidatesVoters { get; set; }
    }
}
