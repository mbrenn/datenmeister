﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatenMeister.DataProvider
{
    public class ListWrapperReflectiveSequence<T> : ListReflectiveSequence<T>
    {
        /// <summary>
        /// Stores the list
        /// </summary>
        private IList<T> list;

        public ListWrapperReflectiveSequence(IList<T> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Gets the list from the wrapper
        /// </summary>
        /// <returns></returns>
        protected override IList<T> GetList()
        {
            return this.list;
        }
    }
}