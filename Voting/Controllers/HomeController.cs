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
            var candidateRepo = await _candidateRepo.Get();
            var cvd = new CandidatesVoteDetails()
            {
                CandidateList = (await _candidateRepo.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.CandidateId), Text = x.CandidateName }).ToList(),
                TotalVoterList = (await _voterRepository.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList(),
                CandidatesVoters = (await _candidateVoterRepository.Get()).Select(x => new CandidatesVoter() { VoterId = x.VoterId, CandidateId = x.CandidateId, Voted = x.Voted }).ToList(),
                NotVotedVoterList = (await _voterRepository.Get()).Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList()
                                    .Where(p => !((_candidateVoterRepository.Get()).Result).Any(p2 => p2.VoterId.Equals(Convert.ToInt32(p.Value))))
                                    .Select(x => new SelectListItem { Value = Convert.ToString(x.Value), Text = x.Text }).ToList()
            };
            return View(cvd);
        }
        public ActionResult AddVoter()
        {
            var voter = new Voter();
            return View("AddVoter", voter);
        }
        [HttpPost]
        public async Task<ActionResult> SaveVoter(Voter voter)
        {
            var voterCount = (await _voterRepository.Get()).Where(x => x.VoterName == voter.VoterName).Count();
            if (!ModelState.IsValid || voterCount > 0)
            {
                TempData["voter"] = "Error";
                return RedirectToAction("AddVoter", "Home");
            }

            if (!string.IsNullOrEmpty(voter.VoterName))
                await _voterRepository.Create(voter);

            TempData["voter"] = "SaveVoter";
            return RedirectToAction("AddVoter", "Home");
        }
        public ActionResult AddCandidate()
        {
            var candidate = new Candidate();
            return View("AddCandidate", candidate);
        }
        [HttpPost]
        public async Task<ActionResult> SaveCandidate(Candidate candidate)
        {
            var candidateCount = (await _candidateRepo.Get()).Where(x=>x.CandidateName == candidate.CandidateName).Count();

            if (!ModelState.IsValid || candidateCount > 0)
            {
                TempData["candidate"] = "Error";
                return RedirectToAction("AddCandidate", "Home");
            }

            if (!string.IsNullOrEmpty(candidate.CandidateName))
               await _candidateRepo.Create(candidate);
            TempData["candidate"] = "SaveCandidate";
            return RedirectToAction("AddCandidate", "Home");
        }
        [HttpPost]
        public async Task<ActionResult> SaveVote(CandidatesVoter candidatesVoteDetails)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
