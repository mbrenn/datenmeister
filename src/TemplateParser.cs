//-----------------------------------------------------------------------
// <copyright file="TemplateParser.cs" company="Martin Brenn">
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using BurnSystems.Collections;
    using BurnSystems.Test;

    /// <summary>
    /// Dieser Delegate wird genutzt um externe Kommandos einzubinden
    /// </summary>
    /// <param name="parser">Der Parser selbst</param>
    /// <param name="command">The command</param>
    /// <returns>Result of the external command</returns>
    public delegate string ExternalCommand(TemplateParser parser, string command);

    /// <summary>
    /// This is the core
    /// </summary>
    public class TemplateParser
    {
        #region Interne Variablen

        /// <summary>
        /// Flag, ob gerade geparst wird. Dieser Wert wird zur
        /// Laufzeitsicherheit benütigt.
        /// </summary>
        private bool parsing;

        /// <summary>
        /// Parser Variables
        /// </summary>
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        /// <summary>
        /// Stores the variables for latebinding
        /// </summary>
        private Dictionary<string, Func<object>> lateBindingVariables =
            new Dictionary<string, Func<object>>();

        /// <summary>
        /// Stringbuilder, which contains the result
        /// </summary>
        private StringBuilder result;

        /// <summary>
        /// Flag, if the current scope is active
        /// </summary>
        private bool active;

        /// <summary>
        /// Stack of scope
        /// </summary>
        private Stack<ScopeStack> stack;

        /// <summary>
        /// Current position of parser
        /// </summary>
        private int currentPosition;

        /// <summary>
        /// Externes Kommando, das aufgerufen wird, wenn keiner der internen
        /// Kommandos zu einem angeforderten Kommando passt
        /// </summary>
        private ExternalCommand externalCommand;

        /// <summary>
        /// Initializes a new instance of the TemplateParser class.
        /// </summary>
        public TemplateParser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the TemplateParser class.
        /// </summary>
        /// <param name="variables">Variables to be set</param>
        public TemplateParser(Dictionary<string, object> variables)
            : this()
        {
            foreach (var pair in variables)
            {
                this.variables[pair.Key] = pair.Value;
            }
        }

        #endregion

        /// <summary>
        /// Gets the parser Variables
        /// </summary>
        public Dictionary<string, object> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Gets or sets the external command, which is called, when 
        /// no command fits to this
        /// </summary>
        public ExternalCommand ExternalCommand
        {
            get { return this.externalCommand; }
            set { this.externalCommand = value; }
        }

        /// <summary>
        /// Gets or sets the result of the parsing
        /// </summary>
        protected StringBuilder Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        /// <summary>
        /// Adds variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="value">Value of variable</param>
        public void AddVariable(string name, object value)
        {
            this.variables[name] = value;
        }

        /// <summary>
        /// Adds a variable for latebinding
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="factory">Factorymethod, which creates the variable</param>
        public void AddLateBinding(string name, Func<object> factory)
        {
            this.lateBindingVariables[name] = factory;
        }

        /// <summary>
        /// Parses stream
        /// </summary>
        /// <param name="reader">Stream to be parsed</param>
        /// <returns>Result of Streaming</returns>
        public string Parse(TextReader reader)
        {
            Ensure.IsNotNull(reader);

            return this.Parse(reader.ReadToEnd());
        }

        /// <summary>
        /// Parses string
        /// </summary>
        /// <param name="content">Content to be parsed</param>
        /// <returns>String to be parsed</returns>
        public string Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                // Sonderbehandlung 
                return string.Empty;
            }
        
            try
            {
                // Normales Parsing
                if (this.parsing)
                {
                    throw new InvalidOperationException(
                        Localization_Parser.TemplateParser_AlreadyParsing);
                }

                this.parsing = true;

                // Initialize Variables
                this.currentPosition = 0;
                int length = content.Length;
                this.result = new StringBuilder();
                this.active = true;
                this.stack = new Stack<ScopeStack>();

                while (true)
                {
                    int position = content.IndexOf('@', this.currentPosition);

                    if ((position == -1) || (position == content.Length - 1))
                    {
                        position = length;
                    }

                    if (this.active)
                    {
                        this.result.Append(content.Substring(this.currentPosition, position - this.currentPosition));
                    }

                    // Check if end of file
                    if (position >= length)
                    {
                        break;
                    }

                    // Search for end of command, the end of command is set by a closing bracket
                    if (content[position + 1] == '[')
                    {
                        bool quote = false; // True, wenn sich der Parser gerade in den Anführungszeichen befindet
                        int cursorEndPosition = position + 2;
                        int endPosition;
                        int openPosition;
                        int opened = 0;        // Anzahl der geöffneten Klammern
                        while (true)
                        {
                            openPosition = content.IndexOf('[', cursorEndPosition);
                            endPosition = content.IndexOf(']', cursorEndPosition);

                            if (endPosition == -1)
                            {
                                break;
                            }

                            int quotePosition = content.IndexOf('"', cursorEndPosition);
                            if (quotePosition == -1)
                            {
                                break;
                            }
                            else if (endPosition > quotePosition)
                            {
                                // Die Anführungszeichen sind vor der geschlossenen Klammer
                                quote = !quote;
                                cursorEndPosition = quotePosition + 1;
                            }
                            else
                            {
                                // überprüfe, ob sich eine zu üffnende Klammer vor der
                                // zu schlieüenden befindet
                                if (openPosition != -1 && openPosition < endPosition)
                                {
                                    opened++;
                                    cursorEndPosition = openPosition + 1;
                                    continue;
                                }
                                else if (opened > 0)
                                {
                                    // Es existieren noch offene Klammern
                                    opened--;
                                    cursorEndPosition = endPosition + 1;
                                }
                                else if (quote)
                                {
                                    // Es sind noch Anführungszeichen offen
                                    cursorEndPosition = endPosition + 1;
                                }
                                else
                                {
                                    // Es passt alles
                                    break;
                                }
                            }
                        }

                        if (endPosition != -1)
                        {
                            string command = content.Substring(position + 2, endPosition - position - 2);

                            if (!this.ExecuteCommand(command, endPosition + 1))
                            {
                                this.currentPosition = endPosition + 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (this.active)
                        {
                            this.result.Append('@');
                        }

                        this.currentPosition = position + 1;
                    }
                }

                return this.result.ToString();
            }
            catch (Exception exc)
            {
                throw new ParserException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        Localization_Parser.TemplateParser_Exception, 
                        exc.Message, 
                        content), 
                    exc);
            }
            finally
            {
                this.parsing = false;
            }
        }

        /// <summary>
        /// Gets the value of a variable with the given name. 
        /// The variable is first searched in the <c>Variables</c>-Member
        /// and afterwards in the late binding-Array. 
        /// If the value is not found, <c>string.Empty</c> will be returned.
        /// </summary>
        /// <param name="valueName">Name of the requested variable.</param>
        /// <returns>Content of variable</returns>
        public object GetValue(string valueName)
        {
            object result;
            if (this.Variables.TryGetValue(valueName, out result))
            {
                return result;
            }
            else
            {
                Func<object> lateBinding;
                if (this.lateBindingVariables.TryGetValue(valueName, out lateBinding))
                {
                    // Late binding of variable
                    result = lateBinding();
                    this.Variables[valueName] = result;
                    return result;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Parses an exception
        /// </summary>
        /// <param name="expression">Expression to be parsed</param>
        /// <returns>Result of expression</returns>
        public object ParseExpression(string expression)
        {
            var expressionParser = new ExpressionParser(this);
            return expressionParser.Parse(expression);
        }

        /// <summary>
        /// Klont den aktuellen Parser.
        /// </summary>
        /// <returns>Der geklonte Parser</returns>
        public TemplateParser CloneParser()
        {
            var clonedParser = new TemplateParser();
            foreach (var pair in this.Variables)
            {
                clonedParser.AddVariable(pair.Key, pair.Value);
            }

            clonedParser.ExternalCommand = ExternalCommand;

            return clonedParser;
        }

        /// <summary>
        /// Executes a global function
        /// </summary>
        /// <param name="functionName">Name of the Function to be executed</param>
        /// <param name="parameters">Parameters of function</param>
        /// <returns>Result of the global testfunction</returns>
        internal static object ExecuteFunction(string functionName, List<object> parameters)
        {
            switch (functionName)
            {
                case "globaltestfunction":
                    int nX = (int)parameters[0];
                    string strY = (string)parameters[1];
                    return (nX * nX) + strY.ToUpper(CultureInfo.CurrentUICulture);
            }

            return string.Empty;
        }

        /// <summary>
        /// Executes command and stores the result into stringbuilder
        /// </summary>
        /// <param name="command">Command line</param>
        /// <param name="endPosition">EndPosition of the command</param>
        /// <returns>Flag, if cursor is set by the function</returns>
        protected virtual bool ExecuteCommand(string command, int endPosition)
        {
            int length = command.Length;
            if (string.IsNullOrEmpty(command))
            {
                return false;
            }

            if (command[0] == '=')
            {
                if (this.active)
                {
                    var expressionParser = new ExpressionParser(this);
                    this.result.Append(expressionParser.ParseForString(command.Substring(1)));
                }

                return false;
            }

            if (length > 3 && command.Substring(0, 3) == "IF:")
            {
                this.ExecuteIfCommand(command, endPosition);
                return false;
            }

            if (command == "ENDIF")
            {
                var stackEntry = (ScopeIfStack)this.stack.Pop();
                if (stackEntry.IsRelevant)
                {
                    this.active = stackEntry.IsActive;
                }

                return false;
            }

            if (command == "ELSE")
            {
                var stackEntry = (ScopeIfStack)this.stack.Peek();
                if (stackEntry.IsRelevant)
                {
                    this.active = !this.active;
                }

                return false;
            }

            if (length > 4 && command.Substring(0, 4) == "SET:")
            {
                if (this.active)
                {
                    this.ExecuteSetStatement(command);
                }

                return false;
            }

            if (length > 6 && command.Substring(0, 6) == "WHILE:")
            {
                var expression = command.Substring(6);

                var whileStack = new ScopeWhileStack();
                var expressionParser = new ExpressionParser(this);

                whileStack.EndPosition = endPosition;
                whileStack.Expression = expression;
                this.active = expressionParser.ParseForBoolean(expression);

                this.stack.Push(whileStack);
                return false;
            }

            if (command == "ENDWHILE")
            {
                var whileStack = (ScopeWhileStack)this.stack.Peek();

                var expressionParser = new ExpressionParser(this);
                bool loop = expressionParser.ParseForBoolean(whileStack.Expression);
                if (loop)
                {
                    this.currentPosition = whileStack.EndPosition;
                    return true;
                }
                else
                {
                    this.stack.Pop();
                    return false;
                }
            }

            if (length > 8 && command.Substring(0, 8) == "FOREACH:")
            {
                IEnumerable enumerable;
                int position = command.IndexOf("IN", StringComparison.Ordinal);

                ScopeForeachStack foreachStack = new ScopeForeachStack();
                foreachStack.IsActive = this.active;
                foreachStack.EndPosition = endPosition;

                if (position == -1)
                {
                    this.active = false;
                }
                else
                {
                    string variable = command.Substring(8, position - 9);
                    string expression = command.Substring(position + 3);

                    var expressionParser = new ExpressionParser(this);
                    if (this.active)
                    {
                        // Nur im aktiven Fall wird die Variable wirklich geparst. Ansonsten ist
                        // dies nicht nütig
                        enumerable = expressionParser.Parse(expression) as IEnumerable;
                    }
                    else
                    {
                        enumerable = null;
                    }

                    if (enumerable == null)
                    {
                        this.active = false;
                        foreachStack.Enumerator = null;
                    }
                    else
                    {
                        foreachStack.Enumerator = enumerable.GetEnumerator();
                    }

                    expressionParser = new ExpressionParser(this);
                    foreachStack.Variablename = expressionParser.ParseForString(variable);
                }

                if (this.active)
                {
                    if (foreachStack.Enumerator.MoveNext())
                    {
                        this.variables[foreachStack.Variablename] = foreachStack.Enumerator.Current;
                    }
                    else
                    {
                        this.active = false;
                    }
                }

                this.stack.Push(foreachStack);
                return false;
            }

            if (command == "ENDFOREACH")
            {
                var foreachStack = (ScopeForeachStack)this.stack.Peek();

                bool loop =
                    foreachStack.Enumerator != null
                        && foreachStack.Enumerator.MoveNext();

                if (loop)
                {
                    this.currentPosition = foreachStack.EndPosition;
                    this.Variables[foreachStack.Variablename] = foreachStack.Enumerator.Current;
                    return true;
                }
                else
                {
                    foreachStack = (ScopeForeachStack)this.stack.Pop();
                    this.active = foreachStack.IsActive;
                    return false;
                }
            }

            if (command == "LIST")
            {
                this.result.Append(
                    StringManipulation.Join(
                        this.variables,
                        x => String.Format("{0}: {1}", x.Key, x.Value),
                        ", "));

                return false;
            }

            if (this.externalCommand != null)
            {
                if (this.active)
                {
                    this.result.Append(this.externalCommand(this, command));
                }
            }

            return false;
        }

        /// <summary>
        /// Executes a set statement
        /// </summary>
        /// <param name="command">Command of set statement</param>
        private void ExecuteSetStatement(string command)
        {
            int equalPosition = command.IndexOf('=');
            if (equalPosition == -1)
            {
                return;
            }

            string variable = command.Substring(4, equalPosition - 4);
            string expression = command.Substring(equalPosition + 1);

            // Parses variablename
            var expressionParser = new ExpressionParser(this);
            variable = expressionParser.ParseForString(variable);

            // Parses value
            expressionParser = new ExpressionParser(this);
            this.variables[variable] = expressionParser.Parse(expression);
            return;
        }

        /// <summary>
        /// Executes an if command
        /// </summary>
        /// <param name="command">Command of if-statement</param>
        /// <param name="endPosition">End position of this command</param>
        private void ExecuteIfCommand(string command, int endPosition)
        {
            // If clause
            var expression = command.Substring(3);
            var expressionParser = new ExpressionParser(this);
            bool activeForIf;
            bool relevant = true;

            var stackOfIf = new ScopeIfStack();
            stackOfIf.EndPosition = endPosition;
            stackOfIf.IsActive = this.active;

            if (this.active)
            {
                activeForIf = expressionParser.ParseForBoolean(expression);
                this.active = activeForIf;
            }
            else
            {
                activeForIf = false;
                relevant = false;
            }

            stackOfIf.IsRelevant = relevant;

            this.stack.Push(stackOfIf);
        }

        #region Classes for internal stack

        /// <summary>
        /// Defines the abstract class for scope
        /// </summary>
        internal abstract class ScopeStack
        {
            /// <summary>
            /// Gets or sets the end-Position of statement, which created this stack
            /// </summary>
            public int EndPosition
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Defines the stack for the ifstatements
        /// </summary>
        internal class ScopeIfStack : ScopeStack
        {
            /// <summary>
            /// Gets or sets a value indicating whether this scope is currently active
            /// </summary>
            public bool IsActive
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this the if-statement is in an active scope
            /// </summary>
            public bool IsRelevant
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Defines the stack for while-statements
        /// </summary>
        private class ScopeWhileStack : ScopeStack
        {
            /// <summary>
            /// Gets or sets of Expression of while
            /// </summary>
            public string Expression
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Defines the stack for the foreach-statements
        /// </summary>
        private class ScopeForeachStack : ScopeStack
        {
            /// <summary>
            /// Gets or sets the name of the variable
            /// </summary>
            public string Variablename
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the enumerator for this scope
            /// </summary>
            public IEnumerator Enumerator
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a value indicating whether this foreach scope is active
            /// </summary>
            public bool IsActive
            {
                get;
                set;
            }
        }

        #endregion
    }
}