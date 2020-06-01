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
                "If you want to delete flashcards, press 'f'\n\r" +
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
                foreach (var topic in topics)
                {
                    userInteractor.WriteLine(topic.Name);
                }
                var key = userInteractor.QuestionAnswerKey("If you realy want to delete these topics, press Enter\n\r" +
                    "If don't want, press else");
                userInteractor.WriteLine("Okey");
                if (key == UserAction.Enter)
                {
                    await fileMaster.DeleteTopics(topics);
                    userInteractor.WriteLine("Flashcards have been deleted");
                }
            }
        }
        private async Task<List<TopicWithFlashcards>> FindTopics()
        {
            var topicsWithFlashcards = new List<TopicWithFlashcards>();
            while (true)
            {
                if (await topicMet.FindTopic())
                {
                    if (CheckTopic(topicsWithFlashcards))
                    {
                        topicsWithFlashcards.Add(topicMet.Topic);
                    }
                }
                else
                {
                    userInteractor.WriteLine("Don't have this topic");
                }
                var key = userInteractor.QuestionAnswerKey("If you want to find more topics to delete, press 'Enter'\n\r" +
                    "If you don't want to find, press else");
                if (key != UserAction.Enter)
                {
                    return topicsWithFlashcards;
                }
            }
        }
        private bool CheckTopic(List<TopicWithFlashcards> topicsWithFlashcards)
        {
            foreach (var topicWithFlashcards in topicsWithFlashcards)
            {
                if (topicWithFlashcards.Id == topicMet.Topic.Id)
                {
                    userInteractor.WriteLine("You added this topic earlier");
                    return false;
                }
            }
            userInteractor.WriteLine("Okey");
            return true;
        }
        private async Task DeleteCards()
        {
            var topics = await FindTopics();
            if (topics.Count != 0)
            {
                var cards = TakeAllCards(topics);
                ShowCards(cards);
                var needDeleteCards = FindCards(cards);
                ShowCards(needDeleteCards);
                var key = userInteractor.QuestionAnswerKey("If you realy want to delete these flashcards, press Enter\n\r" +
                    "If don't want, press else");
                userInteractor.WriteLine("Okey");
                if (key == UserAction.Enter)
                {
                    await fileMaster.DeleteFlashcards(needDeleteCards);
                    userInteractor.WriteLine("Flashcards have been deleted");
                }
            }
        }
        private bool CheckFlashcard(List<Flashcard> needFlashcards, Flashcard flashcard)
        {
            foreach (var needFlashcard in needFlashcards)
            {
                if (needFlashcard.Id == flashcard.Id)
                {
                    userInteractor.WriteLine($"You added this flashcard ({CreateInformString(flashcard)}) earlier");
                    return false;
                }
            }
            userInteractor.WriteLine("Okey");
            return true;
        }
        private List<Flashcard> FindCards(List<Flashcard> flashcards)
        {
            var needFlashcards = new List<Flashcard>();
            while (true)
            {
                var flashcardName = userInteractor.QuestionAnswerInUkr("Write a flashcard name (back or original word)");
                var cards = flashcards.Where(x => x.BackOrOriginalWord == flashcardName);
                switch (cards.Count())
                {
                    case 0:
                        userInteractor.WriteLine("Don't have this card");
                        break;
                    case 1:
                        if (CheckFlashcard(needFlashcards, cards.First()))
                        {
                            needFlashcards.AddRange(cards);
                        }
                        break;
                    default:
                        var (haveCard, someFlashcards) = FindSomeCards(cards);
                        if (haveCard)
                        {
                            foreach (var someFlashcard in someFlashcards)
                            {
                                if (CheckFlashcard(needFlashcards, someFlashcard))
                                {
                                    needFlashcards.Add(someFlashcard);
                                }
                            }
                        }
                        break;
                }
                var key = userInteractor.QuestionAnswerKey("If you want to delete more flashcards, press Enter\n\r" +
                    "If you want to delete found flashcards, press else");
                if (key != UserAction.Enter)
                {
                    return needFlashcards;
                }
            }
        }
        private (bool haveCard, IEnumerable<Flashcard> flashcard) FindSomeCards(IEnumerable<Flashcard> flashcards)
        {
            userInteractor.WriteLine("You have some cards with this name, choose a need card");
            var i = 0;
            foreach (var flashcard in flashcards)
            {
                i++;
                userInteractor.WriteLine($"Id = {i}\n\r{CreateInformString(flashcard)}");
            }
            while (true)
            {
                var line = userInteractor.QuestionAnswer("Write need Id\n\r" +
                    "If you want delete all these cards, write 'd' or 'D'\n\r" +
                    "If you want don't want delete these press 'Enter' ");
                if (line == "")
                {
                    return (false, flashcards);
                }
                else if (line == "d" || line == "D")
                {
                    return (true, flashcards);
                }
                else
                {
                    try
                    {
                        var id = Convert.ToInt32(line);
                        if (id > 0 && id <= flashcards.Count())
                        {
                            return (true, new List<Flashcard> { flashcards.ElementAt(id - 1) });
                        }
                        else
                        {
                            userInteractor.WriteLine("Don't have this id, write else");
                        }
                    }
                    catch (FormatException ex)
                    {
                        userInteractor.WriteLine(ex.Message);
                    }
                }
            }
        }
        private List<Flashcard> TakeAllCards(List<TopicWithFlashcards> topics)
        {
            return topics.SelectMany(x => x.Flashcards).ToList();
        }
        private void ShowCards(List<Flashcard> flashcards)
        {
            foreach (var flashcard in flashcards)
            {
                userInteractor.WriteLine(CreateInformString(flashcard));
            }
        }
        private string CreateInformString(Flashcard flashcard)
        {
            return $"{flashcard.Topic.Name} - " +
                    $"{flashcard.FrontOrForeignTranslation} - " +
                    $"{flashcard.Transcription} - " +
                    $"{flashcard.BackOrOriginalWord}";
        }
    }
}
