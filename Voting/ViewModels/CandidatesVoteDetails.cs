using Microsoft.AspNetCore.Mvc.Rendering;
using Voting.Domain;

namespace Voting.ViewModels
{
    public class CandidatesVoteDetails
    {
        public int VoterId { get; set; } = 0;
        public int CandidateId { get; set; } = 0;
        public List<SelectListItem> CandidateList { get; set; } =new List<SelectListItem>();
        public List<SelectListItem> TotalVoterList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> NotVotedVoterList { get; set; } = new List<SelectListItem>();
        public List<CandidatesVoter> CandidatesVoters { get; set; } = new List<CandidatesVoter>();
    }
}
