using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Voting.Domain;
using Voting.Infrastructure;
using Voting.Models;
using Voting.ViewModels;

namespace Voting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VotingContext _context;

        public HomeController(ILogger<HomeController> logger, VotingContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var cvd = new CandidatesVoteDetails()
            {
                CandidateList = _context.Candidates.Select(x => new SelectListItem { Value = Convert.ToString(x.CandidateId), Text = x.CandidateName }).ToList(),
                TotalVoterList = _context.Voters.Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList(),
                CandidatesVoters = _context.CandidatesVoters.Select(x => new CandidatesVoter() { VoterId = x.VoterId, CandidateId = x.CandidateId, Voted = x.Voted }).ToList(),
                NotVotedVoterList = _context.Voters.Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList()
                                     .Where(p => !(_context.CandidatesVoters).Any(p2 => p2.VoterId.Equals(Convert.ToInt32(p.Value))))
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
        public ActionResult SaveVoter(Voter voter)
        {
            var voterCount = _context.Voters.Where(x => x.VoterName == voter.VoterName).Count();
            if (!ModelState.IsValid || voterCount > 0)
            {
                TempData["voter"] = "Error";
                return RedirectToAction("AddVoter", "Home");
            }

            if (!string.IsNullOrEmpty(voter.VoterName))
                _context.Voters.Add(voter);

            _context.SaveChanges();
            TempData["voter"] = "SaveVoter";
            return RedirectToAction("AddVoter", "Home");
        }
        public ActionResult AddCandidate()
        {
            var candidate = new Candidate();
            return View("AddCandidate", candidate);
        }
        [HttpPost]
        public ActionResult SaveCandidate(Candidate candidate)
        {
            var candidateCount = _context.Candidates.Where(x=>x.CandidateName == candidate.CandidateName).Count();

            if (!ModelState.IsValid || candidateCount > 0)
            {
                TempData["candidate"] = "Error";
                return RedirectToAction("AddCandidate", "Home");
            }

            if (!string.IsNullOrEmpty(candidate.CandidateName))
                _context.Candidates.Add(candidate);

            _context.SaveChanges();
            TempData["candidate"] = "SaveCandidate";
            return RedirectToAction("AddCandidate", "Home");
        }
        [HttpPost]
        public ActionResult SaveVote(CandidatesVoter candidatesVoteDetails)
        {
            var can = new CandidatesVoter()
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
                _context.CandidatesVoters.Add(can);

            _context.SaveChanges();
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
