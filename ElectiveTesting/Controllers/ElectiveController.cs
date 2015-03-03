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

namespace ElectiveTesting.Controllers
{
    [Authorize]
    public class ElectiveController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;

        public ElectiveController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Elective
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var electives = db.Electives.Where(e => e.HostId == currentUser.Id);
            return View(electives.ToList());
        }

        // GET: Elective/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elective elective = db.Electives.Find(id);
            if (elective == null)
            {
                return HttpNotFound();
            }
            return View(elective);
        }

        // GET: Elective/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Elective/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description")] Elective elective)
        {
            if (ModelState.IsValid)
            {
                elective.HostId = User.Identity.GetUserId();
                db.Electives.Add(elective);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HostId = new SelectList(db.Users, "Id", "Email", elective.HostId);
            return View(elective);
        }

        // GET: Elective/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elective elective = db.Electives.Find(id);
            if (elective == null)
            {
                return HttpNotFound();
            }
            ViewBag.HostId = new SelectList(db.Users, "Id", "Email", elective.HostId);
            return View(elective);
        }

        // POST: Elective/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,HostId")] Elective elective)
        {
            if (ModelState.IsValid)
            {
                db.Entry(elective).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HostId = new SelectList(db.Users, "Id", "Email", elective.HostId);
            return View(elective);
        }

        // GET: Elective/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Elective elective = db.Electives.Find(id);
            if (elective == null)
            {
                return HttpNotFound();
            }
            return View(elective);
        }

        // POST: Elective/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Elective elective = db.Electives.Find(id);
            db.Electives.Remove(elective);
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
