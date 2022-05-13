using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TDL.Domain;

namespace ToDoList.Controllers
{
    public class ToDoesController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext() ;

        // GET: ToDoes
        public ActionResult Index()
        {
            /* string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId); */
            return View( /* db.ToDos.ToList().Where(x=>x.User==currentUser)*/);
        }

        private IEnumerable<ToDo> GetToDos()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);
            return db.ToDos.ToList().Where(x => x.User == currentUser);
        }


        public ActionResult BuildToDoTable()
        {
            return View("_ToDoTable", GetToDos());
        }

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDos.Find(id);
            if (toDo == null) return HttpNotFound();
            return View(toDo);
        }

        // GET: ToDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Discription,IsDone")]
            ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.Identity.GetUserId();
                var currentUser = db.Users.FirstOrDefault
                    (X => X.Id == currentUserId);
                toDo.User = currentUser;

                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDos.Find(id);

            if (toDo == null) return HttpNotFound();


            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.FirstOrDefault
                (x => x.Id == currentUserId);

            if (toDo.User != currentUser) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            return View(toDo);
        }

        // POST: ToDoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Discription,IsDone")]
            ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }


        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            toDo.IsDone = value;
            db.Entry(toDo).State = EntityState.Modified;
            db.SaveChanges();
            return PartialView("_ToDoTable", GetToDos());
        }

        // GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var toDo = db.ToDos.Find(id);
            if (toDo == null) return HttpNotFound();
            return View(toDo);
        }

        // POST: ToDoes/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}