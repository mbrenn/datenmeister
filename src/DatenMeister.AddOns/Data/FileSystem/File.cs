﻿using DatenMeister.Entities.UML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.AddOns.Data.FileSystem
{
    public class File : FileSystemObject
    {
        public virtual int length
        {
            get;
            set;
        }
    }
}
