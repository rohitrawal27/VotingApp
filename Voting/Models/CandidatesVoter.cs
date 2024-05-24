using System;
using System.Collections.Generic;

namespace Voting.Models;

public partial class CandidatesVoter
{
    public int CandidatesvotersId { get; set; }

    public int? VoterId { get; set; }

    public int? CandidateId { get; set; }

    public bool? Voted { get; set; }

    public virtual Candidate? Candidate { get; set; }
    public virtual Voter? Voter { get; set; }
}
