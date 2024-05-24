namespace Voting.ViewModels
{
    public class CandidateVote
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public int VoterId { get; set; }
        public string VoterName { get; set; }
        public bool Voted { get; set; }
    }
}
