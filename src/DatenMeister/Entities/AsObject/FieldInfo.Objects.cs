namespace DatenMeister.Entities.AsObject.FieldInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Comment : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public Comment(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public Comment(DatenMeister.IObject obj, object name, object comment)
            : this(obj)
        {
            this.set("name", name);
            this.set("comment", comment);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.Comment);
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

        public System.String getComment()
        {
            return getComment(this);
        }

        public void setComment(System.String value)
        {
            setComment(this, value);
        }

        public static System.String getComment(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("comment"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setComment(DatenMeister.IObject obj, System.String value)
        {
            obj.set("comment", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class General : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public General(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public General(DatenMeister.IObject obj, object name, object binding)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.General);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Checkbox : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public Checkbox(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public Checkbox(DatenMeister.IObject obj, object name, object binding)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.Checkbox);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TextField : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public TextField(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public TextField(DatenMeister.IObject obj, object name, object binding)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.TextField);
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

        public System.Int32 getWidth()
        {
            return getWidth(this);
        }

        public void setWidth(System.Int32 value)
        {
            setWidth(this, value);
        }

        public static System.Int32 getWidth(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("width"));
            return (result is System.Int32) ? ((System.Int32) result) : default(System.Int32);
        }

        public static void setWidth(DatenMeister.IObject obj, System.Int32 value)
        {
            obj.set("width", value);
        }

        public System.Int32 getHeight()
        {
            return getHeight(this);
        }

        public void setHeight(System.Int32 value)
        {
            setHeight(this, value);
        }

        public static System.Int32 getHeight(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("height"));
            return (result is System.Int32) ? ((System.Int32) result) : default(System.Int32);
        }

        public static void setHeight(DatenMeister.IObject obj, System.Int32 value)
        {
            obj.set("height", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ActionButton : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ActionButton(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public ActionButton(DatenMeister.IObject obj, object text, object clickUrl)
            : this(obj)
        {
            this.set("text", text);
            this.set("clickUrl", clickUrl);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.ActionButton);
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

        public System.String getText()
        {
            return getText(this);
        }

        public void setText(System.String value)
        {
            setText(this, value);
        }

        public static System.String getText(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("text"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setText(DatenMeister.IObject obj, System.String value)
        {
            obj.set("text", value);
        }

        public System.String getClickUrl()
        {
            return getClickUrl(this);
        }

        public void setClickUrl(System.String value)
        {
            setClickUrl(this, value);
        }

        public static System.String getClickUrl(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("clickUrl"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setClickUrl(DatenMeister.IObject obj, System.String value)
        {
            obj.set("clickUrl", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ReferenceBase : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ReferenceBase(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public ReferenceBase(DatenMeister.IObject obj, object name, object binding, object referenceUrl, object propertyValue)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
            this.set("referenceUrl", referenceUrl);
            this.set("propertyValue", propertyValue);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.ReferenceBase);
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

        public System.String getPropertyValue()
        {
            return getPropertyValue(this);
        }

        public void setPropertyValue(System.String value)
        {
            setPropertyValue(this, value);
        }

        public static System.String getPropertyValue(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("propertyValue"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setPropertyValue(DatenMeister.IObject obj, System.String value)
        {
            obj.set("propertyValue", value);
        }

        public System.String getReferenceUrl()
        {
            return getReferenceUrl(this);
        }

        public void setReferenceUrl(System.String value)
        {
            setReferenceUrl(this, value);
        }

        public static System.String getReferenceUrl(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("referenceUrl"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setReferenceUrl(DatenMeister.IObject obj, System.String value)
        {
            obj.set("referenceUrl", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ReferenceByValue : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ReferenceByValue(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public ReferenceByValue(DatenMeister.IObject obj, object name, object binding, object referenceUrl, object propertyValue)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
            this.set("referenceUrl", referenceUrl);
            this.set("propertyValue", propertyValue);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.ReferenceByValue);
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

        public System.String getPropertyValue()
        {
            return getPropertyValue(this);
        }

        public void setPropertyValue(System.String value)
        {
            setPropertyValue(this, value);
        }

        public static System.String getPropertyValue(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("propertyValue"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setPropertyValue(DatenMeister.IObject obj, System.String value)
        {
            obj.set("propertyValue", value);
        }

        public System.String getReferenceUrl()
        {
            return getReferenceUrl(this);
        }

        public void setReferenceUrl(System.String value)
        {
            setReferenceUrl(this, value);
        }

        public static System.String getReferenceUrl(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("referenceUrl"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setReferenceUrl(DatenMeister.IObject obj, System.String value)
        {
            obj.set("referenceUrl", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ReferenceByRef : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ReferenceByRef(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public ReferenceByRef(DatenMeister.IObject obj, object name, object binding, object referenceUrl, object propertyValue)
            : this(obj)
        {
            this.set("name", name);
            this.set("binding", binding);
            this.set("referenceUrl", referenceUrl);
            this.set("propertyValue", propertyValue);
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.ReferenceByRef);
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

        public System.String getPropertyValue()
        {
            return getPropertyValue(this);
        }

        public void setPropertyValue(System.String value)
        {
            setPropertyValue(this, value);
        }

        public static System.String getPropertyValue(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("propertyValue"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setPropertyValue(DatenMeister.IObject obj, System.String value)
        {
            obj.set("propertyValue", value);
        }

        public System.String getReferenceUrl()
        {
            return getReferenceUrl(this);
        }

        public void setReferenceUrl(System.String value)
        {
            setReferenceUrl(this, value);
        }

        public static System.String getReferenceUrl(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("referenceUrl"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setReferenceUrl(DatenMeister.IObject obj, System.String value)
        {
            obj.set("referenceUrl", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.String getBinding()
        {
            return getBinding(this);
        }

        public void setBinding(System.String value)
        {
            setBinding(this, value);
        }

        public static System.String getBinding(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("binding"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setBinding(DatenMeister.IObject obj, System.String value)
        {
            obj.set("binding", value);
        }

        public System.Boolean isReadOnly()
        {
            return isReadOnly(this);
        }

        public void setReadOnly(System.Boolean value)
        {
            setReadOnly(this, value);
        }

        public static System.Boolean isReadOnly(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("isReadOnly"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setReadOnly(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class View : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public View(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.View);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            return getFieldInfos(this);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            setFieldInfos(this, value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            pushFieldInfo(this, value);
        }

        public static System.Collections.Generic.IEnumerable<System.Object> getFieldInfos(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsEnumeration(obj.get("fieldInfos"));
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public static void setFieldInfos(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            obj.set("fieldInfos", value);
        }

        public static void pushFieldInfo(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = obj.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            obj.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            return getStartInEditMode(this);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            setStartInEditMode(this, value);
        }

        public static System.Boolean getStartInEditMode(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("startInEditMode"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setStartInEditMode(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("startInEditMode", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class FormView : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public FormView(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.FormView);
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

        public System.Boolean getAllowEdit()
        {
            return getAllowEdit(this);
        }

        public void setAllowEdit(System.Boolean value)
        {
            setAllowEdit(this, value);
        }

        public static System.Boolean getAllowEdit(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowEdit"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowEdit(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowEdit", value);
        }

        public System.Boolean getAllowDelete()
        {
            return getAllowDelete(this);
        }

        public void setAllowDelete(System.Boolean value)
        {
            setAllowDelete(this, value);
        }

        public static System.Boolean getAllowDelete(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowDelete"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowDelete(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowDelete", value);
        }

        public System.Boolean getAllowNew()
        {
            return getAllowNew(this);
        }

        public void setAllowNew(System.Boolean value)
        {
            setAllowNew(this, value);
        }

        public static System.Boolean getAllowNew(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowNew"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowNew(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowNew", value);
        }

        public System.Boolean getShowColumnHeaders()
        {
            return getShowColumnHeaders(this);
        }

        public void setShowColumnHeaders(System.Boolean value)
        {
            setShowColumnHeaders(this, value);
        }

        public static System.Boolean getShowColumnHeaders(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("showColumnHeaders"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setShowColumnHeaders(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("showColumnHeaders", value);
        }

        public System.Boolean getAllowNewProperty()
        {
            return getAllowNewProperty(this);
        }

        public void setAllowNewProperty(System.Boolean value)
        {
            setAllowNewProperty(this, value);
        }

        public static System.Boolean getAllowNewProperty(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowNewProperty"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowNewProperty(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowNewProperty", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            return getFieldInfos(this);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            setFieldInfos(this, value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            pushFieldInfo(this, value);
        }

        public static System.Collections.Generic.IEnumerable<System.Object> getFieldInfos(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsEnumeration(obj.get("fieldInfos"));
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public static void setFieldInfos(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            obj.set("fieldInfos", value);
        }

        public static void pushFieldInfo(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = obj.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            obj.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            return getStartInEditMode(this);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            setStartInEditMode(this, value);
        }

        public static System.Boolean getStartInEditMode(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("startInEditMode"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setStartInEditMode(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("startInEditMode", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.5.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TableView : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public TableView(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.FieldInfo.Types.TableView);
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

        public System.String getExtentUri()
        {
            return getExtentUri(this);
        }

        public void setExtentUri(System.String value)
        {
            setExtentUri(this, value);
        }

        public static System.String getExtentUri(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("extentUri"));
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setExtentUri(DatenMeister.IObject obj, System.String value)
        {
            obj.set("extentUri", value);
        }

        public DatenMeister.IObject getMainType()
        {
            return getMainType(this);
        }

        public void setMainType(DatenMeister.IObject value)
        {
            setMainType(this, value);
        }

        public static DatenMeister.IObject getMainType(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("mainType"));
            return (result is DatenMeister.IObject) ? ((DatenMeister.IObject) result) : default(DatenMeister.IObject);
        }

        public static void setMainType(DatenMeister.IObject obj, DatenMeister.IObject value)
        {
            obj.set("mainType", value);
        }

        public System.Boolean getAllowEdit()
        {
            return getAllowEdit(this);
        }

        public void setAllowEdit(System.Boolean value)
        {
            setAllowEdit(this, value);
        }

        public static System.Boolean getAllowEdit(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowEdit"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowEdit(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowEdit", value);
        }

        public System.Boolean getAllowDelete()
        {
            return getAllowDelete(this);
        }

        public void setAllowDelete(System.Boolean value)
        {
            setAllowDelete(this, value);
        }

        public static System.Boolean getAllowDelete(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowDelete"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowDelete(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowDelete", value);
        }

        public System.Boolean getAllowNew()
        {
            return getAllowNew(this);
        }

        public void setAllowNew(System.Boolean value)
        {
            setAllowNew(this, value);
        }

        public static System.Boolean getAllowNew(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("allowNew"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setAllowNew(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("allowNew", value);
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
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            return getFieldInfos(this);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            setFieldInfos(this, value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            pushFieldInfo(this, value);
        }

        public static System.Collections.Generic.IEnumerable<System.Object> getFieldInfos(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsEnumeration(obj.get("fieldInfos"));
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public static void setFieldInfos(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            obj.set("fieldInfos", value);
        }

        public static void pushFieldInfo(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = obj.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            obj.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            return getStartInEditMode(this);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            setStartInEditMode(this, value);
        }

        public static System.Boolean getStartInEditMode(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.AsSingle(obj.get("startInEditMode"));
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public static void setStartInEditMode(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("startInEditMode", value);
        }

    }

}
