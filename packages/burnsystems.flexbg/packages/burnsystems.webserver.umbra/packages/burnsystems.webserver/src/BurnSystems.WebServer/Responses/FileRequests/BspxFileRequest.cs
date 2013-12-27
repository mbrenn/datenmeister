using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using BurnSystems.ObjectActivation;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Modules.MVC;

namespace BurnSystems.WebServer.Responses.FileRequests
{
    public class BspxFileRequest :BaseDispatcher, IFileRequestDispatcher
    {
        public string PhysicalPath
        {
            get;
            set;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public BspxFileRequest()
            : base(DispatchFilter.All)
        {
        }

        /// <summary>
        /// Dispatches the object
        /// </summary>
        /// <param name="container">Container for activations</param>
        /// <param name="info">Context being used</param>
        public override void Dispatch(ObjectActivation.IActivates container, ContextDispatchInformation info)
        {
            var viewTemplate = File.ReadAllText(this.PhysicalPath);

            // Try to configuration xml file
            var completeFile = viewTemplate.TrimStart();
            if (completeFile.StartsWith("<%"))
            {
                var endPosition = completeFile.IndexOf("%>");
                if (endPosition != -1)
                {
                    viewTemplate = completeFile.Substring(endPosition + 2);

                    // <%1234%>
                    // 01234567
                    var configuration = completeFile.Substring(2, endPosition - 2);

                    // Ok, now we got our configuration node
                    var configurationNode = XDocument.Parse(configuration);

                    // Who's your daddy... Controller is your Daddy, now we got the url of the controller
                    var controllerType = configurationNode.Elements("DynamicPage").Attributes("ControllerType").FirstOrDefault();
                    var controllerWebMethod = configurationNode.Elements("DynamicPage").Attributes("WebMethod").FirstOrDefault();

                    if (controllerType == null)
                    {
                        ErrorResponse.Throw404(container, info, "Xml-Node: DynamicPage/@ControllerType not found");
                        return;
                    }

                    if (controllerWebMethod == null)
                    {
                        ErrorResponse.Throw404(container, info, "Xml-Node: DynamicPage/@WebMethod not found");
                        return;
                    }

                    // Find controller
                    var type = EnvironmentHelper.GetTypeByName(controllerType.Value);
                    if (type == null)
                    {
                        ErrorResponse.Throw404(container, info, string.Format("Type for Controller: {0} not found", controllerType.Value));
                        return;
                    }

                    // Now, create me!
                    var webMethodContainer = new ActivationContainer("BspxFileRequest");
                    webMethodContainer.BindToName("PageTemplate").ToConstant(viewTemplate);

                    using (var block = new ActivationBlock("BspxFileRequest", webMethodContainer, container as ActivationBlock))
                    {
                        var dispatcher = new ControllerDispatcher(type, DispatchFilter.All);
                        dispatcher.DispatchForWebMethod(block, info, controllerWebMethod.Value);
                    }
                }
            }
        }
    }
}
