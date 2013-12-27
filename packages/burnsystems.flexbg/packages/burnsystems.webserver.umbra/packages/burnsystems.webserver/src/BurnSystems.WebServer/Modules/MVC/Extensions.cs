using BurnSystems.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Defines the extension class
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Returns an html result to browser
        /// </summary>
        /// <param name="result">Result to be returned</param>
        public static IActionResult Html(this Controller controller, string result)
        {
            return new HtmlActionResult(result);
        }

        /// <summary>
        /// Returns html to browser and uses the Template parser as stored in container and
        /// PageTemplate as stored in container
        /// </summary>
        /// <typeparam name="T">Type of the model</typeparam>
        /// <param name="model">Model to be set</param>
        public static IActionResult TemplateOrJson<T>(this Controller controller, T model)
        {
            return new TemplateOrJsonResult<T>(model);
        }

        /// <summary>
        /// Returns an html result to browser
        /// </summary>
        /// <param name="result">Result to be returned</param>
        public static IActionResult Json(this Controller controller, object result)
        {
            return new JsonActionResult(result);
        }

        /// <summary>
        /// Returns a success object
        /// </summary>
        /// <param name="controller">Controller to be used</param>
        /// <returns>The succes object</returns>
        public static IActionResult SuccessJson(this Controller controller, bool success = true)
        {
            return controller.Json(
                new
                {
                    success = success
                });
        }

        /// <summary>
        /// Returns the bytes to the browser
        /// </summary>
        /// <param name="controller">Controller to be used</param>
        /// <param name="bytes">Bytes to be sent</param>
        /// <param name="contentType">Content-Type that will be sent to browser</param>
        /// <returns>Action result being used</returns>
        public static BinaryActionResult Bytes(this Controller controller, byte[] bytes, string contentType)
        {
            return new BinaryActionResult(bytes, contentType);
        }

        /// <summary>
        /// Gets the action result value as a dynamic object
        /// </summary>
        /// <param name="actionResult">Actionresult to be converted</param>
        /// <returns>Dynamic object to be converted</returns>
        public static dynamic GetActionResultValue(this IActionResult actionResult)
        {
            var valueAcionResult = actionResult as IValueActionResult;
            if (valueAcionResult == null)
            {
                return null;
            }

            dynamic jsonResult = valueAcionResult.ReturnObject.ToExpando();
            return jsonResult;
        }
    }
}
