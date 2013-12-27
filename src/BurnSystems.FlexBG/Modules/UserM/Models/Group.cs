using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnSystems.FlexBG.Modules.UserM.Models
{
    [Serializable]
    public class Group
    {
        /// <summary>
        /// Stores the Id of token for the administrator group
        /// </summary>
        public readonly static Guid AdministratorsToken = Guid.Parse(
            "A6DA521C-229F-41A0-B247-B28A979504F6");

        /// <summary>
        /// Stores the id
        /// </summary>
        private long id;

        /// <summary>
        /// Stores the title
        /// </summary>
        private string name;

        /// <summary>
        /// Stores the id of the token
        /// </summary>
        private Guid tokenId;

        /// <summary>
        /// Initializes a new instance of the Group class
        /// </summary>
        public Group()
        {
            this.TokenId = Guid.NewGuid();
        }

        /// <summary>
        /// Gets or sets the id of the group
        /// </summary>
        public long Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the title of the 
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Stores the id of the token
        /// </summary>
        public Guid TokenId
        {
            get { return this.tokenId; }
            set { this.tokenId = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
