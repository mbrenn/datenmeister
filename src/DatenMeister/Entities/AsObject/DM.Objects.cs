namespace DatenMeister.Entities.AsObject.DM
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class RecentProject : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public RecentProject(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.DM.Types.RecentProject);
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public DatenMeister.IObject Value
        {
            get { return this.obj; }
        }

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public object get(string propertyName)
        {
            return this.obj.get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> getAll()
        {
            return this.obj.getAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool isSet(string propertyName)
        {
            return this.obj.isSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void set(string propertyName, object value)
        {
            this.obj.set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public bool unset(string propertyName)
        {
            return this.obj.unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void delete()
        {
            this.obj.delete();
        }

        /// <summary>
        /// Gets the id of the object
        /// </summary>
        public string Id
        {
            get
            {
                return this.obj.Id;
            }
        }

		public DatenMeister.IURIExtent Extent
		{
			get
			{
				return this.obj.Extent;
			}
		}

        #endregion

        public System.String getDataPath()
        {
            return getDataPath(this);
        }

        public void setDataPath(System.String value)
        {
            setDataPath(this, value);
        }

        public static System.String getDataPath(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("dataPath"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setDataPath(DatenMeister.IObject obj, System.String value)
        {
            obj.set("dataPath", value);
        }

    }

}
