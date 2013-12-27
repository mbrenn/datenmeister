//-----------------------------------------------------------------------
// <copyright file="CommentParser.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Parser.Dcss
{
    using System;
    using System.Text;

    /// <summary>
    /// Mit Hilfe dieses Parser können Kommentare aus
    /// verschiedenen Quelldateien herausgenommen werden.
    /// </summary>
    public static class CommentParser
    {
        /// <summary>
        /// Verarbeitet den übergebenen Eingangsstring so, 
        /// dass alle Kommentare in der Syntax /* comment */ 
        /// entfernt werden. 
        /// </summary>
        /// <param name="inputText">Source code, which should be 
        /// stripped. </param>
        /// <returns>Ergebnis ohne Kommentare</returns>
        public static string StripStarComments(string inputText)
        {
            var commentActive = false;
            var lastCharacter = ' ';
            var currentPosition = 0;
            var length = inputText.Length;
            var result = new StringBuilder();
            var first = true;

            while (currentPosition < length)
            {
                var currentCharacter = inputText[currentPosition];

                if (commentActive)
                {
                    if (lastCharacter == '*' &&
                        currentCharacter == '/')
                    {
                        commentActive = false;
                        first = true;
                    }
                }
                else
                {
                    if (lastCharacter == '/' &&
                        currentCharacter == '*')
                    {
                        commentActive = true;
                    }
                    else if (!first)
                    {
                        result.Append(lastCharacter);
                    }

                    first = false;                  
                }

                lastCharacter = currentCharacter;
                currentPosition++;
            }

            if (!commentActive && !first)
            {
                result.Append(lastCharacter);
            }

            return result.ToString();
        }
    }
}
