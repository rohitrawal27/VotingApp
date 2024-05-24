using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Voting.Models;
using Voting.ViewModels;

namespace Voting.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var context = new VotingContext();
            var cvd = new CandidatesVoteDetails();
            cvd = new CandidatesVoteDetails()
            {
                CandidateList = context.Candidates.Select(x => new SelectListItem { Value = Convert.ToString(x.CandidateId), Text = x.CandidateName }).ToList(),
                VoterList = context.Voters.Select(x => new SelectListItem { Value = Convert.ToString(x.VoterId), Text = x.VoterName }).ToList(),
                CandidatesVoters= context.CandidatesVoters.Select(x => new CandidatesVoter() { VoterId = x.VoterId, CandidateId = x.CandidateId, Voted = x.Voted }).ToList()
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
            var context = new VotingContext();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddVoter", "Home");
            }

            if (!string.IsNullOrEmpty(voter.VoterName))
                context.Voters.Add(voter);

            context.SaveChanges();
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
            var context = new VotingContext();
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddCandidate", "Home");
            }

            if (!string.IsNullOrEmpty(candidate.CandidateName))
                context.Candidates.Add(candidate);

            context.SaveChanges();
            return RedirectToAction("AddCandidate", "Home");
        }
        [HttpPost]
        public ActionResult SaveVote(CandidatesVoter candidatesVoteDetails)
        {
            var context = new VotingContext();
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
                context.CandidatesVoters.Add(can);

            context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
