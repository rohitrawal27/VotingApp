using System.ComponentModel.DataAnnotations;

namespace Voting.Domain;

public partial class Voter
{
    public int VoterId { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]+$")]
    [Display(Name = "Voter Name")]
    public string VoterName { get; set; } = null!;

    public virtual ICollection<CandidatesVoter> CandidatesVoters { get; set; } = new List<CandidatesVoter>();
}
