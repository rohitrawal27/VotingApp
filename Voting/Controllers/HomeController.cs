using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Voting.Domain;
using Voting.Infrastructure;
using Voting.Infrastructure.Repository;
using Voting.Models;
using Voting.ViewModels;

namespace Voting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICandidateRepository _candidateRepo;
        private readonly IVoterRepository _voterRepository;
        private readonly ICandidateVoterRepository _candidateVoterRepository;

        public HomeController(ILogger<HomeController> logger, ICandidateRepository candidateRepo,IVoterRepository voterRepository, ICandidateVoterRepository candidateVoterRepository)
        {
            _logger = logger;
            _candidateRepo = candidateRepo;
            _voterRepository = voterRepository;
            _candidateVoterRepository = candidateVoterRepository;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetCandidateVoteInfo());
        }
        #region Save Voter
        public ActionResult AddVoter()
        {
            var voter = new Voter();
            return View("AddVoter", voter);
        }
        [HttpPost]
        public async Task<ActionResult> SaveVoter(Voter voter)
        {
            try
            {
                var voterCount = await GetVoterCount(voter);
                if (!ModelState.IsValid || voterCount > 0)
                {
                    TempData["SaveVoter"] = false;
                    return RedirectToAction("AddVoter", "Home");
                }
                else
                {
                    TempData["SaveVoter"] = true;
                    await _voterRepository.Create(voter);
                }
                return RedirectToAction("AddVoter", "Home");
            }
            catch (Exception ex)
            {
               return RedirectToAction("Error");
            }
        }
        #endregion
        #region Save Candidate
        public ActionResult AddCandidate()
        {
            var candidate = new Candidate();
            return View("AddCandidate", candidate);
        }
        [HttpPost]
        public async Task<ActionResult> SaveCandidate(Candidate candidate)
        {
            try
            {
                var candidateCount = await GetCandidateCount(candidate);

                if (!ModelState.IsValid || candidateCount > 0)
                {
                    TempData["SaveCandidate"] = false;
                    return RedirectToAction("AddCandidate", "Home");
                }
                else
                {
                    TempData["SaveCandidate"] = true;
                    await _candidateRepo.Create(candidate);
                }
                return RedirectToAction("AddCandidate", "Home");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
        }
        #endregion
        #region Save Vote
        [HttpPost]
        public async Task<ActionResult> SaveVote(CandidatesVoter candidatesVoteDetails)
        {
            try
            {
                var candidatesVoter = new CandidatesVoter()
                {
                    CandidateId = candidatesVoteDetails.CandidateId,
                    VoterId = candidatesVoteDetails.VoterId,
                    Voted = true
                };

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (!string.IsNullOrEmpty(Convert.ToString(candidatesVoteDetails.VoterId)) && !string.IsNullOrEmpty(Convert.ToString(candidatesVoteDetails.CandidateId)))
                    await _candidateVoterRepository.Create(candidatesVoter);

                TempData["voted"] = "VoteSubmitted";
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }
        }
        #endregion
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region local methods
        private async Task<CandidatesVoteDetails> GetCandidateVoteInfo()
        {
            return new CandidatesVoteDetails()
            {
                CandidateList = (await _candidateRepo.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.CandidateId), Text = x.CandidateName }).ToList(),
                TotalVoterList = (await _voterRepository.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList(),
                CandidatesVoters = (await _candidateVoterRepository.Get()).Select(x => new CandidatesVoter() { VoterId = x.VoterId, CandidateId = x.CandidateId, Voted = x.Voted }).ToList(),
                NotVotedVoterList = (await _voterRepository.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList()
                                                .Where(p => !((_candidateVoterRepository.Get()).Result).Any(p2 => p2.VoterId.Equals(Convert.ToInt32(p.Value))))
                                                .Select(x => new SelectListItem { Value = Convert.ToString(x.Value), Text = x.Text }).ToList()
            };
        }
        private async Task<int> GetVoterCount(Voter voter)
        {
            return (await _voterRepository.Get()).Where(x => x.VoterName == voter.VoterName).Count();
        }
        private async Task<int> GetCandidateCount(Candidate candidate)
        {
            return (await _candidateRepo.Get()).Where(x => x.CandidateName == candidate.CandidateName).Count();
        }
        #endregion
    }
}
