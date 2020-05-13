using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class TopicMet
    {
        public TopicMet(FileMaster fileMaster, IUserInteractor userInteractor)
        {
            this.fileMaster = fileMaster;
            this.userInteractor = userInteractor;
        }
        IUserInteractor userInteractor;
        private FileMaster fileMaster;

        public const string PathAllTopics = "D:\\temp\\Flashcards";
        public string PathTopic;
        public string Topic { get; set; }
        public List<string> topics;


        public void FindTopics()
        {
            userInteractor.WriteLine("All topics you have:");
            topics = fileMaster.GetDirectoriesName(PathAllTopics);
        }
        public void WriteTopics()
        {
            foreach (var topic in topics)
            {
                userInteractor.WriteLine(topic);
            }
        }
        public bool FindOrCreateTopic()
        {
            Topic = userInteractor.QuestionAnswer("Write a need topic");
            PathTopic = $"{PathAllTopics}\\{Topic}\\Flashcards.json";
            fileMaster.CreateDirectory($"{PathAllTopics}\\{Topic}");
            foreach (var topic in topics)
            {
                if (topic == Topic)
                {
                    return true;
                }
            }
            topics.Add(Topic);
            return false;
        }
        public bool FindTopic()
        {
            Topic = userInteractor.QuestionAnswer("Write a need topic");
            PathTopic = $"{PathAllTopics}\\{Topic}\\Flashcards.json";
            return fileMaster.FileExists(PathTopic);
        }
    }
}
