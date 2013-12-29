﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.CommandLine
{
    /// <summary>
    /// Indicates that an argument is required. If argument is not given, an exception will be thrown
    /// </summary>
    public class RequiresArgument : ICommandLineDefinition
    {
        private string name;
        private string message;

        /// <summary>
        /// Initializes a new instance of the RequiresArgument class.
        /// </summary>
        /// <param name="name">Name of the argument to be required</param>
        public RequiresArgument(string name)
            : this(name, name + " is missing")
        {
        }

        /// <summary>
        /// Initializes a new instance of the RequiresArgument class.
        /// </summary>
        /// <param name="name">Name of the argument to be required</param>
        /// <param name="message">Message to be send out</param>
        public RequiresArgument(string name, string message)
        {
            this.name = name;
            this.message = message;
        }
        void ICommandLineDefinition.BeforeParsing(CommandLineEvaluator evaluator)
        {
        }

        void ICommandLineDefinition.AfterParsing(CommandLineEvaluator evaluator)
        {
            if (evaluator.NamedArguments[this.name] == null)
            {
                throw new ArgumentException(message);
            }
        }
    }
}