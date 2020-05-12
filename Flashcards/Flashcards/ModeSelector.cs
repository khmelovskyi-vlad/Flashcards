using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class ModeSelector
    {
        public ModeSelector(IUserInteractor userInteractor)
        {
            this.userInteractor = userInteractor;
        }
        private IUserInteractor userInteractor;
        private FileMaster fileMaster = new FileMaster();
        private TopicMet topicMet;
        public async Task Run()
        {
            topicMet = new TopicMet(fileMaster, userInteractor);
            topicMet.FindTopics();
            while (true)
            {
                var key = userInteractor.QuestionAnswerKey("What do you want to do?\n\r" +
                    "If you want to add cards, press 'a'\n\r" +
                    "If you want to learn cards, press 'l'\n\r" +
                    "If you want to exit the application, press 'Esc'");
                switch (key)
                {
                    case UserAction.A:
                        Input input = new Input(fileMaster, userInteractor, topicMet);
                        await input.Run();
                        break;
                    case UserAction.L:
                        Study study = new Study(fileMaster, userInteractor, topicMet);
                        await study.Run();
                        break;
                    case UserAction.Escape:
                        Console.WriteLine("bye");
                        return;
                    default:
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
