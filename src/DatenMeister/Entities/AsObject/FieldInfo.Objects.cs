namespace DatenMeister.Entities.AsObject.Uml.FieldInfo
{
    public class Comment : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public Comment(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getComment()
        {
            return this.get("comment");
        }

        public void setComment(object value)
        {
            this.set("comment", value);
        }

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

    public class TextField : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public TextField(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getWidth()
        {
            return this.get("width");
        }

        public void setWidth(object value)
        {
            this.set("width", value);
        }

        public object getHeight()
        {
            return this.get("height");
        }

        public void setHeight(object value)
        {
            this.set("height", value);
        }

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

    public class ActionButton : DatenMeister.IObject
    {
        private DatenMeister.IObject obj;
        public ActionButton(DatenMeister.IObject obj)
        {
            this.obj = obj;
        }

        #region IObject Implementation

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getText()
        {
            return this.get("text");
        }

        public void setText(object value)
        {
            this.set("text", value);
        }

        public object getClickUrl()
        {
            return this.get("clickUrl");
        }

        public void setClickUrl(object value)
        {
            this.set("clickUrl", value);
        }

    }

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getFieldInfos()
        {
            return this.get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getAllowEdit()
        {
            return this.get("allowEdit");
        }

        public void setAllowEdit(object value)
        {
            this.set("allowEdit", value);
        }

        public object getAllowDelete()
        {
            return this.get("allowDelete");
        }

        public void setAllowDelete(object value)
        {
            this.set("allowDelete", value);
        }

        public object getAllowNew()
        {
            return this.get("allowNew");
        }

        public void setAllowNew(object value)
        {
            this.set("allowNew", value);
        }

        public object getShowColumnHeaders()
        {
            return this.get("showColumnHeaders");
        }

        public void setShowColumnHeaders(object value)
        {
            this.set("showColumnHeaders", value);
        }

        public object getAllowNewProperty()
        {
            return this.get("allowNewProperty");
        }

        public void setAllowNewProperty(object value)
        {
            this.set("allowNewProperty", value);
        }

        public object getFieldInfos()
        {
            return this.get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

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
        public void unset(string propertyName)
        {
            this.obj.unset(propertyName);
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

        #endregion

        public object getAllowEdit()
        {
            return this.get("allowEdit");
        }

        public void setAllowEdit(object value)
        {
            this.set("allowEdit", value);
        }

        public object getAllowDelete()
        {
            return this.get("allowDelete");
        }

        public void setAllowDelete(object value)
        {
            this.set("allowDelete", value);
        }

        public object getAllowNew()
        {
            return this.get("allowNew");
        }

        public void setAllowNew(object value)
        {
            this.set("allowNew", value);
        }

        public object getFieldInfos()
        {
            return this.get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.get("title");
        }

        public void setTitle(object value)
        {
            this.set("title", value);
        }

        public object getName()
        {
            return this.get("name");
        }

        public void setName(object value)
        {
            this.set("name", value);
        }

        public object isReadOnly()
        {
            return this.get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.set("isReadOnly", value);
        }

    }

}
