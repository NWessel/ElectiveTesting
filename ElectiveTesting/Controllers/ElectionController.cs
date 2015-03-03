using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ElectiveTesting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ElectiveTesting.ViewModels;
using System.Data.Entity.Infrastructure;

namespace ElectiveTesting.Controllers
{
    [Authorize]
    public class ElectionController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public ElectionController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Elections
        public ActionResult Index(int? id, int? electiveId)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());

            var viewModel = new ElectionIndexData();

            viewModel.Elections = db.Elections.Where(e => e.HostId == currentUser.Id)
                .Include(e => e.Electives)
                .Include(e => e.ApplicationUsers)
                .OrderBy(e => e.Name);

            if (id != null)
            {
                ViewBag.ElectionId = id.Value;
                viewModel.Electives = viewModel.Elections.Where(
                    i => i.Id == id.Value).Single().Electives;
            }

            //if (electiveId != null)
            //{
            //    ViewBag.ElectiveId = electiveId.Value;
            //    // Lazy loading
            //    //viewModel.Enrollments = viewModel.Courses.Where(
            //    //    x => x.CourseID == courseID).Single().Enrollments;
            //    // Explicit loading
            //    var selectedElective = viewModel.Electives.Where(x => x.Id == electiveId).Single();
            //    db.Entry(selectedElective).Collection(x => x.Enrollments).Load();
            //    foreach (Enrollment enrollment in selectedCourse.Enrollments)
            //    {
            //        db.Entry(enrollment).Reference(x => x.Student).Load();
            //    }

            //    viewModel.Enrollments = selectedCourse.Enrollments;
            //}

            //viewModel.InvitedToElections = db.Elections.Where(e => e.ApplicationUsers == currentUser)
            //    .Include(e => e.Electives);

            //viewModel.InvitedToElections = db.Elections
            //    .Include(e => e.ApplicationUsers.Where(u => u.Id == currentUser.Id));

            //viewModel.InvitedToElections = db.Elections.Where(e => e.ApplicationUsers == currentUser)
            //    .Include(e => e.Electives)
            //    .Include(e => e.ApplicationUsers)
            //    .OrderBy(e => e.Name);

            viewModel.InvitedToElections = db.Elections.Where(e => e.ApplicationUsers.Any(au => au.Id == currentUser.Id))
                .Include(e => e.Electives);

            return View(viewModel);
        }

        // GET: Election/Vote
        public ActionResult Vote()
        {
            return View();
        }

        // GET: Election/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election election = db.Elections.Find(id);
            if (election == null)
            {
                return HttpNotFound();
            }
            return View(election);
        }

        // GET: Election/Create
        public ActionResult Create()
        {
            var vm = new ElectionCreateData();
            vm.availableUsers = db.Users.Select(u => u.Email);
            var election = new Election();
            election.Electives = new List<Elective>();
            PopulateAssignedElectiveData(election);
            return View(vm);
        }

        // POST: Election/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Election election, string[] selectedElectives, string[] invitedUsers)
        {
            if (selectedElectives != null)
            {
                election.Electives = new List<Elective>();
                foreach (var elective in selectedElectives)
                {
                    var electiveToAdd = db.Electives.Find(int.Parse(elective));
                    election.Electives.Add(electiveToAdd);
                }
            }

            if(invitedUsers != null)
            {
                election.ApplicationUsers = new List<ApplicationUser>();
                foreach (var mail in invitedUsers)
                {
                    var userToAdd = manager.FindByEmail(mail);
                    election.ApplicationUsers.Add(userToAdd);
                }
            }

            if (ModelState.IsValid)
            {
                election.HostId = User.Identity.GetUserId();
                db.Elections.Add(election);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateAssignedElectiveData(election);
            return View(election);
        }

        // GET: Election/Edit/5
        public ActionResult Edit(int? id)
        {
            var vm = new ElectionCreateData();
            
            Election election = db.Elections
                .Include(e => e.Electives)
                .Include(e => e.ApplicationUsers)
                .Where(e => e.Id == id)
                .Single();
            PopulateAssignedElectiveData(election);

            vm.election = election;
            vm.invitedUsers = election.ApplicationUsers.Select(e => e.Email);
            vm.availableUsers = db.Users.Select(u => u.Email);

            var invited = new HashSet<string>(vm.invitedUsers);
            var available = new HashSet<string>(vm.availableUsers);

            foreach (var allUsers in db.Users)
            {
                if(invited.Contains(allUsers.Email))
                {
                    available.Remove(allUsers.Email);
                }

            }
            vm.availableUsers = available;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (election == null)
            {
                return HttpNotFound();
            }
            
            return View(vm);
        }

        private void PopulateAssignedElectiveData(Election election)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var allElectives = db.Electives.Where(e => e.HostId == currentUser.Id);
            var electionElectives = new HashSet<int>(election.Electives.Select(e => e.Id));
            var viewModel = new List<AssignedElectiveData>();
            foreach (var elective in allElectives)
            {
                viewModel.Add(new AssignedElectiveData
                {
                    ElectiveId = elective.Id,
                    Name = elective.Name,
                    Assigned = electionElectives.Contains(elective.Id)
                });
            }
            ViewBag.Electives = viewModel;
        }

        // POST: Election/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, string[] selectedElectives, string[] invitedUsers)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var electionToUpdate = db.Elections
               .Include(i => i.Electives)
               .Include(i => i.ApplicationUsers)
               .Where(i => i.Id == id)
               .Single();

            if (TryUpdateModel(electionToUpdate, "",
               new string[] { "Id", "Name" }))
            {
                try
                {
                    UpdateElectionElectives(invitedUsers, selectedElectives, electionToUpdate);

                    db.Entry(electionToUpdate).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedElectiveData(electionToUpdate);
            return View(electionToUpdate);
        }

        private void UpdateElectionElectives(string [] invitedUsers, string[] selectedElectives, Election electionToUpdate)
        {
            if (selectedElectives == null)
            {
                electionToUpdate.Electives = new List<Elective>();
                return;
            }
            if (invitedUsers == null)
            {
                electionToUpdate.ApplicationUsers = new List<ApplicationUser>();
            }

            var invitedUsersHS = new HashSet<string>(invitedUsers);
            var electionUsers = new HashSet<string>(electionToUpdate.ApplicationUsers.Select(c => c.Email));

            foreach (var user in db.Users)
            {
                if(invitedUsersHS.Contains(user.Email))
                {
                    if(!electionUsers.Contains(user.Email))
                    {
                        electionToUpdate.ApplicationUsers.Add(user);
                    }
                }
                else
                {
                    if(electionUsers.Contains(user.Email))
                    {
                        electionToUpdate.ApplicationUsers.Remove(user);
                    }
                }
            }

            var selectedElectivesHS = new HashSet<string>(selectedElectives);
            var electionElectives = new HashSet<int>(electionToUpdate.Electives.Select(c => c.Id));
            foreach (var elective in db.Electives)
            {
                if (selectedElectivesHS.Contains(elective.Id.ToString()))
                {
                    if (!electionElectives.Contains(elective.Id))
                    {
                        electionToUpdate.Electives.Add(elective);
                    }
                }
                else
                {
                    if (electionElectives.Contains(elective.Id))
                    {
                        electionToUpdate.Electives.Remove(elective);
                    }
                }
            }
        }

        // GET: Election/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Election election = db.Elections.Find(id);
            if (election == null)
            {
                return HttpNotFound();
            }
            return View(election);
        }

        // POST: Election/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Election election = db.Elections.Find(id);
            db.Elections.Remove(election);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
