using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public class FileSystemObject : NamedElement
    {
        public virtual string id
        {
            get;
            set;
        }

        public new virtual string name
        {
            get;
            set;
        }

        public virtual string relativePath
        {
            get;
            set;
        }

        public virtual string extension
        {
            get;
            set;
        }
    }
}
