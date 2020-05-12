using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    interface IUserInteractor
    {
        string QuestionAnswerInUkr(string question);
        string QuestionAnswer(string question);
        UserAction QuestionAnswerKey(string question);
        void WriteLine(string message);
    }
}
