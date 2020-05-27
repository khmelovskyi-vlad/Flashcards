using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class Deletion
    {
        public Deletion(FileMaster fileMaster, IUserInteractor userInteractor, TopicMet topicMet)
        {
            this.fileMaster = fileMaster;
            this.userInteractor = userInteractor;
            this.topicMet = topicMet;
        }
        private FileMaster fileMaster;
        private IUserInteractor userInteractor;
        TopicMet topicMet;
        enum TypesOfDelete
        {
            topics,
            cards
        }

        public async Task Run()
        {
            topicMet.WriteTopics();
            await SelectMode();
        }
        //private async Task DeleteData()
        //{
        //    await topicMet.FindTopic();
        //    await SelectMode();
        //}
        private async Task SelectMode()
        {
            var key = userInteractor.QuestionAnswerKey("If you want to delete some topics, press 't',\n\r" +
                "If you want to delete flashcards, press 'c'\n\r" +
                "If you want to escape, press 'Esc'");
            switch (key)
            {
                case UserAction.T:
                    await DeleteTopics();
                    break;
                case UserAction.F:
                    await DeleteCards();
                    break;
                case UserAction.Escape:
                    break;
                default:
                    await SelectMode();
                    break;
            }
        }
        private async Task DeleteTopics()
        {
            var topics = await FindTopics();
            if (topics.Count != 0)
            {

            }
        }
        private async Task<List<TopicWithFlashcards>> FindTopics()
        {
            var topicWithFlashcards = new List<TopicWithFlashcards>();
            while (true)
            {
                if (await topicMet.FindTopic())
                {
                    topicWithFlashcards.Add(topicMet.Topic);
                    userInteractor.WriteLine("Ok");
                }
                else
                {
                    userInteractor.WriteLine("Don't have this topic");
                }
                var key = userInteractor.QuestionAnswerKey("If you want to find more topics to delete, press 'Enter'\n\r" +
                    "If you don't want to find, press else");
                if (key != UserAction.Enter)
                {
                    return topicWithFlashcards;
                }
            }
        }
        private async Task DeleteCards()
        {
            var topics = await FindTopics();
            if (topics.Count != 0)
            {
                ShowAllCards(topics);
            }
        }
        private void FindCards()
        {

        }
        private List<Flashcard> TakeAllCards(List<TopicWithFlashcards> topics)
        {
            return topics.SelectMany(x => x.Flashcards).ToList();
        }
        private void ShowAllCards(List<TopicWithFlashcards> topics)
        {
            foreach (var topic in topics)
            {
                foreach (var flashcard in topic.Flashcards)
                {
                    userInteractor.WriteLine($"{flashcard.Topic.Name} - " +
                        $"{flashcard.FrontOrForeignTranslation} - " +
                        $"{flashcard.Transcription} - " +
                        $"{flashcard.BackOrOriginalWord}");
                }
            }
        }
    }
}
