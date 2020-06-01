using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards
{
    class ConsoleUserInteractor : IUserInteractor
    {
        public string QuestionAnswerInUkr(string question)
        {
            Console.WriteLine(question);
            var line = Console.ReadLine();
            var stringBuilder = new StringBuilder();
            foreach (var sign in line)
            {
                if (sign == '?')
                {
                    stringBuilder.Append('i');
                    continue;
                }
                stringBuilder.Append(sign);
            }
            return stringBuilder.ToString();
        }
        public string QuestionAnswer(string question)
        {
            Console.WriteLine($"\n\r{question}");
            return Console.ReadLine();
        }
        public UserAction QuestionAnswerKey(string question)
        {
            Console.WriteLine($"\n\r{question}");
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.Enter:
                    return UserAction.Enter;
                case ConsoleKey.Escape:
                    return UserAction.Escape;
                case ConsoleKey.L:
                    return UserAction.L;
                case ConsoleKey.F:
                    return UserAction.F;
                case ConsoleKey.A:
                    return UserAction.A;
                case ConsoleKey.T:
                    return UserAction.T;
                case ConsoleKey.D:
                    return UserAction.D;
                case ConsoleKey.N:
                    return UserAction.N;
                case ConsoleKey.B:
                    return UserAction.B;
                default:
                    return UserAction.Else;
            }
        }
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
        public void ClearWindow()
        {
            Console.Clear();
        }
    }
}
