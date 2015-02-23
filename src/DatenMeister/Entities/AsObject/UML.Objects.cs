namespace DatenMeister.Entities.AsObject.Uml
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class NamedElement : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public NamedElement(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.Uml.Types.NamedElement);
        }

        public static NamedElement createTyped(DatenMeister.IFactory factory)
        {
            return new NamedElement(create(factory));
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
        public object get(string propertyName, DatenMeister.RequestType requestType = DatenMeister.RequestType.AsDefault)
        {
            return this.obj.get(propertyName, requestType);
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
            var result = obj.get("name", DatenMeister.RequestType.AsSingle);
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Type : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public Type(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.Uml.Types.Type);
        }

        public static Type createTyped(DatenMeister.IFactory factory)
        {
            return new Type(create(factory));
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
        public object get(string propertyName, DatenMeister.RequestType requestType = DatenMeister.RequestType.AsDefault)
        {
            return this.obj.get(propertyName, requestType);
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
            var result = obj.get("name", DatenMeister.RequestType.AsSingle);
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Property : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public Property(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.Uml.Types.Property);
        }

        public static Property createTyped(DatenMeister.IFactory factory)
        {
            return new Property(create(factory));
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
        public object get(string propertyName, DatenMeister.RequestType requestType = DatenMeister.RequestType.AsDefault)
        {
            return this.obj.get(propertyName, requestType);
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
            var result = obj.get("name", DatenMeister.RequestType.AsSingle);
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

    }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DatenMeister.Logic.SourceFactory.CSharpSourceFactory", "1.1.0.0")]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Class : DatenMeister.IObject, DatenMeister.DataProvider.IProxyObject
    {
        private DatenMeister.IObject obj;
        public Class(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        public static DatenMeister.IObject create(DatenMeister.IFactory factory)
        {
            return factory.create(DatenMeister.Entities.AsObject.Uml.Types.Class);
        }

        public static Class createTyped(DatenMeister.IFactory factory)
        {
            return new Class(create(factory));
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
        public object get(string propertyName, DatenMeister.RequestType requestType = DatenMeister.RequestType.AsDefault)
        {
            return this.obj.get(propertyName, requestType);
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

        public System.Boolean isAbstract()
        {
            return isAbstract(this);
        }

        public void setAbstract(System.Boolean value)
        {
            setAbstract(this, value);
        }

        public static System.Boolean isAbstract(DatenMeister.IObject obj)
        {
            var result = obj.get("isAbstract", DatenMeister.RequestType.AsSingle);
            return DatenMeister.ObjectConversion.ToBoolean(result);
        }

        public static void setAbstract(DatenMeister.IObject obj, System.Boolean value)
        {
            obj.set("isAbstract", value);
        }

        public System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property> getOwnedAttribute()
        {
            return getOwnedAttribute(this);
        }

        public void setOwnedAttribute(System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property> value)
        {
            setOwnedAttribute(this, value);
        }

        public void pushOwnedAttribute(DatenMeister.Entities.UML.Property value)
        {
            pushOwnedAttribute(this, value);
        }

        public static System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property> getOwnedAttribute(DatenMeister.IObject obj)
        {
            var result = DatenMeister.Extensions.getAsReflectiveSequence(obj, "ownedAttribute");
            return (result is System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property>) ? ((System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property>) result) : default(System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property>);
        }

        public static void setOwnedAttribute(DatenMeister.IObject obj, System.Collections.Generic.IEnumerable<DatenMeister.Entities.UML.Property> value)
        {
            obj.set("ownedAttribute", value);
        }

        public static void pushOwnedAttribute(DatenMeister.IObject obj, DatenMeister.IObject value)
        {
            var list = DatenMeister.Extensions.getAsReflectiveSequence(obj, "ownedAttribute");
            list.Add(value);
        }

        public static void pushOwnedAttribute(DatenMeister.IObject obj, DatenMeister.Entities.UML.Property value)
        {
            var list = DatenMeister.Extensions.getAsReflectiveSequence(obj, "ownedAttribute");
            list.Add(value);
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
            var result = obj.get("name", DatenMeister.RequestType.AsSingle);
            return DatenMeister.ObjectConversion.ToString(result);
        }

        public static void setName(DatenMeister.IObject obj, System.String value)
        {
            obj.set("name", value);
        }

    }

}
