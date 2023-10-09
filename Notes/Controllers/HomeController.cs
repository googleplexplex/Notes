using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Notes.Models;

namespace Notes.Controllers
{
    public class HomeController : Controller
    {
        // создаем контекст данных
        NoteContext db = new NoteContext();

        public ActionResult Index()
        {
            IEnumerable<Note> notes = db.Notes;
            IEnumerable<Category> categories = db.Categories;

            ViewBag.Notes = notes;
            ViewBag.Categories = categories.ToList();
            ViewBag.MaxTextLen = 140;

            return View();
        }


        [HttpGet]
        public ActionResult Open(int? id)
        {
            if (id == null)
            {
                Note newNote = new Note();
                newNote.id = db.Notes.FirstOrDefault(p => p.id == db.Notes.Max(x => x.id)).id + 1;
                newNote.categoryId = 1;
                newNote.lastEdited = DateTime.Now;
                newNote.text = "";
                newNote.title = "";
                db.Notes.Add(newNote);

                ViewBag.Note = newNote;
                ViewBag.Categories = db.Categories.ToList();
            }
            else
            {
                ViewBag.Note = db.Notes.FirstOrDefault(d => d.id == id);
                ViewBag.Categories = db.Categories.ToList();
            }
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public ActionResult Open(Note note)
        {
            Note editedNote = db.Notes.FirstOrDefault(d => d.id == note.id);
            editedNote.id = note.id;
            editedNote.lastEdited = DateTime.Now;
            editedNote.text = note.text ?? "";
            editedNote.title = note.title ?? "";
            editedNote.categoryId = note.categoryId;

            db.SaveChanges();
            return Redirect("/Home/Index");
        }


        [HttpGet]
        public ActionResult ChangeCategories()
        {
            ViewBag.Categories = db.Categories;
            ViewBag.emptyCategory = db.Categories.FirstOrDefault(f => f.id == 1);
            return View();
        }


        [NonAction]
        private void UpdateCategoriesNames(List<Category> newCategoriesData)
        {
            var presentDbCategoriesNames = db.Categories.Select(f => f.name).ToList();
            for (int i = 0; i < presentDbCategoriesNames.Count; i++)
            {
                if (presentDbCategoriesNames[i] != newCategoriesData[i].name)
                {
                    var newCategoryId = newCategoriesData[i].id;
                    Category changedCategory = db.Categories.FirstOrDefault(f => f.id == newCategoryId);
                    changedCategory.name = newCategoriesData[i].name;
                }
            }
            db.SaveChanges();
        }

        [HttpPost, ActionName("Save")]
        public ActionResult ChangeCategories(List<Category> newCategoriesData, string newCategoryName)
        {
            UpdateCategoriesNames(newCategoriesData);

            if (newCategoryName != "")
            {
                Category newCategory = new Category();
                newCategory.id = db.Categories.FirstOrDefault(p => p.id == db.Categories.Max(x => x.id)).id + 1;
                newCategory.name = newCategoryName;
                db.Categories.Add(newCategory);
            }
            db.SaveChanges();

            return Redirect("/Home/Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCategoryAndExit(List<Category> newCategoriesData, string deletedCategoryId, string newCategoryName)
        {
            UpdateCategoriesNames(newCategoriesData);

            var id = newCategoriesData[int.Parse(deletedCategoryId)].id;
            var removedCategory = db.Categories.FirstOrDefault(f => f.id == id);
            db.Categories.Remove(removedCategory);

            foreach (var b in db.Notes)
            {
                if (b.categoryId == id)
                {
                    b.categoryId = 1;
                }
            }
            db.SaveChanges();

            return Redirect("/Home/Index");
        }



        [HttpGet]
        public ActionResult ShowByCategory(int id = 1)
        {
            IEnumerable<Note> notes = db.Notes.Where(f => f.categoryId == id);
            IEnumerable<Category> categories = db.Categories;
            ViewBag.Notes = notes.ToList();
            ViewBag.Categories = categories.ToList();
            ViewBag.MaxTextLen = 140;

            return View();
        }
    }
}