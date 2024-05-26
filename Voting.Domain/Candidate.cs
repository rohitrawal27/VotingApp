using System.ComponentModel.DataAnnotations;

namespace Voting.Domain;

public partial class Candidate
{
    public int CandidateId { get; set; }
    public string CandidateName { get; set; } = null!;
    public virtual ICollection<CandidatesVoter> CandidatesVoters { get; set; } = new List<CandidatesVoter>();
}
