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

        public async Task Run()
        {
            topicMet.WriteTopics();
            if (await FindNeedCards())
            {
                WriteCards();
                var typesOfStudy = FindTypesOfStudy();
                if (typesOfStudy.TypeOfStudy != TypesOfStudy.noStudy && typesOfStudy.TypeOfTranslation != TypesOfStudy.noStudy)
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
                    userInteractor.WriteLine($"{flashcard.Topic.Name} - " +
                        $"{flashcard.FrontOrForeignTranslation} - " +
                        $"{flashcard.Transcription} - " +
                        $"{flashcard.BackOrOriginalWord}");
                }
            }
        }
        private TypesOfStudy FindUserActionStudy()
        {
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey($"If you want to {TypesOfStudy.normal} study, press 'n',\n\r" +
                    $"If you want to {TypesOfStudy.bad} study, press 'b'\n\r" +
                    "If you want to exit, press 'Escape'");
                if (key == UserAction.N)
                {
                    return TypesOfStudy.normal;
                }
                else if (key == UserAction.B)
                {
                    return TypesOfStudy.bad;
                }
                else if (key == UserAction.Escape)
                {
                    return TypesOfStudy.noStudy;
                }
                else
                {
                    userInteractor.WriteLine("Write else");
                }
            }
        }
        private TypesOfStudy FindUserActionTranslation()
        {
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey($"If you want to translate {TypesOfStudy.fromOriginalToForeign}, press 'n',\n\r" +
                    $"If you want to translate {TypesOfStudy.fromForeignToOriginal}, press 'b'\n\r" +
                    "If you want to exit, press 'Escape'");
                if (key == UserAction.N)
                {
                    return TypesOfStudy.fromOriginalToForeign;
                }
                else if (key == UserAction.B)
                {
                    return TypesOfStudy.fromForeignToOriginal;
                }
                else if (key == UserAction.Escape)
                {
                    return TypesOfStudy.noStudy;
                }
                else
                {
                    userInteractor.WriteLine("Write else");
                }
            }
        }
        private UserTypesOfStudy FindTypesOfStudy()
        {
            UserTypesOfStudy userTypesOfStudy = new UserTypesOfStudy();
            userTypesOfStudy.TypeOfStudy = FindUserActionStudy();
            if (userTypesOfStudy.TypeOfStudy != TypesOfStudy.noStudy)
            {
                userTypesOfStudy.TypeOfTranslation = FindUserActionTranslation();
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
            flashcards = await fileMaster.TakeAllFlashcards();
            if (flashcards == null || flashcards.Count() == 0)
            {
                userInteractor.WriteLine("You don't have any flashcards, create them");
                return false;
            }
            return true;
        }
        private async Task<bool> InitializeTopicCards()
        {
            if (await topicMet.FindTopic())
            {
                flashcards.AddRange(await fileMaster.ReadData(topicMet.Topic));
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
                "If you want to end, press 'Escape',\n\r");
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
                    $"transcription - {transcription}");
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
                    $"transcription - {transcription}");
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
    }
}
