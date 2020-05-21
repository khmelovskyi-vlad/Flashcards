using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class TopicWithFlashcards
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Flashcard> Flashcards { get; set; }
    }
}
