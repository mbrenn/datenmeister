using BurnSystems.ObjectActivation;
using BurnSystems.Test;
using BurnSystems.WebServer.Dispatcher;
using BurnSystems.WebServer.Umbra.Views.Treeview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BurnSystems.WebServer.Umbra.Views.DetailView.Entities
{
    /// <summary>
    /// Defines the table
    /// </summary>
    public class EntityViewDetailTable : EntityViewTable
    {
        /// <summary>
        /// Gets or sets the name of the table
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the url, if the browser shall send the contents
        /// of the table to another url
        /// </summary>
        public string OverrideUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the button text
        /// </summary>
        public string ButtonText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the rows
        /// </summary>
        public List<EntityViewElement> Elements
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets a selection object that will convert the native element to another object that will be used to retrieve 
        /// the properties. Setting might be possible but is not useful for simple objects
        /// </summary>
        public Func<IActivates, object, object> Selector
        {
            get;
            set;
        }        

        /// <summary>
        /// Initializes a new instance of the table
        /// </summary>
        /// <param name="name">Stores the name of the table</param>
        public EntityViewDetailTable(
            string name,
            params EntityViewElement[] elements)
        {
            this.Name = name;
            this.Elements = new List<EntityViewElement>();

            foreach (var element in elements)
            {
                this.AddElement(element);
            }
        }

        /// <summary>
        /// Adds a element
        /// </summary>
        /// <param name="element">Element to be added</param>
        /// <returns>Same instance</returns>
        public EntityViewDetailTable AddElement(EntityViewElement element)
        {
            this.Elements.Add(element);
            return this;
        }

        /// <summary>
        /// Gets the table as json
        /// </summary>
        /// <param name="item">Item which shall be executed</param>
        /// <returns>Object that can be converted as a json object</returns>
        public override object ToJson(IActivates container, ITreeViewItem item)
        {
            var context = container.Get<ContextDispatchInformation>();
            Ensure.That(context != null);

            if (this.OverrideUrl == null)
            {
                this.OverrideUrl = context.Context.Request.Url.ToString() + "?t=update&n=" + this.Name;
            }

            // Performs the selection
            object entity = null;
            if (item != null)
            {
                entity = item.Entity;
                if (this.Selector != null)
                {
                    entity = this.Selector(container, entity);
                }
            }

            // Creates json stuf
            return new
            {
                type = "detail",
                elements = this.Elements.Select(x => x.ToJson(container)),
                data = this.Elements.Select(x =>
                    {
                        if (entity == null)
                        {
                            return null;
                        }
                        else
                        {
                            return x.ObjectToJson(entity);
                        }
                    }),
                overrideUrl = this.OverrideUrl,
                buttonText = this.ButtonText
            };
        }

        #region Some helper methods

        public EntityViewDetailTable WithOverrideUrl(string overrideUrl)
        {
            this.OverrideUrl = overrideUrl;
            return this;
        }

        public EntityViewDetailTable SetButtonText(string buttonText)
        {
            this.ButtonText = buttonText;
            return this;
        }

        public EntityViewDetailTable WithSelector<T>(Func<IActivates, T, object> func)
        {
            this.Selector = (x, y) =>
                {
                    return func(x, (T)y);
                };

            return this;
        }

        #endregion
    }
}
