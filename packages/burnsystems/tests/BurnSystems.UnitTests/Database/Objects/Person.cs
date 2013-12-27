using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BurnSystems.Database.Objects;

namespace BurnSystems.UnitTests.Database.Objects
{
    /// <summary>
    /// Enumeration for sex of person
    /// </summary>
    public enum Sex
    {
        /// <summary>
        /// Used for male persons
        /// </summary>
        Male,
        
        /// <summary>
        /// Used for female persons
        /// </summary>
        Female
    }

    /// <summary>
    /// This class is used as an example type for O/R-Mapper
    /// </summary>
    [DatabaseClass]
    public class Person
    {

        [DatabaseKey("Id")]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DatabaseProperty("Prename")]
        public string Prename
        {
            get;
            set;
        }

        [DatabaseProperty("Name")]
        public string Name
        {
            get;
            set;
        }

        [DatabaseProperty("Age")]
        public int Age_Temp
        {
            get;
            set;
        }

        [DatabaseProperty("Weight")]
        public double Weight
        {
            get;
            set;
        }

        [DatabaseProperty("Sex")]
        public Sex Sex
        {
            get;
            set;
        }

        public string Obsolete
        {
            get;
            set;
        }

        [DatabaseProperty("Marriage")]
        public DateTime Marriage
        {
            get;
            set;
        }
    }
}
