namespace Voting.Domain;

public partial class Voter
{
    public int VoterId { get; set; }

    public string VoterName { get; set; } = null!;

    public virtual ICollection<CandidatesVoter> CandidatesVoters { get; set; } = new List<CandidatesVoter>();
}
