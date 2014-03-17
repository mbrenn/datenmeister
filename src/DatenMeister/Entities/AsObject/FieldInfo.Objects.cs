namespace BurnSystems.Test
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("comment");
        }

        public void setComment(object value)
        {
            this.Set("comment", value);
        }

        public object getTitle()
        {
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("width");
        }

        public void setWidth(object value)
        {
            this.Set("width", value);
        }

        public object getHeight()
        {
            return this.Get("height");
        }

        public void setHeight(object value)
        {
            this.Set("height", value);
        }

        public object getTitle()
        {
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("text");
        }

        public void setText(object value)
        {
            this.Set("text", value);
        }

        public object getClickUrl()
        {
            return this.Get("clickUrl");
        }

        public void setClickUrl(object value)
        {
            this.Set("clickUrl", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.Set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.Get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.Set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.Get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.Set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("allowEdit");
        }

        public void setAllowEdit(object value)
        {
            this.Set("allowEdit", value);
        }

        public object getAllowDelete()
        {
            return this.Get("allowDelete");
        }

        public void setAllowDelete(object value)
        {
            this.Set("allowDelete", value);
        }

        public object getAllowNew()
        {
            return this.Get("allowNew");
        }

        public void setAllowNew(object value)
        {
            this.Set("allowNew", value);
        }

        public object getShowColumnHeaders()
        {
            return this.Get("showColumnHeaders");
        }

        public void setShowColumnHeaders(object value)
        {
            this.Set("showColumnHeaders", value);
        }

        public object getAllowNewProperty()
        {
            return this.Get("allowNewProperty");
        }

        public void setAllowNewProperty(object value)
        {
            this.Set("allowNewProperty", value);
        }

        public object getFieldInfos()
        {
            return this.Get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.Set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.Get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.Set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.Get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.Set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
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
        public object Get(string propertyName)
        {
            return this.obj.Get(propertyName);
        }

        /// <summary>
        /// Gets all properties as key value pairs
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<DatenMeister.ObjectPropertyPair> GetAll()
        {
            return this.obj.GetAll();
        }

        /// <summary>
        /// Checks, if a certain property is set
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>true, if set</returns>
        public bool IsSet(string propertyName)
        {
            return this.obj.IsSet(propertyName);
        }

        /// <summary>
        /// Sets the value of the property 
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">Value to be set</param>
        public void Set(string propertyName, object value)
        {
            this.obj.Set(propertyName, value);
        }

        /// <summary>
        /// Unsets the property
        /// </summary>
        /// <param name="propertyName">Name of the property to be removed</param>
        public void Unset(string propertyName)
        {
            this.obj.Unset(propertyName);
        }

        /// <summary>
        /// Deletes this object and all composed elements
        /// </summary>
        public void Delete()
        {
            this.obj.Delete();
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
            return this.Get("allowEdit");
        }

        public void setAllowEdit(object value)
        {
            this.Set("allowEdit", value);
        }

        public object getAllowDelete()
        {
            return this.Get("allowDelete");
        }

        public void setAllowDelete(object value)
        {
            this.Set("allowDelete", value);
        }

        public object getAllowNew()
        {
            return this.Get("allowNew");
        }

        public void setAllowNew(object value)
        {
            this.Set("allowNew", value);
        }

        public object getFieldInfos()
        {
            return this.Get("fieldInfos");
        }

        public void setFieldInfos(object value)
        {
            this.Set("fieldInfos", value);
        }

        public void pushFieldInfo(object value)
        {
            var list = this.Get("fieldInfos") as System.Collections.IList ?? new System.Collections.Generic.List<object>();
            list.Add(value);
            this.Set("fieldInfos", list);
        }

        public object getStartInEditMode()
        {
            return this.Get("startInEditMode");
        }

        public void setStartInEditMode(object value)
        {
            this.Set("startInEditMode", value);
        }

        public object getTitle()
        {
            return this.Get("title");
        }

        public void setTitle(object value)
        {
            this.Set("title", value);
        }

        public object getName()
        {
            return this.Get("name");
        }

        public void setName(object value)
        {
            this.Set("name", value);
        }

        public object isReadOnly()
        {
            return this.Get("isReadOnly");
        }

        public void setReadOnly(object value)
        {
            this.Set("isReadOnly", value);
        }

    }

}
