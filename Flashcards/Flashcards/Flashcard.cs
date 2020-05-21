using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class Flashcard
    {
        public Flashcard(TopicWithFlashcards Topic, string FrontOrTranslation, string Transcription, string BackOrOriginal)
        {
            this.Topic = Topic;
            this.FrontOrForeignTranslation = FrontOrTranslation;
            this.Transcription = Transcription;
            this.BackOrOriginalWord = BackOrOriginal;
        }
        public int Id { get; set; }
        public string FrontOrForeignTranslation { get; set; }
        public string Transcription { get; set; }
        public string BackOrOriginalWord { get; set; }
        public virtual int TopicID { get; set; }
        public virtual TopicWithFlashcards Topic { get; set; }
    }
}
