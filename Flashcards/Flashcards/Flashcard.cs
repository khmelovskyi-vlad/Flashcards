using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    struct Flashcard
    {
        public Flashcard(string Topic, string FrontOrTranslation, string Transcription, string BackOrOriginal)
        {
            this.Topic = Topic;
            this.FrontOrForeignTranslation = FrontOrTranslation;
            this.Transcription = Transcription;
            this.BackOrOriginalWord = BackOrOriginal;
        }
        public string Topic { get; set; }
        public string FrontOrForeignTranslation { get; set; }
        public string Transcription { get; set; }
        public string BackOrOriginalWord { get; set; }
    }
}
