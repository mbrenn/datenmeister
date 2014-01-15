using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BurnSystems.Extensions;
using BurnSystems.Logging;
using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Modules.PostVariables;
using BurnSystems.WebServer.Responses;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace BurnSystems.WebServer.Modules.MVC
{
    /// <summary>
    /// Contains the controller dispatcher for a specific controller class
    /// </summary>
    /// <typeparam name="T">Type of the controller</typeparam>
    public class ControllerDispatcher<T> : ControllerDispatcher where T : Controller, new()
    {
        /// <summary>
        /// Initializes a new instance of the ControllerDispatcher class
        /// </summary>
        /// <param name="filter">Filter being used</param>
        public ControllerDispatcher(Func<ContextDispatchInformation, bool> filter)
            : base(typeof(T), filter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ControllerDispatcher class
        /// </summary>
        /// <param name="filter">Filter being used</param>
        /// <param name="webPath">Virtual web path being used to strip away constant part</param>
        public ControllerDispatcher(Func<ContextDispatchInformation, bool> filter, string webPath)
            : base(typeof(T), filter, webPath)
        {
        }
    }

    public class ControllerDispatcher : BaseDispatcher
    {
        /// <summary>
        /// Stores the type of the controller
        /// </summary>
        private Type controllerType;

        /// <summary>
        /// Logger being used in this class.
        /// </summary>
        private static ClassLogger logger = new ClassLogger(typeof(ControllerDispatcher));

        /// <summary>
        /// Stores the webmethod infos
        /// </summary>
        private List<WebMethodInfo> webMethodInfos = new List<WebMethodInfo>();

        /// <summary>
        /// Gets or sets the webpath, including controller name
        /// </summary>
        public string WebPath
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of the ControllerDispatcher class
        /// </summary>
        /// <param name="controllerType">Type of the controller being used</param>
        /// <param name="filter">Filter being used</param>
        public ControllerDispatcher(Type controllerType, Func<ContextDispatchInformation, bool> filter)
            : base(filter)
        {
            this.controllerType = controllerType;
            this.InitializeWebMethods();
        }

        /// <summary>
        /// Initializes a new instance of the ControllerDispatcher class
        /// </summary>
        /// <param name="controllerType">Type of the controller</param>
        /// <param name="filter">Filter being used</param>
        /// <param name="webPath">Path, where controller is stored</param>
        public ControllerDispatcher(Type controllerType, Func<ContextDispatchInformation, bool> filter, string webPath)
            : this(controllerType, filter)
        {
            this.WebPath = webPath;

            if (!webPath.EndsWith("/"))
            {
                logger.Fail("WebPath '" + webPath + "' does not end with trailing slash '/'");
            }
        }

        /// <summary>
        /// Dispatches the object, depending of activity and context
        /// </summary>
        /// <param name="activates">Container used for activation</param>
        /// <param name="context">Context of http</param>
        public override void Dispatch(ObjectActivation.IActivates activates, ContextDispatchInformation info)
        {
            // Stores the absolute path
            var absolutePath = info.RequestUrl.AbsolutePath;
            if (!string.IsNullOrEmpty(this.WebPath) && !this.WebPath.EndsWith("/"))
            {
                this.WebPath = this.WebPath + "/";
                logger.LogEntry(new LogEntry("this.WebPath did not end with '/'", LogLevel.Verbose));
            }

            if (!absolutePath.StartsWith(this.WebPath))
            {
                // I'm not the real responsible for this task
                ErrorResponse.Throw404(activates, info);
                return;
            }

            // Now try to find the correct method and call the function
            var restUrl = absolutePath.Substring(this.WebPath.Length);
            if (string.IsNullOrEmpty(restUrl))
            {
                // Nothing to do here... Default page. Not implemented yet
                ErrorResponse.Throw404(activates, info);
                return;
            }

            // Split segments
            var segments = restUrl.Split(new char[] { '/' });

            // First segment is method name
            var methodName = segments[0];

            this.DispatchForWebMethod(activates, info, methodName);
        }

        /// <summary>
        /// Performs the dispatch for a specific 
        /// </summary>
        /// <param name="activates">Activation Container</param>
        /// <param name="context">WebContext for request</param>
        /// <param name="controller">Controller to be used</param>
        /// <param name="methodName">Requested web method</param>
        public void DispatchForWebMethod (ObjectActivation.IActivates activates, ContextDispatchInformation info, string methodName)
		{
			try {
				// Creates controller
				var controller = activates.Create (this.controllerType) as Controller;
				Ensure.That (controller != null, "Not a ControllerType: " + this.controllerType.FullName);

				// Try to find the method
				foreach (var webMethodInfo in this.webMethodInfos
                    .Where(x => x.Name == methodName)) {
					// Check for http Method
					if (!string.IsNullOrEmpty (webMethodInfo.IfMethodIs)) {
						if (webMethodInfo.IfMethodIs != info.HttpMethod.ToLower ()) {
							// No match, 
							continue;
						}
					}

					// Ok, now look for get variables
					var parameters = webMethodInfo.MethodInfo.GetParameters ();
					var callArguments = new List<object> ();
					foreach (var parameter in parameters) {
						var parameterAttributes = parameter.GetCustomAttributes (false);

						/////////////////////////////////
						// Check for POST-Parameter
						var postParameterAttribute = parameterAttributes.Where (x => x is PostModelAttribute).Cast<PostModelAttribute> ()
							.FirstOrDefault ();
						if (postParameterAttribute != null) {
							if (info.HttpMethod.ToLower () != "post") {
								callArguments.Add (null);
							} else {
								callArguments.Add (
                                    this.CreatePostModel (activates, parameter));
							}

							continue;
						}

						/////////////////////////////////
						// Check for RawPost
						var rawPostAttribute = parameterAttributes.Where (x => x is RawPostAttribute).Cast<RawPostAttribute> ()
							.FirstOrDefault ();
						if (rawPostAttribute != null) {
							using (var requestStream = info.Context.Request.InputStream) {
								using (var reader = new BinaryReader(requestStream)) {
									var length = (int)info.Context.Request.ContentLength64;
									Ensure.That (info.Context.Request.ContentLength64 < 10 * 1024 * 1024);

									var data = new byte[length];
									var toBeRead = length;
									var alreadyRead = 0;

									while (toBeRead > 0) {
										var read = reader.Read (data, alreadyRead, length - alreadyRead);
										Ensure.That (read > 0, "Less bytes read than expected");

										toBeRead -= read;
										alreadyRead += read;
									}

									callArguments.Add (data);
								}
							}

							continue;
						}

						/////////////////////////////////
						// Check for injection parameter
						var injectParameterAttribute = parameterAttributes.Where (x => x is InjectAttribute).Cast<InjectAttribute> ()
							.FirstOrDefault ();
						if (injectParameterAttribute != null) {
							object argument;
							if (string.IsNullOrEmpty (injectParameterAttribute.ByName)) {
								// Ok, we are NOT by name, injection by type
								argument = activates.Get (parameter.ParameterType);
							} else {
								argument = activates.GetByName (injectParameterAttribute.ByName);
							}

							if (argument == null && injectParameterAttribute.IsMandatory) {
								throw new InvalidOperationException ("Parameter '" + injectParameterAttribute.ByName + "' is required as mandatory but has not been set");
							}

							callArguments.Add (argument);
							continue;
						}

						/////////////////////////////////
						// Check for Url-Parameter
						var urlParameterAttributes = parameterAttributes.Where (x => x is UrlParameterAttribute).FirstOrDefault ();
						if (urlParameterAttributes != null) {
							callArguments.Add (null);
							// Is a url parameter attribute, do not like this
							continue;
						}

						// Rest is Get-Parameter
						var value = info.Context.Request.QueryString [parameter.Name];
						Console.WriteLine (info.Context.Request.Url.ToString ());
						Console.WriteLine ("Parameter: " + parameter.Name + " = " + value);
                        if (value == null)
                        {
                            callArguments.Add(this.GetDefaultArgument(parameter));
                        }
                        else
                        {
                            callArguments.Add(this.ConvertToArgument(value, parameter));
                        }
                    }

                    try
                    {
                        var result = webMethodInfo.MethodInfo.Invoke(controller, callArguments.ToArray()) as IActionResult;
                        if (result == null)
                        {
                            logger.LogEntry(LogEntry.Format(LogLevel.Message, "WebMethod '{0}' does not return IActionResult", methodName));
                        }
                        else
                        {
                            result.Execute(info.Context, activates);
                        }
                    }
                    catch (TargetInvocationException targetInvocation)
                    {
                        var exception = targetInvocation.InnerException as MVCProcessException;
                        if (exception != null)
                        {
                            // We have an MVC Exception, create datastructure and return errormessage
                            var result = new
                            {
                                success = false,
                                error = new
                                {
                                    type = "MVCProcessException",
                                    code = exception.Code,
                                    message = exception.Message,
                                    success = false
                                }
                            };

                            new TemplateOrJsonResult<object>(result).Execute(info.Context, activates);
                        }
                        else
                        {
                            throw;
                        }
                    }

                    // First hit is success
                    return;
                }

                ErrorResponse.Throw404(activates, info, "No Webmethod for '" + methodName + "' found");
                return;
            }
            catch (Exception exc)
            {
                logger.LogEntry(LogLevel.Verbose, exc.ToString());
                throw new InvalidOperationException(
                    string.Format("Exception occured during execution of '" + methodName + "': " + exc.Message),
                    exc);
            }
        }

        /// <summary>
        /// Creates the post model for a certain parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private object CreatePostModel(IActivates activates, System.Reflection.ParameterInfo parameter)
        {
            var httpListenerContext = activates.Get<HttpListenerContext>();
            Ensure.That(httpListenerContext != null);

            if (httpListenerContext.Request.ContentType != null && 
				httpListenerContext.Request.ContentType.ToLower().Contains("application/json"))
            {
                // Ok, we have application/Json
                return CreateModelByJson(activates, parameter);
            }
            else
            {
                return CreatePostModelByFormData(activates, parameter);
            }
        }

        /// <summary>
        /// Creates the post model by Json object. Json.Net library will be used to deserialize the object
        /// </summary>
        /// <param name="activates">Activation container</param>
        /// <param name="parameter">Parameter information</param>
        /// <returns>Created Object</returns>
        private object CreateModelByJson(IActivates activates, ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;

            var deserializer = activates.Create<PostModelJsonDeserializer>();
            return deserializer.Deserialize(parameterType);
        }

        /// <summary>
        /// Creates the postmodel by formdata, RFC 2388
        /// </summary>
        /// <param name="activates">Activation container</param>
        /// <param name="parameter">Parameterinto to be filled</param>
        /// <returns>Created Object</returns>
        private static object CreatePostModelByFormData(IActivates activates, System.Reflection.ParameterInfo parameter)
        {
            var parameterType = parameter.ParameterType;
            var postVariables = activates.Get<PostVariableReader>();

            if (parameter.ParameterType == typeof(Dictionary<string, string>))
            {
                var result = new Dictionary<string, string>();

                foreach (var pair in postVariables)
                {
                    result[pair.Key] = pair.Value;
                }

                return result;
            }
            else
            {
                var instance = Activator.CreateInstance(parameterType);

                foreach (var property in parameterType.GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public))
                {
                    // Checks, if type is Webfile and reads from files
                    if (property.PropertyType == typeof(WebFile))
                    {
                        property.SetValue(
                            instance,
                            postVariables.Files[property.Name],
                            null);
                    }
                    else
                    {
                        // Reads normal post variable
                        var value = postVariables[property.Name];
                        if (value == null)
                        {
                            continue;
                        }

                        property.SetValue(
                            instance,
                            ConvertToType(value, property.PropertyType),
                            null);
                    }
                }

                return instance;
            }
        }

        /// <summary>
        /// Initializes the webmethods and stores the webmethods into 
        /// </summary>
        private void InitializeWebMethods()
        {
            // Try to find method
            var foundMethods = this.controllerType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                .Select(x => new
                {
                    Method = x,
                    WebMethodAttribute = (WebMethodAttribute)x.GetCustomAttributes(typeof(WebMethodAttribute), false).FirstOrDefault()
                })
                .Where(x => x.WebMethodAttribute != null);

            // Convert methods into predefined data structures
            foreach (var info in foundMethods)
            {
                var name = info.Method.Name;
                if (!string.IsNullOrEmpty(info.WebMethodAttribute.Name))
                {
                    name = info.WebMethodAttribute.Name;
                }

                var code = new WebMethodInfo()
                {
                    MethodInfo = info.Method,
                    Name = name
                };

                var methods = info.Method.GetCustomAttributes(typeof(IfMethodIsAttribute), false);
                if (methods.Length == 1)
                {
                    code.IfMethodIs = (methods[0] as IfMethodIsAttribute).MethodName;
                }

                this.webMethodInfos.Add(code);
            }
        }

        /// <summary>
        /// Gets the default argument, depending on type
        /// </summary>
        /// <param name="parameter">Used parameter info</param>
        /// <returns>Default argument</returns>
        private object GetDefaultArgument(System.Reflection.ParameterInfo parameter)
        {
            if (parameter.ParameterType == typeof(string))
            {
                return null;
            }

            if (parameter.ParameterType == typeof(int))
            {
                return 0;
            }

            if (parameter.ParameterType == typeof(double))
            {
                return 0.0;
            }

            if (parameter.ParameterType == typeof(long))
            {
                return 0L;
            }

            if (parameter.ParameterType == typeof(Guid))
            {
                return Guid.Empty;
            }

            throw new ArgumentException("Unknown Parameter Type: " + parameter.ParameterType);
        }

        /// <summary>
        /// Converts the given string to a parameter
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="parameter">Parameter Info</param>
        /// <returns>Converted Argument</returns>
        private object ConvertToArgument(string value, System.Reflection.ParameterInfo parameter)
        {
            var type = parameter.ParameterType; 
            return ConvertToType(value, type);
        }

        /// <summary>
        /// Converts the given string to a type
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="type">Type of the argument</param>
        /// <returns>Converted Argument</returns>
        private static object ConvertToType(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }

            if (type == typeof(int))
            {
                return Convert.ToInt32(value);
            }

            if (type == typeof(double))
            {
                return Convert.ToDouble(value);
            }

            if (type == typeof(long))
            {
                return Convert.ToInt64(value);
            }

            if (type == typeof(Guid))
            {
                return new Guid(value);
            }

            if (type == typeof(bool))
            {
                if (value.ToLowerInvariant() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            throw new ArgumentException("Unknown Parameter Type: " + type);
        }
    }
}
