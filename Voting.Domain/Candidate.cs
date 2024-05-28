using System.ComponentModel.DataAnnotations;

namespace Voting.Domain;

public partial class Candidate
{
    public int CandidateId { get; set; }
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]+$")]
    [Display(Name = "Candidate Name")]
    public string CandidateName { get; set; } = null!;
    public virtual ICollection<CandidatesVoter> CandidatesVoters { get; set; } = new List<CandidatesVoter>();
}
