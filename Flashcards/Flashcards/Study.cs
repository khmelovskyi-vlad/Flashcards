using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Study
    {
        public Study(FileMaster fileMaster, IUserInteractor userInteractor, TopicMet topicMet)
        {
            this.fileMaster = fileMaster;
            this.userInteractor = userInteractor;
            this.topicMet = topicMet;
        }

        private FileMaster fileMaster;
        IUserInteractor userInteractor;
        TopicMet topicMet;
        private List<Flashcard> flashcards = new List<Flashcard>();

        private const string PathAllTopics = "D:\\temp\\Flashcards";

        public async Task Run()
        {
            topicMet.WriteTopics();
        }
        private UserAction SelectMode()
        {
            var key = userInteractor.QuestionAnswerKey("If you want to study all cards, press 'a',\n\r" +
                "If you want to study some topic cards, press 't'");
            switch (key)
            {
                case UserAction.Enter:
                    break;
                case UserAction.Escape:
                    break;
                case UserAction.A:
                    break;
                case UserAction.L:
                    break;
                case UserAction.Else:
                    break;
                default:
                    return UserAction.Else;
            }
            return UserAction.Else;
        }
        private async Task<List<Flashcard>> TakeAllFlashcards()
        {
            var paths = fileMaster.GetDirectoriesPath(PathAllTopics);
            var flashcards = new List<Flashcard>();
            foreach (var path in paths)
            {
                flashcards.AddRange(await TakeFlashCards($"{path}\\Flashcards.json"));
            }
            return flashcards;
        }
        private async Task<List<Flashcard>> TakeFlashCards(string parh)
        {
            return await fileMaster.ReadData(parh);
        }
    }
}
