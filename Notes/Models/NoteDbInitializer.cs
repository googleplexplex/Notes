using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Notes.Models
{
    public class NoteDbInitializer : DropCreateDatabaseAlways<NoteContext>
    {
        protected override void Seed(NoteContext db)
        {
            for (int i = 1; i < 10+1; i++)
            {
                db.Notes.Add(new Note { id = i, title = "Заметка" + i, text = "Текст заметки " + i, lastEdited = DateTime.Now, categoryId = (i % 3) + 1 });
            }

            db.Categories.Add(new Category { id = 0, name = "Без категории" });
            for (int i = 2; i < 3+2; i++)
            {
                db.Categories.Add(new Category { id = i, name = "Категория "+i  });
            }

            base.Seed(db);
        }
    }
}