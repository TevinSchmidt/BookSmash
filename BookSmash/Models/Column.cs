using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookSmash.Models
{
    /// <summary>
    /// This class represents a column in a table in a MySql database
    /// </summary>
    public partial class Column
    {
        /// <summary>
        /// Constructor for a column object
        /// </summary>
        /// <param name="name">The name of the column</param>
        /// <param name="type">The data type the column contains, as well as its relevent restrictions</param>
        /// <param name="mods">Any specific modidfications to the data, i.e. UNIQUE, NOT NULL, etc</param>
        /// <param name="primaryKey">Indicates if this column is a primary key</param>
        public Column(string name, string type, string[] mods, bool primaryKey, bool foreignKey, 
            string reference, int onUpdate, int onDelete)
        {
            this.name = name.ToLower();
            this.type = type;
            this.primaryKey = primaryKey;
            this.mods = mods;
            this.foreignKey = foreignKey;
            this.reference = reference;
            this.onUpdate = onUpdate;
            this.onDelete = onDelete;
        }

        /// <summary>
        /// Returns the structure of the column in a way that can be used in a
        /// CREATE table statement
        /// </summary>
        /// <returns>The structure of the column as a string</returns>
        public string getCreateStructure()
        {
            string structure = name + " " + type;
            if (mods != null)
            {
                foreach (string mod in mods)
                {
                    structure += " " + mod;
                }
            }
            return structure;
        }
    }

    /// <summary>
    /// This portion of the class contains the member variables
    /// </summary>
    public partial class Column
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public string name { get; }

        /// <summary>
        /// The data type of the column
        /// </summary>
        public string type { get; }

        /// <summary>
        /// The modfiers on the column, such as UNIQUE or NOT NULL
        /// May be null
        /// </summary>
        public string[] mods { get; }

        /// <summary>
        /// Represents whether or not this column is a primary key.
        /// </summary>
        public bool primaryKey { get; }

        /// <summary>
        /// Represents whether or not this column is a foreign key.
        /// </summary>
        public bool foreignKey { get; }

        /// <summary>
        /// The name of the column the foreign key references
        /// Must put null if none
        /// </summary>
        public string reference { get; }

        /// <summary>
        /// Integer to specify the action on update
        /// 1 = CASCADE
        /// 2 = SET NULL
        /// </summary>
        public int onUpdate { get;  }

        /// <summary>
        /// Integer to specify the action on delete
        /// 1 = CASCADE
        /// 2 = SET NULL
        /// </summary>
        public int onDelete { get; }
    }
}
