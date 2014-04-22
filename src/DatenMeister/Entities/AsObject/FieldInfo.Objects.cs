namespace DatenMeister.Entities.AsObject.FieldInfo
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Comment : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public Comment(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public Comment(DatenMeister.IObject obj, object title, object comment)
            : this(obj)
        {
            this.set("title", title);
            this.set("comment", comment);
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("comment");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setComment(System.String value)
        {
            this.set("comment", value);
        }

        public System.String getTitle()
        {
            var result = this.get("title");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setTitle(System.String value)
        {
            this.set("title", value);
        }

        public System.String getName()
        {
            var result = this.get("name");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setName(System.String value)
        {
            this.set("name", value);
        }

        public System.Boolean isReadOnly()
        {
            var result = this.get("isReadOnly");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setReadOnly(System.Boolean value)
        {
            this.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class General : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public General(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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

        public System.String getTitle()
        {
            var result = this.get("title");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setTitle(System.String value)
        {
            this.set("title", value);
        }

        public System.String getName()
        {
            var result = this.get("name");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setName(System.String value)
        {
            this.set("name", value);
        }

        public System.Boolean isReadOnly()
        {
            var result = this.get("isReadOnly");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setReadOnly(System.Boolean value)
        {
            this.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Checkbox : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public Checkbox(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public Checkbox(DatenMeister.IObject obj, object title, object name)
            : this(obj)
        {
            this.set("title", title);
            this.set("name", name);
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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

        public System.String getTitle()
        {
            var result = this.get("title");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setTitle(System.String value)
        {
            this.set("title", value);
        }

        public System.String getName()
        {
            var result = this.get("name");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setName(System.String value)
        {
            this.set("name", value);
        }

        public System.Boolean isReadOnly()
        {
            var result = this.get("isReadOnly");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setReadOnly(System.Boolean value)
        {
            this.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TextField : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public TextField(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public TextField(DatenMeister.IObject obj, object title, object name)
            : this(obj)
        {
            this.set("title", title);
            this.set("name", name);
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("width");
            return (result is System.Int32) ? ((System.Int32) result) : default(System.Int32);
        }

        public void setWidth(System.Int32 value)
        {
            this.set("width", value);
        }

        public System.Int32 getHeight()
        {
            var result = this.get("height");
            return (result is System.Int32) ? ((System.Int32) result) : default(System.Int32);
        }

        public void setHeight(System.Int32 value)
        {
            this.set("height", value);
        }

        public System.String getTitle()
        {
            var result = this.get("title");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setTitle(System.String value)
        {
            this.set("title", value);
        }

        public System.String getName()
        {
            var result = this.get("name");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setName(System.String value)
        {
            this.set("name", value);
        }

        public System.Boolean isReadOnly()
        {
            var result = this.get("isReadOnly");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setReadOnly(System.Boolean value)
        {
            this.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
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

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("text");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setText(System.String value)
        {
            this.set("text", value);
        }

        public System.String getClickUrl()
        {
            var result = this.get("clickUrl");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setClickUrl(System.String value)
        {
            this.set("clickUrl", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ReferenceByValue : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ReferenceByValue(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public ReferenceByValue(DatenMeister.IObject obj, object title, object name, object propertyValue, object referenceUrl)
            : this(obj)
        {
            this.set("title", title);
            this.set("name", name);
            this.set("propertyValue", propertyValue);
            this.set("referenceUrl", referenceUrl);
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("propertyValue");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setPropertyValue(System.String value)
        {
            this.set("propertyValue", value);
        }

        public System.String getReferenceUrl()
        {
            var result = this.get("referenceUrl");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setReferenceUrl(System.String value)
        {
            this.set("referenceUrl", value);
        }

        public System.String getTitle()
        {
            var result = this.get("title");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setTitle(System.String value)
        {
            this.set("title", value);
        }

        public System.String getName()
        {
            var result = this.get("name");
            return (result is System.String) ? ((System.String) result) : default(System.String);
        }

        public void setName(System.String value)
        {
            this.set("name", value);
        }

        public System.Boolean isReadOnly()
        {
            var result = this.get("isReadOnly");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setReadOnly(System.Boolean value)
        {
            this.set("isReadOnly", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class View : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public View(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            var result = this.get("fieldInfos");
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            var result = this.get("startInEditMode");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            this.set("startInEditMode", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class FormView : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public FormView(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("allowEdit");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowEdit(System.Boolean value)
        {
            this.set("allowEdit", value);
        }

        public System.Boolean getAllowDelete()
        {
            var result = this.get("allowDelete");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowDelete(System.Boolean value)
        {
            this.set("allowDelete", value);
        }

        public System.Boolean getAllowNew()
        {
            var result = this.get("allowNew");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowNew(System.Boolean value)
        {
            this.set("allowNew", value);
        }

        public System.Boolean getShowColumnHeaders()
        {
            var result = this.get("showColumnHeaders");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setShowColumnHeaders(System.Boolean value)
        {
            this.set("showColumnHeaders", value);
        }

        public System.Boolean getAllowNewProperty()
        {
            var result = this.get("allowNewProperty");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowNewProperty(System.Boolean value)
        {
            this.set("allowNewProperty", value);
        }

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            var result = this.get("fieldInfos");
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            var result = this.get("startInEditMode");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            this.set("startInEditMode", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.0.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class TableView : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public TableView(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

        /// <summary>
        /// Gets the property by propertyname. 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Retrieved object</returns>
        public IObject Value
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
            var result = this.get("allowEdit");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowEdit(System.Boolean value)
        {
            this.set("allowEdit", value);
        }

        public System.Boolean getAllowDelete()
        {
            var result = this.get("allowDelete");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowDelete(System.Boolean value)
        {
            this.set("allowDelete", value);
        }

        public System.Boolean getAllowNew()
        {
            var result = this.get("allowNew");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setAllowNew(System.Boolean value)
        {
            this.set("allowNew", value);
        }

        public System.Collections.Generic.IEnumerable<System.Object> getFieldInfos()
        {
            var result = this.get("fieldInfos");
            return (result is System.Collections.Generic.IEnumerable<System.Object>) ? ((System.Collections.Generic.IEnumerable<System.Object>) result) : default(System.Collections.Generic.IEnumerable<System.Object>);
        }

        public void setFieldInfos(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(System.Collections.Generic.IEnumerable<System.Object> value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public System.Boolean getStartInEditMode()
        {
            var result = this.get("startInEditMode");
            return (result is System.Boolean) ? ((System.Boolean) result) : default(System.Boolean);
        }

        public void setStartInEditMode(System.Boolean value)
        {
            this.set("startInEditMode", value);
        }

    }

}
