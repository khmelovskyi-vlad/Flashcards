using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class UserContext : DbContext
    {
        public UserContext() : base("DbConnectionStr")
        {

        }
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<TopicWithFlashcards> TopicWithFlashcards { get; set; }
    }
}
