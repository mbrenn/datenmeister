namespace DatenMeister.AddOns.Data.FileSystem.AsObject
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.7.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class File : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public File(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public File(DatenMeister.IObject obj, object fileInfo, object relativePath)
            : this(obj)
        {
            this.set("fileInfo", fileInfo);
            this.set("relativePath", relativePath);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.AddOns.Data.FileSystem.AsObject.Types.File);
        }

        #region IObject Implementation

        public static DatenMeister.IObject create(DatenMeister.IURIExtent extent)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            return create(factory); 
        }

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

        public System.String getRelativePath()
        {
            return getRelativePath(this);
        }

        public void setRelativePath(System.String value)
        {
            setRelativePath(this, value);
        }

        public static System.String getRelativePath(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("relativePath"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setRelativePath(DatenMeister.IObject obj, System.String value)
        {
            obj.set("relativePath", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.7.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Directory : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public Directory(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public Directory(DatenMeister.IObject obj, object directoryInfo, object relativePath)
            : this(obj)
        {
            this.set("directoryInfo", directoryInfo);
            this.set("relativePath", relativePath);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.AddOns.Data.FileSystem.AsObject.Types.Directory);
        }

        #region IObject Implementation

        public static DatenMeister.IObject create(DatenMeister.IURIExtent extent)
        {
            var factory = DatenMeister.DataProvider.Factory.GetFor(extent);
            return create(factory); 
        }

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

        public System.String getRelativePath()
        {
            return getRelativePath(this);
        }

        public void setRelativePath(System.String value)
        {
            setRelativePath(this, value);
        }

        public static System.String getRelativePath(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("relativePath"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setRelativePath(DatenMeister.IObject obj, System.String value)
        {
            obj.set("relativePath", value);
        }

    }

}
