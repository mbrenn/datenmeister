//-----------------------------------------------------------------------
// <copyright file="CommandLineEvaluater.cs" company="Martin Brenn">
//     Alle Rechte vorbehalten. 
// 
//     Die Inhalte dieser Datei sind ebenfalls automatisch unter 
//     der AGPL lizenziert. 
//     http://www.fsf.org/licensing/licenses/agpl-3.0.html
//     Weitere Informationen: http://de.wikipedia.org/wiki/AGPL
// </copyright>
//-----------------------------------------------------------------------

namespace BurnSystems
{
    using System;
    using System.Collections.Generic;
    using BurnSystems.Collections;
    using BurnSystems.CommandLine;

    /// <summary>
    /// Wertet die Kommandozeilen aus
    /// </summary>
    public class CommandLineEvaluator
    {
        /// <summary>
        /// Nichtbenannte Argument
        /// </summary>
        private List<string> unnamedArguments =
            new List<string>();

        /// <summary>
        /// Benannte Argumente
        /// </summary>
        private NiceDictionary<string, string> namedArguments =
            new NiceDictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        public CommandLineEvaluator(string[] arguments)
        {
            this.Parse(arguments);
        }

        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluator class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        /// <param name="definitions">List of definitions, that will define the argument structure to be parsed</param>
        public CommandLineEvaluator(string[] arguments, IEnumerable<ICommandLineDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                definition.BeforeParsing(this);                
            }

            this.Parse(arguments);

            foreach (var definition in definitions)
            {
                definition.AfterParsing(this);
            }
        }

        /// <summary>
        /// Parses the arguments
        /// </summary>
        /// <param name="arguments">Arguments to be parsed</param>
        private void Parse(string[] arguments)
        {
            foreach (var argument in arguments)
            {
                if (argument.Length == 0)
                {
                    continue;
                }

                if (argument.StartsWith("--", StringComparison.Ordinal))
                {
                    int pos = argument.IndexOf('=');
                    if (pos == -1)
                    {
                        this.namedArguments[argument.Substring(2)] = "1";
                    }
                    else
                    {
                        this.namedArguments[argument.Substring(2, pos - 2)] =
                            argument.Substring(pos + 1);
                    }

                    continue;
                }
                else if (argument[0] == '-')
                {
                    int pos = argument.IndexOf('=');
                    if (pos == -1)
                    {
                        this.namedArguments[argument.Substring(1)] = "1";
                    }
                    else
                    {
                        this.namedArguments[argument.Substring(1, pos - 1)] =
                            argument.Substring(pos + 1);
                    }

                    continue;
                }
                else
                {
                    this.unnamedArguments.Add(argument);
                }
            }
        }

        /// <summary>
        /// Gets a list of unnamed arguments
        /// </summary>
        public List<string> UnnamedArguments
        {
            get { return this.unnamedArguments; }
        }

        /// <summary>
        /// Gets a dictionary of named arguments
        /// </summary>
        public NiceDictionary<string, string> NamedArguments
        {
            get { return this.namedArguments; }
        }

        /// <summary>
        /// Executes the after parsing test
        /// </summary>
        /// <param name="definition">Test to be executed</param>
        public void Check(ICommandLineDefinition definition)
        {
            definition.AfterParsing(this);
        }
    }

    /// <summary>
    /// Wrong writing
    /// </summary>
    [Obsolete("Use CommandLineEvaluator")]
    public class CommandLineEvaluater : CommandLineEvaluator
    {
        /// <summary>
        /// Initializes a new instance of the CommandLineEvaluater class.
        /// </summary>
        /// <param name="arguments">List of program arguments</param>
        public CommandLineEvaluater(string[] arguments) :
            base(arguments)
        {
        }
    }
}
