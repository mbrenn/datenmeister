//-----------------------------------------------------------------------
// <copyright file="ExpressionParser.cs" company="Martin Brenn">
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
    using BurnSystems.Interfaces;
    using BurnSystems.Parser.Helper;

    /// <summary>
    /// Expression parser, which evaluates simple expressions like '23+name'
    /// </summary>
    public class ExpressionParser
    {
        /// <summary>
        /// Priority value of brackets
        /// </summary>
        private const int BracketPriority = 20;

        /// <summary>
        /// Dictionary of operators
        /// </summary>
        private static Dictionary<string, Operator> operatorTable =
            new Dictionary<string, Operator>();

        /// <summary>
        /// Templateparser, which is used for the expressionparser
        /// </summary>
        private TemplateParser core;

        /// <summary> 
        /// Current position of the cursor
        /// </summary>
        private int currentPosition;

        /// <summary>
        /// Current operator priority
        /// </summary>
        private int currentOperatorPriority;

        /// <summary>
        /// Operator stack
        /// </summary>
        private Stack<OperatorStructure> operatorStack = new Stack<OperatorStructure>();

        /// <summary>
        /// Expression statck
        /// </summary>
        private Stack<ExpressionStructure> expressionStack = new Stack<ExpressionStructure>();

        /// <summary>
        /// Flag, if debug is active
        /// </summary>
        private bool debugActive;

        #region Static variables and methods

        /// <summary>
        /// Initializes static members of the ExpressionParser class.
        /// </summary>
        static ExpressionParser()
        {
            operatorTable["+"] = Operator.Addition;
            operatorTable["-"] = Operator.Subtraction;
            operatorTable["*"] = Operator.Multiplication;
            operatorTable["/"] = Operator.Division;
            operatorTable["."] = Operator.Concatenation;
            operatorTable["&"] = Operator.And;
            operatorTable["|"] = Operator.Or;
            operatorTable["^"] = Operator.Xor;
            operatorTable["&&"] = Operator.LogicalAnd;
            operatorTable["||"] = Operator.LogicalOr;
            operatorTable["^^"] = Operator.LogicalXor;
            operatorTable["!"] = Operator.LogicalNot;
            operatorTable["->"] = Operator.Dereference;
            operatorTable["=="] = Operator.Equal;
            operatorTable["!="] = Operator.Inequal;
            operatorTable[">"] = Operator.Greater;
            operatorTable[">="] = Operator.GreaterOrEqual;
            operatorTable["<"] = Operator.Less;
            operatorTable["<="] = Operator.LessOrEqual;
            operatorTable["!="] = Operator.Inequal;
            operatorTable["<>"] = Operator.Inequal;
            operatorTable["$"] = Operator.Indirect;
        }

        /// <summary>
        /// Initializes a new instance of the ExpressionParser class.
        /// </summary>
        /// <param name="core">Templateparser to be used</param>
        public ExpressionParser(TemplateParser core)
        {
            this.core = core;
        }

        /// <summary>
        /// Operators of the expressionparser
        /// </summary>
        private enum Operator
        {
            /// <summary>
            /// Unknown operator
            /// </summary>
            Unknown,

            /// <summary>
            /// Addition operator
            /// </summary>
            Addition,

            /// <summary>
            /// Subtraction Operator
            /// </summary>
            Subtraction,

            /// <summary>
            /// Multiplication operator
            /// </summary>
            Multiplication,

            /// <summary>
            /// Division operator
            /// </summary>
            Division,
            
            /// <summary>
            /// Concatenation operator
            /// </summary>
            Concatenation,

            /// <summary>
            /// Logical-And operator
            /// </summary>
            LogicalAnd,

            /// <summary>
            /// Logical-Or operator
            /// </summary>
            LogicalOr,

            /// <summary>
            /// Logical-Xor operator
            /// </summary>
            LogicalXor,

            /// <summary>
            /// Logical-Not operator
            /// </summary>
            LogicalNot,

            /// <summary>
            /// And operator
            /// </summary>
            And,

            /// <summary>
            /// Or operator
            /// </summary>
            Or,

            /// <summary>
            /// Xor operator
            /// </summary>
            Xor,

            /// <summary>
            /// Dereference operator
            /// </summary>
            Dereference,

            /// <summary>
            /// Equal operator
            /// </summary>
            Equal,

            /// <summary>
            /// Greater operator
            /// </summary>
            Greater,

            /// <summary>
            /// Greater or equal operator
            /// </summary>
            GreaterOrEqual,

            /// <summary>
            /// Less operator
            /// </summary>
            Less,

            /// <summary>
            /// Less or equal operator
            /// </summary>
            LessOrEqual,

            /// <summary>
            /// Inequal operator
            /// </summary>
            Inequal,

            /// <summary>
            /// Indirect operator, which means that the contents on 
            /// stack are reevaluated
            /// </summary>
            Indirect
        }

        /// <summary>
        /// Type of the literal, which is stored in the structure
        /// </summary>
        private enum LiteralType
        {
            /// <summary>
            /// Unknown literal type
            /// </summary>
            Unknown,

            /// <summary>
            /// Data stored is a booleam
            /// </summary>
            Boolean,

            /// <summary>
            /// Data stored is an integer
            /// </summary>
            Integer,

            /// <summary>
            /// Data stored is a string
            /// </summary>
            String,

            /// <summary>
            /// Data stored is an object
            /// </summary>
            Object,

            /// <summary>
            /// Data stored is a variable
            /// </summary>
            Variable
        }

        /// <summary>
        /// Current parse mode
        /// </summary>
        private enum ParseMode
        {
            /// <summary>
            /// Current text is an expression
            /// </summary>
            Expression,

            /// <summary>
            /// Current text is an operator
            /// </summary>
            Operator,

            /// <summary>
            /// Opens bracket
            /// </summary>
            BracketOpen,

            /// <summary>
            /// Closes bracket
            /// </summary>
            BracketClose
        }

        #endregion

        /// <summary>
        /// Gets or sets a value indicating whether debug information should be shown
        /// </summary>
        public bool Debug
        {
            get { return this.debugActive; }
            set { this.debugActive = value; }
        }

        /// <summary>
        /// Converts an object to a boolean
        /// </summary>
        /// <param name="value">Object to be converted</param>
        /// <returns>true, if oObject is true or != 0 and != "" and != null</returns>
        public static bool ConvertToBoolean(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool)
            {
                return (bool)value;
            }

            if (value is int)
            {
                return ((int)value) != 0;
            }

            string valueAsString = value as string;
            if (valueAsString != null)
            {
                return !String.IsNullOrEmpty(valueAsString)
                    && valueAsString != Boolean.FalseString;
            }

            return true;
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="oA">First object</param>
        /// <param name="oB">Second object</param>
        /// <returns>-1 if oA is smaller than oB, 0 if they are equal, otherwise 1</returns>
        public static int CompareObjects(object oA, object oB)
        {
            if ((oA == null || (oA as string) == string.Empty) &&
                (oB == null || (oB as string) == string.Empty))
            {
                return 0;
            }

            if (oA == null)
            {
                return 1;
            }

            if (oB == null)
            {
                return -1;
            }

            // Konvertiere Int32 zu Int64
            if (oA is int || oA is short)
            {
                oA = Convert.ToInt64(oA, CultureInfo.CurrentUICulture);
            }

            if (oB is int || oB is short)
            {
                oB = Convert.ToInt64(oB, CultureInfo.CurrentUICulture);
            }

            if (oA is short || oA is double)
            {
                oA = Convert.ToDouble(oA, CultureInfo.CurrentUICulture);
                oB = Convert.ToDouble(oB, CultureInfo.CurrentUICulture);
            }

            if (oB is short || oB is double)
            {
                oA = Convert.ToDouble(oA, CultureInfo.CurrentUICulture);
                oB = Convert.ToDouble(oB, CultureInfo.CurrentUICulture);
            }

            string strA = oA as string;
            string strB = oB as string;

            if (strA != null)
            {
                if (strB != null)
                {
                    return string.Compare(strA, strB, StringComparison.CurrentCulture);
                }

                if (oB is int)
                {
                    return string.Compare(strA, oB.ToString(), StringComparison.CurrentCulture);
                }

                return string.Compare(strA, oB.ToString(), StringComparison.CurrentCulture);
            }

            if (strB != null)
            {
                if (strA != null)
                {
                    return string.Compare(strB, strA, StringComparison.CurrentCulture);
                }

                if (oA is int)
                {
                    return string.Compare(strB, oA.ToString(), StringComparison.CurrentCulture);
                }

                return string.Compare(strB, oA.ToString(), StringComparison.CurrentCulture);
            }

            if (oA is long)
            {
                long nA = (long)oA;
                if (strB != null)
                {
                    strA = nA.ToString(CultureInfo.CurrentUICulture);
                    return string.Compare(strA, strB, StringComparison.CurrentCulture);
                }

                if (oB is long)
                {
                    long nB = (long)oB;
                    return nA.CompareTo(nB);
                }

                return nA.CompareTo(oB);
            }

            if (oB is long)
            {
                long nB = (long)oB;
                if (strA != null)
                {
                    strB = nB.ToString(CultureInfo.CurrentUICulture);
                    return string.Compare(strB, strA, StringComparison.CurrentCulture);
                }

                if (oA is long)
                {
                    long nA = (long)oA;
                    return nB.CompareTo(nA);
                }

                return nB.CompareTo(oA);
            }

            if (oA is double)
            {
                double dA = (double)oA;
                double dB = (double)oB;

                return dA.CompareTo(dB);
            }

            IComparable compA = oA as IComparable;
            if (compA != null)
            {
                return compA.CompareTo(oB);
            }

            return -1;
        }

        /// <summary>
        /// Parses an expression and returns a boolean value
        /// </summary>
        /// <param name="expression">Expression to be parsed</param>
        /// <returns>Parsed variable as boolean</returns>
        public bool ParseForBoolean(string expression)
        {
            object result = this.Parse(expression);
            return ConvertToBoolean(result);
        }

        /// <summary>
        /// Parses an expression and returns a string
        /// </summary>
        /// <param name="expression">Variable to parsed</param>
        /// <returns>Parsed variable as string</returns>
        public string ParseForString(string expression)
        {
            object result = this.Parse(expression);
            string resultAsString = result as string;
            if (resultAsString != null)
            {
                return resultAsString;
            }

            return string.Format(CultureInfo.CurrentUICulture, "{0}", result);
        }

        /// <summary>
        /// Parses an expression
        /// </summary>
        /// <param name="expression">Expression to be parsed</param>
        /// <returns>Result of Expression</returns>
        public object Parse(string expression)
        {
            try
            {
                // Check for empty string
                if (string.IsNullOrEmpty(expression))
                {
                    return string.Empty;
                }

                // Initialize
                int length = expression.Length;
                var parseMode = GetParseModeByCharacter(expression[0]);
                bool onlyDigits = true;
                bool loop = true;
                int expressionLength = -1;
                this.expressionStack = new Stack<ExpressionStructure>();
                this.operatorStack = new Stack<OperatorStructure>();
                this.currentOperatorPriority = 0;
                bool quoteMode = false;
                this.currentPosition = -1;

                // Start loop
                while (loop)
                {
                    // Parses current value till special character
                    this.currentPosition++;
                    expressionLength++;
                    char currentChar;

                    bool endExpression = false;

                    if (this.currentPosition == length)
                    {
                        // End of string
                        endExpression = true;
                        loop = false;
                        currentChar = ' ';
                    }
                    else
                    {
                        currentChar = expression[this.currentPosition];
                        if (currentChar == ')')
                        {
                            endExpression = true;
                        }
                        else if (currentChar == '(')
                        {
                            endExpression = true;
                        }
                        else
                        {
                            if (currentChar == '"')
                            {
                                quoteMode = !quoteMode;
                            }

                            switch (parseMode)
                            {
                                case ParseMode.Expression:
                                    if (!Char.IsDigit(currentChar)
                                        && !Char.IsLetter(currentChar)
                                        && !Char.IsWhiteSpace(currentChar)
                                        && currentChar != '_'
                                        && currentChar != '"')
                                    {
                                        // No digit, no letter, no quotes, must be an operator
                                        if (!quoteMode)
                                        {
                                            endExpression = true;
                                        }
                                    }
                                    else if (!Char.IsDigit(currentChar))
                                    {
                                        onlyDigits = false;
                                    }

                                    break;
                                case ParseMode.Operator:
                                    if (Char.IsLetterOrDigit(currentChar) || currentChar == '"' || currentChar == '_')
                                    {
                                        // Operator
                                        endExpression = true;

                                        onlyDigits = Char.IsDigit(currentChar);
                                    }

                                    break;
                                case ParseMode.BracketOpen:
                                    // Ignore
                                    endExpression = true;
                                    break;
                                case ParseMode.BracketClose:
                                    // Ignore
                                    endExpression = true;
                                    break;
                            }
                        }
                    }

                    // If end of current expression/operator, evaluate it
                    if (endExpression)
                    {
                        if (expressionLength == 0)
                        {
                            parseMode = ParseMode.Expression;
                        }
                        else
                        {
                            string temp = expression.Substring(
                                this.currentPosition - expressionLength, 
                                expressionLength);

                            // Executes mode
                            switch (parseMode)
                            {
                                case ParseMode.Expression:
                                    this.EvaluateLiteral(temp, onlyDigits);
                                    if (onlyDigits)
                                    {
                                        this.WriteDebug("Number: " + temp);
                                    }
                                    else
                                    {
                                        this.WriteDebug("Expression: " + temp);
                                    }

                                    parseMode = ParseMode.Operator;
                                    break;
                                case ParseMode.Operator:
                                    this.EvaluateOperator(temp);
                                    this.WriteDebug("Operator:" + temp);
                                    parseMode = ParseMode.Expression;
                                    break;
                                case ParseMode.BracketClose:
                                    this.WriteDebug(")");
                                    parseMode = ParseMode.Operator;
                                    break;
                                case ParseMode.BracketOpen:
                                    this.WriteDebug("(");
                                    parseMode = GetParseModeByCharacter(currentChar);
                                    break;
                            }
                        }

                        // Brackets increases priority of operator, so they are executed before
                        // the operators around the bracket
                        if (currentChar == ')')
                        {
                            parseMode = ParseMode.BracketClose;
                            this.currentOperatorPriority -= ExpressionParser.BracketPriority;
                        }

                        if (currentChar == '(')
                        {
                            if (parseMode == ParseMode.Expression)
                            {
                                // This bracket is for setting priorities
                                parseMode = ParseMode.BracketOpen;
                                this.currentOperatorPriority += BracketPriority;
                            }
                            else
                            {
                                // This bracket is for a function
                                this.EvaluateFunction(expression);

                                // Checks, if function is last operator
                                if (this.currentPosition >= length)
                                {
                                    break;
                                }

                                // Kleiner Sonderfall, der überprüft, ob sich die 
                                // Klammern schlieüen
                                if (expression[this.currentPosition] == ')')
                                {
                                    this.currentOperatorPriority -= BracketPriority;
                                    parseMode = ParseMode.BracketClose;
                                }
                            }
                        }

                        expressionLength = 0;
                    }
                }

                while (this.operatorStack.Count > 0)
                {
                    var operatorObject = this.operatorStack.Pop();
                    this.ExecuteOperator(operatorObject.Operator);
                }

                return this.PopObject();
            }
            catch (Exception exc)
            {
                throw new ParserException(
                    string.Format(
                        CultureInfo.CurrentUICulture,
                        Localization_Parser.ExpressionParser_Exception,
                        expression,
                        exc.Message),
                    exc);
            }
        }

        /// <summary>
        /// Pops one integer from expressionstack
        /// </summary>
        /// <returns>The object on stack as Integer</returns>
        public int PopInteger()
        {
            object objectOnStack = this.PopObject();
            if (objectOnStack is int)
            {
                return (int)objectOnStack;
            }

            if (objectOnStack is IConvertible)
            {
                return Convert.ToInt32(objectOnStack, CultureInfo.CurrentUICulture);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Pops one double from expressionstack
        /// </summary>
        /// <returns>The object on stack as Double</returns>
        public double PopDouble()
        {
            object objectOnStack = this.PopObject();
            if (objectOnStack is double)
            {
                return (double)objectOnStack;
            }

            if (objectOnStack is IConvertible)
            {
                return Convert.ToDouble(objectOnStack, CultureInfo.CurrentUICulture);
            }

            throw new InvalidCastException();
        }

        /// <summary>
        /// Pops string from expressionstack
        /// </summary>
        /// <returns>The object on stack as String</returns>
        public string PopString()
        {
            object objectOnStack = this.PopObject();
            string objectOnStackAsString = objectOnStack as string;
            if (objectOnStackAsString != null)
            {
                return objectOnStackAsString;
            }

            return objectOnStack.ToString();
        }

        /// <summary>
        /// Pops a boolean
        /// </summary>
        /// <returns>The object on stack as boolean</returns>
        public bool PopBoolean()
        {
            object objectOnStack = this.PopObject();
            return ConvertToBoolean(objectOnStack);
        }

        /// <summary>
        /// Gets operator by string
        /// </summary>
        /// <param name="operatorName">Operator as string</param>
        /// <returns>Operator or Operator.Unknown of <c>strOperator</c> is invalid</returns>
        private static Operator GetOperator(string operatorName)
        {
            Operator result;

            if (operatorTable.TryGetValue(operatorName, out result))
            {
                return result;
            }

            return Operator.Unknown;
        }

        /// <summary>
        /// Executes a method on an object
        /// </summary>
        /// <param name="instance">Instance of object, whose method should be executed</param>
        /// <param name="functionName">Name of the function</param>
        /// <param name="parameters">Parameters for the method</param>
        /// <returns>Result of the execution</returns>
        private static object ExecuteMethod(object instance, string functionName, List<object> parameters)
        {
            if (instance == null)
            {
                return null;
            }

            if (instance is TimeSpan)
            {
                instance = new TimeSpanHelper((TimeSpan)instance);
            }
            else if (instance is DateTime)
            {
                instance = new DateTimeHelper((DateTime)instance);
            }
            else if (instance is long)
            {
                instance = new LongHelper((long)instance);
            }
            else if (instance is double)
            {
                instance = new DoubleHelper((double)instance);
            }
            else
            {
                string strInstance = instance as string;
                if (strInstance != null)
                {
                    instance = new StringHelper(strInstance);
                }
            }

            var parserInstance = instance as IParserObject;

            if (parserInstance != null)
            {
                return parserInstance.ExecuteFunction(functionName, parameters);
            }
            else
            {
                // Yeah, Reflection fun
                var types = new Type[parameters.Count];

                int n = 0;
                foreach (object parameter in parameters)
                {
                    types[n] = parameter.GetType();
                    n++;
                }

                var methodInfo = instance.GetType().GetMethod(functionName, types);

                if (methodInfo == null)
                {
                    return string.Empty;
                }
                else
                {
                    return methodInfo.Invoke(instance, parameters.ToArray());
                }
            }
        }

        /// <summary>
        /// Gets operator priority of a certain operator. 
        /// </summary>
        /// <param name="operatorType">Type of the operator</param>
        /// <returns>Priority of the operator</returns>
        private static int GetOperatorPriority(Operator operatorType)
        {
            switch (operatorType)
            {
                case Operator.Indirect:
                    return 0;
                case Operator.Equal:
                    return 1;
                case Operator.Less:
                    return 1;
                case Operator.LessOrEqual:
                    return 1;
                case Operator.Greater:
                    return 1;
                case Operator.GreaterOrEqual:
                    return 1;
                case Operator.Inequal:
                    return 1;
                case Operator.LogicalAnd:
                    return 2;
                case Operator.LogicalOr:
                    return 2;
                case Operator.LogicalXor:
                    return 2;
                case Operator.And:
                    return 3;
                case Operator.Or:
                    return 3;
                case Operator.Xor:
                    return 3;
                case Operator.LogicalNot:
                    return 4;
                case Operator.Concatenation:
                    return 5;
                case Operator.Addition:
                    return 6;
                case Operator.Subtraction:
                    return 6;
                case Operator.Multiplication:
                    return 7;
                case Operator.Division:
                    return 7;
                case Operator.Dereference:
                    return 8;
            }

            return 0;
        }

        /// <summary>
        /// Checks, if the operator is a left priority operator
        /// </summary>
        /// <param name="operatorType">Type of operator</param>
        /// <returns>true, if this is a leftoperator type</returns>
        private static bool IsLeftPriority(Operator operatorType)
        {
            if (operatorType == Operator.LogicalNot)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the parse mode of the next characters by the first 
        /// character of an unknown string
        /// </summary>
        /// <param name="firstChar">First Character</param>
        /// <returns>Matching parse mode</returns>
        private static ParseMode GetParseModeByCharacter(char firstChar)
        {
            var parseMode =
                Char.IsLetterOrDigit(firstChar)
                    || firstChar == '"'
                    || firstChar == '(' 
                    || firstChar == '_' ?
                    ParseMode.Expression : ParseMode.Operator;
            return parseMode;
        }

        /// <summary>
        /// Evaluates operator
        /// </summary>
        /// <param name="nameOperator">Name of the operator</param>
        private void EvaluateOperator(string nameOperator)
        {
            var operatorType = GetOperator(nameOperator);
            int priority = ExpressionParser.GetOperatorPriority(operatorType) + this.currentOperatorPriority;
            bool leftPriority = IsLeftPriority(operatorType);

            // Checks, if current operator has higher priority than operator on stack
            while (this.operatorStack.Count > 0)
            {
                OperatorStructure structure = this.operatorStack.Peek();

                if ((priority == structure.Priority && leftPriority) ||
                    (priority < structure.Priority))
                {
                    // Execute current operator
                    this.operatorStack.Pop();

                    this.ExecuteOperator(structure.Operator);
                }
                else
                {
                    break;
                }
            }

            this.operatorStack.Push(new OperatorStructure(operatorType, priority));
        }

        /// <summary>
        /// Executes operator
        /// </summary>
        /// <param name="operatorType">Operator to be executed</param>
        private void ExecuteOperator(Operator operatorType)
        {
            this.WriteDebug("Execute Operator: " + operatorType.ToString());
            switch (operatorType)
            {
                case Operator.Addition:
                    this.ExecuteAddition();
                    break;
                case Operator.Subtraction:
                    this.ExecuteSubtraction();
                    break;
                case Operator.Multiplication:
                    this.ExecuteMultiplication();
                    break;
                case Operator.Division:
                    this.ExecuteDivision();
                    break;
                case Operator.Concatenation:
                    this.ExecuteConcatenation();
                    break;
                case Operator.LogicalAnd:
                    this.ExecuteLogicalAnd();
                    break;
                case Operator.LogicalOr:
                    this.ExecuteLogicalOr();
                    break;
                case Operator.LogicalXor:
                    this.ExecuteLogicalXor();
                    break;
                case Operator.LogicalNot:
                    this.ExecuteLogicalNot();
                    break;
                case Operator.And:
                    this.ExecuteAnd();
                    break;
                case Operator.Or:
                    this.ExecuteOr();
                    break;
                case Operator.Xor:
                    this.ExecuteXor();
                    break;
                case Operator.Dereference:
                    this.ExecuteDereference();
                    break;
                case Operator.Equal:
                    this.ExecuteEqual();
                    break;
                case Operator.Greater:
                    this.ExecuteGreater();
                    break;
                case Operator.GreaterOrEqual:
                    this.ExecuteGreaterOrEqual();
                    break;
                case Operator.Less:
                    this.ExecuteLess();
                    break;
                case Operator.LessOrEqual:
                    this.ExecuteLessOrEqual();
                    break;
                case Operator.Inequal:
                    this.ExecuteInequal();
                    break;
                case Operator.Indirect:
                    this.ExecuteIndirect();
                    break;
            }
        }

        /// <summary>
        /// Executes the indirect function
        /// </summary>
        private void ExecuteIndirect()
        {
            var expression = this.PopString();
            this.WriteDebug("Indirect: " + expression);

            var parser = new ExpressionParser(this.core);
            var result = parser.Parse(expression);

            var structure = new ExpressionStructure(result, LiteralType.String);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes an inequal function
        /// </summary>
        private void ExecuteInequal()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) != 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the less or equal function
        /// </summary>
        private void ExecuteLessOrEqual()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) <= 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the less function
        /// </summary>
        private void ExecuteLess()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) < 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the greater or equal function
        /// </summary>
        private void ExecuteGreaterOrEqual()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) >= 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the greater function
        /// </summary>
        private void ExecuteGreater()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) > 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the equal function
        /// </summary>
        private void ExecuteEqual()
        {
            object oA = this.PopObject();
            object oB = this.PopObject();

            bool equal = CompareObjects(oB, oA) == 0;
            var structure = new ExpressionStructure(equal, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Makes a dereference
        /// </summary>
        private void ExecuteDereference()
        {
            var structure = this.PopExpressionStructure();
            string method = (string)structure.Literal;

            var objectOnStack = this.PopObject();
            var list = objectOnStack as IList;

            if (objectOnStack == null)
            {
                this.expressionStack.Push(
                    new ExpressionStructure(
                        null, LiteralType.Object));
                return;
            }
            else if (objectOnStack is int)
            {
                // Creates int helper object
            }
            else if (objectOnStack is double)
            {
                objectOnStack = new DoubleHelper((double)objectOnStack);
            }
            else if (objectOnStack is long)
            {
                objectOnStack = new LongHelper((long)objectOnStack);
            }
            else if (objectOnStack is TimeSpan)
            {
                objectOnStack = new TimeSpanHelper((TimeSpan)objectOnStack);
            }
            else if (objectOnStack is DateTime)
            {
                objectOnStack = new DateTimeHelper((DateTime)objectOnStack);
            }
            else if (list != null)
            {
                objectOnStack = new IListHelper(list);
            }
            else
            {
                string strObject = objectOnStack as string;
                if (strObject != null)
                {
                    objectOnStack = new StringHelper(strObject);
                }
            }

            IParserObject parserObject = objectOnStack as IParserObject;
            IDictionary dictionary = objectOnStack as IDictionary;
            IParserDictionary parserDictionary = objectOnStack as IParserDictionary;
            if (parserObject != null)
            {
                this.expressionStack.Push(
                    new ExpressionStructure(
                        parserObject.GetProperty(method), LiteralType.Object));
            }
            else if (dictionary != null)
            {
                this.expressionStack.Push(
                    new ExpressionStructure(
                        dictionary[method],
                        LiteralType.Object));
            }
            else if (parserDictionary != null)
            {
                this.expressionStack.Push(
                    new ExpressionStructure(
                        parserDictionary[method],
                        LiteralType.Object));
            }
            else
            {
                // Do reflections
                var type = objectOnStack.GetType();
                var property = type.GetProperty(method);
                object result = string.Empty;

                if (property != null)
                {
                    result = property.GetValue(objectOnStack, null);
                }

                this.expressionStack.Push(
                    new ExpressionStructure(
                        result, LiteralType.Object));
            }
        }

        /// <summary>
        /// Evaluates the function
        /// </summary>
        /// <param name="expression">Complete Expression string</param>
        private void EvaluateFunction(string expression)
        {
            char currentChar;

            // Look for closing bracket and set all parameter
            List<object> parameters = new List<object>();

            // Open brackets
            int numberOfOpenBrackets = 1;

            // Flag, if currently in quote
            bool quoteMode = false;
            bool escapeMode = false;
            int parameterLength = 0;
            this.currentPosition++;

            while (true)
            {
                currentChar = expression[this.currentPosition];
                this.currentPosition++;

                if (escapeMode)
                {
                    escapeMode = false;
                    continue;
                }
                else if (currentChar == '\\')
                {
                    escapeMode = true;
                }
                else if (currentChar == '"')
                {
                    quoteMode = !quoteMode;
                    parameterLength++;
                }
                else if (currentChar == '(' && !quoteMode)
                {
                    numberOfOpenBrackets++;
                }
                else if (currentChar == ')' && !quoteMode)
                {
                    numberOfOpenBrackets--;
                }
                else if (currentChar == ',' && !quoteMode)
                {
                    var strParameter =
                        expression.Substring(
                        this.currentPosition - parameterLength - 1,
                        parameterLength);

                    var expressionParser = new ExpressionParser(this.core);
                    parameters.Add(expressionParser.Parse(strParameter));

                    parameterLength = 0;
                }
                else
                {
                    parameterLength++;
                }

                if (numberOfOpenBrackets == 0)
                {
                    if (parameterLength != 0)
                    {
                        var parameter =
                            expression.Substring(
                            this.currentPosition - parameterLength - 1,
                            parameterLength);

                        var expressionParser = new ExpressionParser(this.core);
                        parameters.Add(expressionParser.Parse(parameter));
                    }

                    // Execute function
                    var structure = this.expressionStack.Pop();
                    if (structure.ExpressionType == LiteralType.Variable)
                    {
                        // Check, if last operator is a dereference pointer
                        var strFunctionName = (string)structure.Literal;

                        if (this.operatorStack.Count != 0 &&
                            this.operatorStack.Peek().Operator == Operator.Dereference)
                        {
                            // Ok, have fun, execute function on this object
                            this.operatorStack.Pop();

                            object instance = this.PopObject();

                            object result = ExecuteMethod(instance, strFunctionName, parameters);

                            var newObject = new ExpressionStructure(
                                result,
                                LiteralType.Object);
                            this.expressionStack.Push(newObject);
                        }
                        else
                        {
                            // Variable, execute function
                            object result = TemplateParser.ExecuteFunction(strFunctionName, parameters);

                            var newObject = new ExpressionStructure(
                                result,
                                LiteralType.Object);
                            this.expressionStack.Push(newObject);
                        }
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Writes a debug message
        /// </summary>
        /// <param name="text">Text, which should be shown</param>
        private void WriteDebug(string text)
        {
            if (this.debugActive)
            {
                Console.WriteLine(text);
            }
        }

        /// <summary>
        /// Evaluates the expression and stores it on the stack if necessary
        /// </summary>
        /// <param name="literal">string to be converted to literal</param>
        /// <param name="onlyDigits">Flag, if string contains only digits</param>
        private void EvaluateLiteral(string literal, bool onlyDigits)
        {
            object result;
            LiteralType literalType = LiteralType.Unknown;

            if (string.IsNullOrEmpty(literal))
            {
                result = string.Empty;
            }
            else if (onlyDigits)
            {
                result = Convert.ToInt32(literal, CultureInfo.CurrentUICulture);
                literalType = LiteralType.Integer;
            }
            else
            {
                if (literal[0] == '"' && (literal[literal.Length - 1] == '"'))
                {
                    result = literal.Substring(1, literal.Length - 2);
                    literalType = LiteralType.String;
                }
                else if (literal == "false")
                {
                    result = false;
                    literalType = LiteralType.Boolean;
                }
                else if (literal == "true")
                {
                    result = true;
                    literalType = LiteralType.Boolean;
                }
                else if (literal == "null")
                {
                    result = null;
                    literalType = LiteralType.Object;
                }
                else if (literal == "Now")
                {
                    result = DateTime.Now;
                    literalType = LiteralType.Object;
                }
                else
                {
                    // Evaluate Variable
                    result = literal;
                    literalType = LiteralType.Variable;
                }
            }

            if (result != null)
            {
                this.WriteDebug(result.ToString() + " " + result.GetType().Name);
            }
            else
            {
                this.WriteDebug("null");
            }

            this.expressionStack.Push(new ExpressionStructure(result, literalType));
        }

        /// <summary>
        /// Pops an object
        /// </summary>
        /// <returns>Object on stack</returns>
        private object PopObject()
        {
            var structure = this.expressionStack.Pop();
            object objectOnStack = structure.Literal;

            if (structure.ExpressionType == LiteralType.Variable)
            {
                string variableName = (string)objectOnStack;

                // Resolves variable
                return this.core.GetValue(variableName);
            }

            return objectOnStack;
        }

        /// <summary>
        /// Pops an object without variable replacement
        /// </summary>
        /// <returns>Expression to be popped</returns>
        private ExpressionStructure PopExpressionStructure()
        {
            return this.expressionStack.Pop();
        }

        /// <summary>
        /// Executes the logical not function
        /// </summary>
        private void ExecuteLogicalNot()
        {
            bool nA = this.PopBoolean();

            var structure = new ExpressionStructure(!nA, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the xor function
        /// </summary>
        private void ExecuteXor()
        {
            int nA = this.PopInteger();
            int nB = this.PopInteger();

            var structure = new ExpressionStructure(nA ^ nB, LiteralType.Integer);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the or function
        /// </summary>
        private void ExecuteOr()
        {
            int nA = this.PopInteger();
            int nB = this.PopInteger();

            var structure = new ExpressionStructure(nA | nB, LiteralType.Integer);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the and function
        /// </summary>
        private void ExecuteAnd()
        {
            int nA = this.PopInteger();
            int nB = this.PopInteger();

            var structure = new ExpressionStructure(nA & nB, LiteralType.Integer);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the logical-xor function
        /// </summary>
        private void ExecuteLogicalXor()
        {
            bool nA = this.PopBoolean();
            bool nB = this.PopBoolean();

            var structure = new ExpressionStructure(nA ^ nB, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the logical-or function
        /// </summary>
        private void ExecuteLogicalOr()
        {
            bool nA = this.PopBoolean();
            bool nB = this.PopBoolean();

            var structure = new ExpressionStructure(nA || nB, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes the logical-and function
        /// </summary>
        private void ExecuteLogicalAnd()
        {
            bool nA = this.PopBoolean();
            bool nB = this.PopBoolean();

            var structure = new ExpressionStructure(nA && nB, LiteralType.Boolean);
            this.expressionStack.Push(structure);
        }

        /// <summary>
        /// Executes addition
        /// </summary>
        private void ExecuteAddition()
        {
            var a = this.PopObject();
            var b = this.PopObject();

            if (a is double || b is double)
            {
                var da = Convert.ToDouble(a);
                var db = Convert.ToDouble(b);
                var structure = new ExpressionStructure(da + db, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
            else
            {
                var na = Convert.ToInt64(a);
                var nb = Convert.ToInt64(b);
                var structure = new ExpressionStructure(na + nb, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
        }

        /// <summary>
        /// Executes Subtraction
        /// </summary>
        private void ExecuteSubtraction()
        {
            var a = this.PopObject();
            var b = this.PopObject();

            if (a is double || b is double)
            {
                var da = Convert.ToDouble(a);
                var db = Convert.ToDouble(b);
                var structure = new ExpressionStructure(db - da, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
            else
            {
                var na = Convert.ToInt64(a);
                var nb = Convert.ToInt64(b);
                var structure = new ExpressionStructure(nb - na, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
        }

        /// <summary>
        /// Executes multiplication
        /// </summary>
        private void ExecuteMultiplication()
        {
            var a = this.PopObject();
            var b = this.PopObject();

            if (a is double || b is double)
            {
                var da = Convert.ToDouble(a);
                var db = Convert.ToDouble(b);
                var structure = new ExpressionStructure(da * db, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
            else
            {
                var na = Convert.ToInt64(a);
                var nb = Convert.ToInt64(b);
                var structure = new ExpressionStructure(na * nb, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
        }

        /// <summary>
        /// Executes division
        /// </summary>
        private void ExecuteDivision()
        {
            var a = this.PopObject();
            var b = this.PopObject();

            if (a is double || b is double)
            {
                var da = Convert.ToDouble(a);
                var db = Convert.ToDouble(b);
                var structure = new ExpressionStructure(db / da, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
            else
            {
                var na = Convert.ToInt64(a);
                var nb = Convert.ToInt64(b);
                var structure = new ExpressionStructure(nb / na, LiteralType.Integer);
                this.expressionStack.Push(structure);
            }
        }

        /// <summary>
        /// Executes concatenation
        /// </summary>
        private void ExecuteConcatenation()
        {
            string nA = this.PopString();
            string nB = this.PopString();

            var structure = new ExpressionStructure(nB + nA, LiteralType.String);
            this.expressionStack.Push(structure);
        }
        
        /// <summary>
        /// Structure for expressions
        /// </summary>
        private class ExpressionStructure
        {
            /// <summary>
            /// Initializes a new instance of the ExpressionStructure class.
            /// </summary>
            /// <param name="literal">Literal for the type</param>
            /// <param name="type">Type for the type</param>
            public ExpressionStructure(object literal, LiteralType type)
            {
                this.Literal = literal;
                this.ExpressionType = type;
            }

            /// <summary>
            /// Gets or sets the literal in the structure
            /// </summary>
            public object Literal
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the current expressiontype
            /// </summary>
            public LiteralType ExpressionType
            {
                get;
                set;
            }
        }

        /// <summary>
        /// Operator structure
        /// </summary>
        private class OperatorStructure
        {
            /// <summary>
            /// Initializes a new instance of the OperatorStructure class.
            /// </summary>
            /// <param name="operatorStructure">Operator for the current structure</param>
            /// <param name="priority">Priority for the operator</param>
            public OperatorStructure(Operator operatorStructure, int priority)
            {
                this.Operator = operatorStructure;
                this.Priority = priority;
            }

            /// <summary>
            /// Gets or sets the operator of the current structure
            /// </summary>
            public Operator Operator
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the value of the property
            /// </summary>
            public int Priority
            {
                get;
                set;
            }
        }
    }
}