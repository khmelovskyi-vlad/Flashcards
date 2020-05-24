using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Input
    {
        public Input(FileMaster fileMaster, IUserInteractor userInteractor, TopicMet topicMet)
        {
            this.fileMaster = fileMaster;
            this.userInteractor = userInteractor;
            this.topicMet = topicMet;
        }
        private FileMaster fileMaster;
        private IUserInteractor userInteractor;
        TopicMet topicMet;

        private const string PathAllTopics = "D:\\temp\\Flashcards";

        public async Task Run()
        {
            topicMet.WriteTopics();
            await FindData();
        }
        private async Task FindData()
        {
            if (await topicMet.FindOrCreateTopic())
            {
                var key = userInteractor.QuestionAnswerKey("If you want to watch your cards, press 'Enter', if not - press else");
                if (key == UserAction.Enter)
                {
                    var flashcards = fileMaster.ReadData(topicMet.Topic);
                    if (flashcards == null)
                    {
                        userInteractor.WriteLine("Don't have cards");
                    }
                    else
                    {
                        foreach (var flashcard in flashcards)
                        {
                            userInteractor.WriteLine($"{flashcard.Topic.Name} - " +
                                $"{flashcard.FrontOrForeignTranslation} - " +
                                $"{flashcard.Transcription} - " +
                                $"{flashcard.BackOrOriginalWord}");
                        }
                    }
                }
            }
            await EnterData();
        }
        private async Task EnterData()
        {
            var newCards = new List<Flashcard>();
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey($"If you want enter flashcards in {topicMet.Topic.Name}, press 'Enter',\n\r" +
                    $"If not - press else");
                if (key == UserAction.Enter)
                {
                    var front = userInteractor.QuestionAnswerInUkr("Write front or foreign translation");
                    var transcription = userInteractor.QuestionAnswerInUkr("Write transcription");
                    var back = userInteractor.QuestionAnswerInUkr("Write back or original word");
                    newCards.Add( new Flashcard(topicMet.Topic.Id, front, transcription, back));
                    userInteractor.WriteLine("Your card has been added!");
                }
                else
                {
                    var key2 = userInteractor.QuestionAnswerKey($"If you want to save flashcards in {topicMet.Topic.Name}, press 'Enter',\n\r" +
                        $"If not - press else");
                    if (key2 == UserAction.Enter)
                    {
                        await fileMaster.WriteData(newCards);
                    }
                    return;
                }
            }
        }
    }
}
