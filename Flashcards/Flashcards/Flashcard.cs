using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    public class Flashcard
    {
        public Flashcard()
        {

        }
        public Flashcard(int TopicID, string FrontOrTranslation, string Transcription, string BackOrOriginal)
        {
            this.TopicID = TopicID;
            this.FrontOrForeignTranslation = FrontOrTranslation;
            this.Transcription = Transcription;
            this.BackOrOriginalWord = BackOrOriginal;
        }
        public int TopicID { get; set; }
        public int Id { get; set; }
        public string FrontOrForeignTranslation { get; set; }
        public string Transcription { get; set; }
        public string BackOrOriginalWord { get; set; }
        public virtual TopicWithFlashcards Topic { get; set; }
    }
}
