using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.Logic.SourceFactory
{
    /// <summary>
    /// Contains some helper methods which can be used
    /// to build Javascript or Typescript files
    /// </summary>
    public static class TypeScriptBuilder
    {
        /// <summary>
        /// Converts the given object to a literal. 
        /// Depending on type of literal, the Javascript equivalent will be created. 
        /// </summary>
        /// <param name="literal">Literal being converted</param>
        /// <returns>Converted literal</returns>
        public static string ConvertToLiteral(object literal)
        {
            if (literal is bool)
            {
                var boolLiteral = (bool)literal;
                if (boolLiteral)
                {
                    return "true";
                }
                else
                {
                    return "false";
                }
            }

            if (literal is int || literal is long || literal is short || literal is double || literal is float)
            {
                return literal.ToString();
            }

            if (literal is string)
            {
                return string.Format("\"{0}\"", literal.ToString());
            }

            throw new NotImplementedException(literal.GetType().ToString() + " is not supported in ConvertToLiteral");
        }
    }
}
