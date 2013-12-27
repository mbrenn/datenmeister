//-----------------------------------------------------------------------
// <copyright file="UserCommandTemplateParser.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems.Parser
{
    using BurnSystems.Test;

    /// <summary>
    /// Dieser Delegat wird genutzt um eine eigene Behandlung von Kommandos
    /// durchzuführen
    /// </summary>
    /// <param name="command">Command to be executed</param>
    /// <param name="endPosition">End position of command</param>
    /// <returns>Ergebnis, welcher in die Vorlage eingefügt wird</returns>
    public delegate string ExecuteCommand(string command, int endPosition);

    /// <summary>
    /// Dieser Parser wird genutzt, um eine eigene Behandlung der
    /// Kommandos zu erzielen
    /// </summary>
    public class UserCommandTemplateParser : TemplateParser
    {
        /// <summary>
        /// The delegate of command
        /// </summary>
        private ExecuteCommand executeCommandDelegate;

        /// <summary>
        /// Initializes a new instance of the UserCommandTemplateParser class. 
        /// </summary>
        /// <param name="commandDelegate">Command delegation</param>
        public UserCommandTemplateParser(ExecuteCommand commandDelegate)
        {
            Ensure.IsNotNull(commandDelegate);
            this.executeCommandDelegate = commandDelegate;
        }

        /// <summary>
        /// Lässt ein Kommando vom Delegaten ausführen
        /// </summary>
        /// <param name="command">Command to be executed</param>
        /// <param name="endPosition">End position</param>
        /// <returns>false, because offset is not changed</returns>
        protected override bool ExecuteCommand(string command, int endPosition)
        {
            this.Result.Append(this.executeCommandDelegate(command, endPosition));
            return false;
        }
    }
}
