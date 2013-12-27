using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserQueryM
{
    /// <summary>
    /// Prints out the question on console and requests user by input to give an answer
    /// </summary>
    public class UserQueryConsole : IUserQuery
    {
        /// <summary>
        /// Asks the user a certain question and gives a set of choices
        /// </summary>
        /// <param name="question">Question to be asked</param>
        /// <param name="choices">Choices to be possible or null if every answer is acceptable</param>
        /// <param name="defaultChoice">Default choice of the user</param>
        /// <returns>Resulting choice</returns>
        public string Ask(string question, IEnumerable<string> choices, string defaultChoice)
        {
            Console.WriteLine();
            Console.WriteLine(question);

            if (choices != null)
            {
                var comma = "";

                Console.Write(" [");
                foreach (var choice in choices)
                {
                    Console.Write(comma + choice);
                    comma = "/";
                }

                Console.Write("] ");
            }
            else
            {
                Console.Write(" ");
            }

            Console.Write("({0}): ", defaultChoice);

            var result = Console.ReadLine().Trim();
            if (result == string.Empty)
            {
                result = defaultChoice;
            }

            if (choices != null && !choices.Contains(result))
            {
                return Ask(question, choices, defaultChoice);
            }

            Console.WriteLine();

            return result;
        }
    }
}
