namespace DatenMeister.Entities.AsObject.DM
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.6.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ExtentInstance : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public ExtentInstance(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.DM.Types.ExtentInstance);
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

        public System.String getUrl()
        {
            return getUrl(this);
        }

        public void setUrl(System.String value)
        {
            setUrl(this, value);
        }

        public static System.String getUrl(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("url"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setUrl(DatenMeister.IObject obj, System.String value)
        {
            obj.set("url", value);
        }

        public System.String getName()
        {
            return getName(this);
        }

        public void setName(System.String value)
        {
            setName(this, value);
        }

        public static System.String getName(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("name"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public DatenMeister.Logic.ExtentType getExtentType()
        {
            return getExtentType(this);
        }

        public void setExtentType(DatenMeister.Logic.ExtentType value)
        {
            setExtentType(this, value);
        }

        public static DatenMeister.Logic.ExtentType getExtentType(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("extentType"));
            return (result is DatenMeister.Logic.ExtentType) ? ((DatenMeister.Logic.ExtentType) result) : default(DatenMeister.Logic.ExtentType);
        }

        public static void setExtentType(DatenMeister.IObject obj, DatenMeister.Logic.ExtentType value)
        {
            obj.set("extentType", value);
        }

        public System.Boolean isPrepopulated()
        {
            return isPrepopulated(this);
        }

        public void setPrepopulated(System.Boolean value)
        {
            setPrepopulated(this, value);
        }

        public static System.Boolean isPrepopulated(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isPrepopulated"));
            return DatenMeister.ObjectConversion.ToBoolean(result);
        }

        public static void setPrepopulated(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isPrepopulated", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.6.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class RecentProject : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
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

        public System.String getFilePath()
        {
            return getFilePath(this);
        }

        public void setFilePath(System.String value)
        {
            setFilePath(this, value);
        }

        public static System.String getFilePath(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("filePath"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setFilePath(DatenMeister.IObject obj, System.String value)
        {
            obj.set("filePath", value);
        }

        public System.DateTime getCreated()
        {
            return getCreated(this);
        }

        public void setCreated(System.DateTime value)
        {
            setCreated(this, value);
        }

        public static System.DateTime getCreated(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("created"));
            return (result is System.DateTime) ? ((System.DateTime) result) : default(System.DateTime);
        }

        public static void setCreated(DatenMeister.IObject obj, System.DateTime value)
        {
            obj.set("created", value);
        }

        public System.String getName()
        {
            return getName(this);
        }

        public void setName(System.String value)
        {
            setName(this, value);
        }

        public static System.String getName(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("name"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.6.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Workbench : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public Workbench(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.DM.Types.Workbench);
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

        public System.String getPath()
        {
            return getPath(this);
        }

        public void setPath(System.String value)
        {
            setPath(this, value);
        }

        public static System.String getPath(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("path"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setPath(DatenMeister.IObject obj, System.String value)
        {
            obj.set("path", value);
        }

        public System.String getType()
        {
            return getType(this);
        }

        public void setType(System.String value)
        {
            setType(this, value);
        }

        public static System.String getType(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("type"));
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setType(DatenMeister.IObject obj, System.String value)
        {
            obj.set("type", value);
        }

        public System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance> getInstances()
        {
            return getInstances(this);
        }

        public void setInstances(System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance> value)
        {
            setInstances(this, value);
        }

        public void pushInstance(DatenMeister.Entities.DM.ExtentInstance value)
        {
            pushInstance(this, value);
        }

        public static System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance> getInstances(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsEnumeration<DatenMeister.Entities.DM.ExtentInstance>(obj.get("Instances"));
            return (result is System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance>) ? ((System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance>) result) : default(System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance>);
        }

        public static void setInstances(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<DatenMeister.Entities.DM.ExtentInstance> value)
        {
            obj.set("Instances", value);
        }

        public static void pushInstance(DatenMeister.IObject obj, DatenMeister.IObject value)
        {
            var list = DatenMeister.Extensions.AsReflectiveCollection(obj.get("Instances"));
            list.Add(value);
        }

        public static void pushInstance(DatenMeister.IObject obj, DatenMeister.Entities.DM.ExtentInstance value)
        {
            var list = DatenMeister.Extensions.AsReflectiveCollection(obj.get("Instances"));
            list.Add(value);
        }

    }

}
