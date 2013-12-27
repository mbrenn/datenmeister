using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.DeponNet.MapM
{
    /// <summary>
    /// Enumerates and stores the possible field types
    /// </summary>
    public class FieldType : IHasId
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        public byte IdAsByte
        {
            get { return (byte)this.Id; }
        }

        /// <summary>
        /// Gets or sets the Token that will be used for messages
        /// </summary>
        public string Token
        {
            get;
            set;
        }
    }
}
