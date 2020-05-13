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
        private List<Flashcard> flashcards;

        private const string PathAllTopics = "D:\\temp\\Flashcards";

        public async Task Run()
        {
            topicMet.WriteTopics();
            if (await FindNeedCards())
            {
                WriteCards();
                var typesOfStudy = FindTypesOfStudy();
                if (typesOfStudy.TypeOfStudy != TypesOfStudy.noStudy)
                {
                    StudyMet(typesOfStudy);
                }
            }
        }
        private void WriteCards()
        {
            var key = userInteractor.QuestionAnswerKey("If you want to watch your cards, press 'Enter', if not - press else");
            if (key == UserAction.Enter)
            {
                foreach (var flashcard in flashcards)
                {
                    userInteractor.WriteLine($"{flashcard.Topic} - " +
                        $"{flashcard.FrontOrForeignTranslation} - " +
                        $"{flashcard.Transcription} - " +
                        $"{flashcard.BackOrOriginalWord}");
                }
            }
        }
        private UserTypesOfStudy FindTypesOfStudy()
        {
            UserTypesOfStudy userTypesOfStudy = new UserTypesOfStudy();
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey($"If you want to {TypesOfStudy.normal} study, press 'n',\n\r" +
                    $"If you want to {TypesOfStudy.bad} study, press 'b'\n\r" +
                    "If you want to exit, press 'Escape'");
                if (key == UserAction.N)
                {
                    userTypesOfStudy.TypeOfStudy = TypesOfStudy.normal;
                    break;
                }
                else if (key == UserAction.B)
                {
                    userTypesOfStudy.TypeOfStudy = TypesOfStudy.bad;
                    break;
                }
                else if (key == UserAction.Escape)
                {
                    return new UserTypesOfStudy(TypesOfStudy.noStudy, TypesOfStudy.noStudy);
                }
                else
                {
                    userInteractor.WriteLine("Write else");
                }
            }
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey($"If you want to translate {TypesOfStudy.fromOriginalToForeign}, press 'n',\n\r" +
                    $"If you want to translate {TypesOfStudy.fromForeignToOriginal}, press 'b'\n\r" +
                    "If you want to exit, press 'Escape'");
                if (key == UserAction.N)
                {
                    userTypesOfStudy.TypeOfTranslation = TypesOfStudy.fromOriginalToForeign;
                    break;
                }
                else if (key == UserAction.B)
                {
                    userTypesOfStudy.TypeOfTranslation = TypesOfStudy.fromForeignToOriginal;
                    break;
                }
                else if (key == UserAction.Escape)
                {
                    return new UserTypesOfStudy(TypesOfStudy.noStudy, TypesOfStudy.noStudy);
                }
                else
                {
                    userInteractor.WriteLine("Write else");
                }
            }
            return userTypesOfStudy;
        }
        private async Task<bool> FindNeedCards()
        {
            while (true)
            {
                flashcards = new List<Flashcard>();
                var key = userInteractor.QuestionAnswerKey("If you want to study all cards, press 'a',\n\r" +
                    "If you want to study some topic cards, press 't'\n\r" +
                    "If you want to exit, press 'Escape'");
                switch (key)
                {
                    case UserAction.A:
                        return await InitializeAllCards();
                    case UserAction.T:
                        return await InitializeTopicCards();
                    case UserAction.Escape:
                        return false;
                    default:
                        userInteractor.WriteLine("Write else");
                        break;
                }
            }
        }
        private async Task<bool> InitializeAllCards()
        {
            flashcards = await TakeAllFlashcards();
            if (flashcards == new List<Flashcard>())
            {
                userInteractor.WriteLine("You don't have any flashcards, create them");
                return false;
            }
            return true;
        }
        private async Task<bool> InitializeTopicCards()
        {
            if (topicMet.FindTopic())
            {
                flashcards.AddRange(await TakeFlashCards(topicMet.PathTopic));
                if (flashcards == null || flashcards.Count() == 0)
                {
                    userInteractor.WriteLine("You don't have flashcards in this topic, create them");
                    return false;
                }
                return true;
            }
            else
            {
                userInteractor.WriteLine("You don't have this topic, create it");
                return false;
            }
        }
        private void StudyMet(UserTypesOfStudy userTypesOfStudy)
        {
            var key = userInteractor.QuestionAnswerKey("If you want to continue, press 'Enter',\n\r" +
                "if you want to end, press 'Escape',\n\r" +
                "if you agree, press 'Enter'");
            if (key == UserAction.Enter)
            {
                Random rand = new Random();
                switch (userTypesOfStudy.TypeOfStudy)
                {
                    case TypesOfStudy.normal:
                        NormalStudy(userTypesOfStudy.TypeOfTranslation, rand);
                        break;
                    case TypesOfStudy.bad:
                        BadStudy(userTypesOfStudy.TypeOfTranslation, rand);
                        break;
                    default:
                        break;
                }
            }
        }
        private void NormalStudy(TypesOfStudy typeOfTranslation, Random rand)
        {
            while (true)
            {
                var(question, answer, transcription) = FindNeedCardData(typeOfTranslation, rand);
                userInteractor.QuestionAnswerKey(question);
                var key = userInteractor.QuestionAnswerKey($"{answer},\n\r" +
                    $"tr - {transcription}");
                if (key == UserAction.Escape)
                {
                    return;
                }
            }
        }
        private void BadStudy(TypesOfStudy typeOfTranslation, Random rand)
        {
            while (true)
            {
                var (question, answer, transcription) = FindNeedCardData(typeOfTranslation, rand);
                var word = userInteractor.QuestionAnswer(question);
                if (word == answer)
                {
                    userInteractor.WriteLine("You are right");
                }
                else
                {
                    userInteractor.WriteLine("You are wrong");
                }
                var key = userInteractor.QuestionAnswerKey($"{answer},\n\r" +
                    $"tr - {transcription}");
                if (key == UserAction.Escape)
                {
                    return;
                }
            }
        }
        private (string question, string answer, string transcription) FindNeedCardData(TypesOfStudy typeOfTranslation, Random rand)
        {
            userInteractor.ClearWindow();
            var num = rand.Next(flashcards.Count());
            if (typeOfTranslation == TypesOfStudy.fromOriginalToForeign)
            {
                return (flashcards[num].BackOrOriginalWord, flashcards[num].FrontOrForeignTranslation, flashcards[num].Transcription);
            }
            else
            {
                return (flashcards[num].FrontOrForeignTranslation, flashcards[num].BackOrOriginalWord, flashcards[num].Transcription);
            }
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
        private async Task<List<Flashcard>> TakeFlashCards(string path)
        {
            return await fileMaster.ReadData(path);
        }
    }
}
