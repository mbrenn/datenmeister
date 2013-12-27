using BurnSystems.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BurnSystems.FlexBG.Modules.DeponNet.ResourceSetM
{
    /// <summary>
    /// Stores the container of resources
    /// </summary>
    [Serializable]
    public class ResourceSet
    {
        /// <summary>
        /// Stores an empty resourceset 
        /// </summary>
        private static ResourceSet empty = new ResourceSet();

        /// <summary>
        /// This container stores the resources
        /// </summary>
        private Dictionary<long, double> resources = new Dictionary<long, double>();

        /// <summary>
        /// Initializes a new instance of the ResourceSet class.
        /// </summary>
        public ResourceSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ResourceSet class.
        /// </summary>
        /// <param name="resources">Pair of resources to be set</param>
        public ResourceSet(IEnumerable<KeyValuePair<long, double>> resources)
        {
            foreach (var pair in resources)
            {
                this.resources[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ResourceSet class.
        /// One resourcetype will get a value
        /// </summary>
        /// <param name="resourceType">Resourcetype to be set</param>
        /// <param name="value">Value of the resource</param>
        public ResourceSet(int resourceType, double value)
        {
            this.resources[resourceType] = value;
        }

        /// <summary>
        /// Gets an empty resourceset
        /// </summary>
        public static ResourceSet Empty
        {
            get { return empty.Clone(); }
        }

        /// <summary>
        /// Gets the resources as a dictionary
        /// </summary>
        public Dictionary<long, double> Resources
        {
            get { return this.resources; }
        }

        /// <summary>
        /// Gets the number of different resourcetypes in this resourceset
        /// </summary>
        public int Count
        {
            get { return this.Resources.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether all value of all 
        /// resource items are positive or 0.0
        /// </summary>
        public bool AreAllResourcesPositive
        {
            get
            {
                foreach (var pair in this.resources)
                {
                    if (pair.Value < 0.0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all value of 
        /// all resource items are negative or 0.0
        /// </summary>
        public bool AreAllResourcesNegative
        {
            get
            {
                foreach (var pair in this.resources)
                {
                    if (pair.Value > 0.0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Gets only the positive resourceset values
        /// </summary>
        public ResourceSet OnlyPositive
        {
            get
            {
                return new ResourceSet(this.resources.Where(x => x.Value > 0));
            }
        }

        /// <summary>
        /// Gets only the negative resourceset values
        /// </summary>
        public ResourceSet OnlyNegative
        {
            get
            {
                return new ResourceSet(this.resources.Where(x => x.Value < 0));
            }
        }

        /// <summary>
        /// Gets or sets the amount of resources
        /// </summary>
        /// <param name="resourceType">Requested resource type</param>
        /// <returns>Amount of resources or null, if not set</returns>
        public double this[long resourceType]
        {
            get
            {
                double value;
                if (this.resources.TryGetValue(resourceType, out value))
                {
                    return value;
                }

                // Nichts gefunden => 0.0
                return 0.0;
            }

            set
            {
                this.resources[resourceType] = value;
            }
        }

        /// <summary>
        /// Gets or sets the amount of resources
        /// </summary>
        /// <param name="resourceType">Requested resource type</param>
        /// <returns>Amount of resources or null, if not set</returns>
        public double this[IHasId resourceType]
        {
            get
            {
                return this[resourceType.Id];
            }

            set
            {
                this[resourceType.Id] = value;
            }
        }

        /// <summary>
        /// Adds two resourceset and returns the sum
        /// </summary>
        /// <param name="o1">First summand</param>
        /// <param name="o2">Second summand</param>
        /// <returns>Summed up resources</returns>
        public static ResourceSet operator +(ResourceSet o1, ResourceSet o2)
        {
            var result = new ResourceSet();
            result.Set(o1);
            result.Add(o2);
            return result;
        }

        /// <summary>
        /// Subtracts two resourceset and returns the difference
        /// </summary>
        /// <param name="o1">Minuend for subtraction</param>
        /// <param name="o2">Subtrahend of minuend</param>
        /// <returns>Difference between the two resources</returns>
        public static ResourceSet operator -(ResourceSet o1, ResourceSet o2)
        {
            var result = o2.CloneScale(-1);
            result.Add(o1);
            return result;
        }

        /// <summary>
        /// Combines two predicates by an or operator. 
        /// This means, that the predicate will return true, if one of the predicates
        /// return true.
        /// </summary>
        /// <param name="pred1">First predicate</param>
        /// <param name="pred2">Second predicate</param>
        /// <returns>true, if one of the predicate returns true</returns>
        public static Predicate<int> GetOrPredicate(Predicate<long> pred1, Predicate<long> pred2)
        {
            return (x) => pred1(x) || pred2(x);
        }

        /// <summary>
        /// Combines three predicates by an or operator. 
        /// This means, that the predicate will return true, if one of the predicates
        /// return true.
        /// </summary>
        /// <param name="pred1">First predicate</param>
        /// <param name="pred2">Second predicate</param>
        /// <param name="pred3">Third predicate</param>
        /// <returns>true, if one of the predicate returns true</returns>
        public static Predicate<long> GetOrPredicate(
            Predicate<long> pred1,
            Predicate<long> pred2,
            Predicate<long> pred3)
        {
            return (x) => pred1(x) || pred2(x) || pred3(x);
        }

        /// <summary>
        /// Combines two predicates by an or operator. 
        /// This means, that the predicate will return true, if all predicates
        /// return true.
        /// </summary>
        /// <param name="pred1">First predicate</param>
        /// <param name="pred2">Second predicate</param>
        /// <returns>true, if all predicates returns true</returns>
        public static Predicate<int> GetAndPredicate(
            Predicate<int> pred1, 
            Predicate<int> pred2)
        {
            return (x) => pred1(x) && pred2(x);
        }

        /// <summary>
        /// Calculates the sum of objects to be converted to a resourceset
        /// </summary>
        /// <typeparam name="T">Type of items</typeparam>
        /// <param name="objects">Objects to be iterated</param>
        /// <param name="action">Function, which converts the object to a resourceset</param>
        /// <returns>Sum of all resources to be converted</returns>
        public static ResourceSet CalculateSum<T>(
            IEnumerable<T> objects,
            Func<T, ResourceSet> action)
        {
            return objects
                .Select(action)
                .Sum();
        }

        /// <summary>
        /// Returns the elementwise minimum values of both 
        /// resourcesets
        /// </summary>
        /// <param name="set1">First resourceset</param>
        /// <param name="set2">Second resourceset</param>
        /// <returns>Minimum resourceset</returns>
        public static ResourceSet Min(ResourceSet set1, ResourceSet set2)
        {
            var resource = new ResourceSet();

            foreach (var pair in set1.Resources)
            {
                resource[pair.Key] = Math.Min(set1[pair.Key], set2[pair.Key]);
            }

            return resource;
        }

        /// <summary>
        /// Clears all resources
        /// </summary>
        public void Clear()
        {
            this.Resources.Clear();
        }

        /// <summary>
        /// Fügt neue Rohstoffe hinzu
        /// </summary>
        /// <param name="resourceType">Type of resource to be added</param>
        /// <param name="value">Wert des Rohstoffs</param>
        public void AddResource(long resourceType, double value)
        {
            if (value != 0.0)
            {
                this[resourceType] += value;
            }
        }

        /// <summary>
        /// Scales the resources
        /// </summary>
        /// <param name="scaleFactor">Scaling factor</param>
        public void Scale(double scaleFactor)
        {
            foreach (var pair in new Dictionary<long, double>(this.resources))
            {
                if (pair.Value != 0.0)
                {
                    this[pair.Key] = pair.Value * scaleFactor;
                }
            }
        }

        /// <summary>
        /// Skaliert den Rohstoff elementweise
        /// </summary>
        /// <param name="factor">Factor for resources</param>
        public void Scale(ResourceSet factor)
        {
            foreach (var pair in new Dictionary<long, double>(this.resources))
            {
                this[pair.Key] = pair.Value * factor[pair.Key];
            }
        }

        /// <summary>
        /// Adds resources to the current resourceset
        /// </summary>
        /// <param name="resources">Resources to be added</param>
        public void Add(ResourceSet resources)
        {
            // Gleiche Instanz, Skalierung reicht
            if (this == resources)
            {
                resources.Scale(2);
                return;
            }

            foreach (var pair in resources.resources)
            {
                if (pair.Value == 0.0)
                {
                    continue;
                }

                this[pair.Key] += pair.Value;
            }
        }

        /// <summary>
        /// Changes a certain type of resources. This method is equivalent 
        /// to add. 
        /// </summary>
        /// <param name="resourceSet">Resources which should be added 
        /// or subtracted</param>
        public void Change(ResourceSet resourceSet)
        {
            this.Add(resourceSet);
        }

        /// <summary>
        /// Adds a certain type of resources
        /// </summary>
        /// <param name="resourceType">Resourcetype to be removed</param>
        /// <param name="amount">Amount of resources to be removed</param>
        public void Add(int resourceType, double amount)
        {
            this[resourceType] += amount;
        }

        /// <summary>
        /// Changes a certain type of resources
        /// </summary>
        /// <param name="resourceType">Resourcetype to be removed</param>
        /// <param name="amount">Amount of resources to be removed</param>
        public void Change(int resourceType, double amount)
        {
            this[resourceType] += amount;
        }

        /// <summary>
        /// Subtracts the resources
        /// </summary>
        /// <param name="resources">Resources to be subtracted</param>
        public void Subtract(ResourceSet resources)
        {
            // Gleiche Instanz, Skalierung reicht
            if (this == resources)
            {
                this.Scale(0);
                return;
            }

            foreach (var pair in resources.resources)
            {
                if (pair.Value == 0.0)
                {
                    continue;
                }

                this[pair.Key] -= pair.Value;
            }
        }

        /// <summary>
        /// Subtracts a certain type of resources
        /// </summary>
        /// <param name="resourceType">Resourcetype to be removed</param>
        /// <param name="amount">Amount of resources to be removed</param>
        public void Subtract(int resourceType, double amount)
        {
            this[resourceType] -= amount;
        }

        /// <summary>
        /// Checks whether the current resourceset has enough resources for 
        /// the given resources
        /// </summary>
        /// <param name="resources">Resourceset, which can be described
        /// as the cost</param>
        /// <returns>true, if this instance has enough resources</returns>
        public bool HasEnoughFor(ResourceSet resources)
        {
            foreach (var pair in resources.resources)
            {
                if (pair.Value <= 0)
                {
                    // Negative or null resources can be skipped
                    continue;
                }

                if (this[pair.Key] < pair.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks whether the current resourceset has enough resources for 
        /// the given resources
        /// </summary>
        /// <param name="resources">Resourceset, which can be described
        /// as the cost</param>
        /// <param name="predicate">Predicate, which filters the resources
        /// that shall be checked</param>
        /// <returns>true, if this instance has enough resources</returns>
        public bool HasEnoughFor(ResourceSet resources, Predicate<long> predicate)
        {
            foreach (var pair in resources.resources)
            {
                if (predicate(pair.Key))
                {
                    if (this[pair.Key] < pair.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the number of possible items that can be bought 
        /// by the current resourceset
        /// </summary>
        /// <param name="cost">Cost of one resource</param>
        /// <returns>Number of possible items</returns>
        public double GetNrOfPossibleItems(ResourceSet cost)
        {
            var result = Double.MaxValue;

            foreach (var pair in cost.resources)
            {
                if (pair.Value == 0 && this[pair.Key] == 0)
                {
                    continue;
                }

                if (pair.Value > 0)
                {
                    result = Math.Min(this[pair.Key] / pair.Value, result);
                }
                else
                {
                    result = double.PositiveInfinity;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the number of possible items that can be bought
        /// be this resourceset
        /// </summary>
        /// <param name="cost">Cost for one unit</param>
        /// <param name="predicate">Filter for resources to be regarded.
        /// If this instance is <c>null</c>, all resources will be regarded
        /// </param>
        /// <returns>Number of possible units</returns>
        public double GetNrOfPossibleItems(ResourceSet cost, Predicate<long> predicate)
        {
            if (predicate == null)
            {
                return this.GetNrOfPossibleItems(cost);
            }

            var result = 0.0;

            var found = cost.resources.Where(x => predicate(x.Key)).ToList();
            if (found.Count == 0)
            {
                return double.PositiveInfinity;
            }

            foreach (var pair in found)
            {
                if (pair.Value <= 0 && this[pair.Key] == 0)
                {
                    continue;
                }

                if (pair.Value > 0)
                {
                    result = Math.Max(this[pair.Key] / pair.Value, result);
                }
                else
                {
                    result = double.PositiveInfinity;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the required seconds for the production of one unit
        /// with the cost in <c>this</c>.
        /// </summary>
        /// <param name="productionResources">Available resources per second for 
        /// production</param>
        /// <returns>Required seconds.</returns>
        public double CalculateRequiredSeconds(ResourceSet productionResources)
        {
            return this.CalculateRequiredSeconds(productionResources, x => true);
        }

        /// <summary>
        /// Calculates the required seconds for the production of one unit
        /// with the cost in <c>this</c>.
        /// </summary>
        /// <param name="productionResources">Available resources per second for 
        /// production</param>
        /// <param name="filter">Filter for the important resources.</param>
        /// <returns>Required seconds.</returns>
        public double CalculateRequiredSeconds(ResourceSet productionResources, Predicate<long> filter)
        {
            if (filter == null)
            {
                filter = x => true;
            }

            var returnValue = 0.0;

            foreach (var pair in this.resources)
            {
                if (filter(pair.Key))
                {
                    double required = this[pair.Key];
                    double available = productionResources[pair.Key];

                    if (available == 0.0 && required == 0.0)
                    {
                        continue;
                    }

                    if (available == 0.0)
                    {
                        return double.MaxValue;
                    }

                    returnValue = Math.Max(
                        returnValue,
                        this[pair.Key] / productionResources[pair.Key]);
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Clones the resources
        /// </summary>
        /// <returns>Cloned resources</returns>
        public ResourceSet Clone()
        {
            var result = new ResourceSet();
            result.resources = new Dictionary<long, double>(this.Resources);
            return result;
        }

        /// <summary>
        /// Clones the resourceset
        /// </summary>
        /// <param name="predicate">Predicate, which determines which resources have to be cloned</param>
        /// <returns>Cloned resourceset</returns>
        public ResourceSet Clone(Predicate<long> predicate)
        {
            var result = new ResourceSet();
            foreach (var pair in this.resources)
            {
                if (predicate(pair.Key))
                {
                    result.resources[pair.Key] = pair.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Clones the resources and scales the result by a given factor
        /// </summary>
        /// <param name="scalingFactor">Scaling factor</param>
        /// <returns>Scaled result</returns>
        public ResourceSet CloneScale(double scalingFactor)
        {
            var result = new ResourceSet();

            foreach (var pair in this.resources)
            {
                if (pair.Value == 0.0)
                {
                    continue;
                }

                result[pair.Key] = scalingFactor * pair.Value;
            }

            return result;
        }

        /// <summary>
        /// Clones the resources and scales the result by a given factor 
        /// for each resourcetype
        /// </summary>
        /// <param name="factor">Scaling factor for each resourctype</param>
        /// <returns>Scaled result</returns>
        public ResourceSet CloneScale(ResourceSet factor)
        {
            var result = this.Clone();
            result.Scale(factor);
            return result;
        }

        /// <summary>
        /// Clones the resources and scales the result by a given factor
        /// </summary>
        /// <param name="scaleFactor">Scaling factor</param>
        /// <param name="predicate">Predicate, which defines the resources
        /// to be scaled</param>
        /// <returns>Scaled result</returns>
        public ResourceSet CloneScale(double scaleFactor, Predicate<long> predicate)
        {
            var result = new ResourceSet();

            foreach (var pair in this.resources)
            {
                if (predicate(pair.Key) && pair.Value != 0.0)
                {
                    result[pair.Key] = pair.Value * scaleFactor;
                }
            }

            return result;
        }

        /// <summary>
        /// Sets a resourcetype in the container
        /// </summary>
        /// <param name="resourceType">Type of resource to be set</param>
        /// <param name="value">Number of resources</param>
        public void SetResource(long resourceType, double value)
        {
            this[resourceType] = value;
        }

        /// <summary>
        /// Sets the resources to the given value. The former resources
        /// will be cleared
        /// </summary>
        /// <param name="newResources">The new resources to be set </param>
        public void Set(ResourceSet newResources)
        {
            this.resources.Clear();
            foreach (var pair in newResources.Resources)
            {
                this.Resources[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Checks whether this and the other resourceset have the 
        /// same content. This comparison itself is not optimized
        /// </summary>
        /// <param name="other">ResourceSet to be compared</param>
        /// <returns>true, if both resources are equal. </returns>
        public bool HasSameContent(ResourceSet other)
        {
            // One way
            foreach (var pair in this.Resources)
            {
                if (other[pair.Key] != pair.Value)
                {
                    return false;
                }
            }

            // Other way
            foreach (var pair in other.Resources)
            {
                if (this[pair.Key] != pair.Value)
                {
                    return false;
                }
            }

            // Kein Unterschied gefunden => true
            return true;
        }

        /// <summary>
        /// This function returns a specific property, which is accessed by name
        /// </summary>
        /// <param name="name">Name of requested property</param>
        /// <returns>Property behind this object</returns>
        public object GetProperty(int name)
        {
            return this[name];
        }

        /// <summary>
        /// This function has to execute a function and to return an object
        /// </summary>
        /// <param name="functionName">Name of function</param>
        /// <param name="parameters">Parameters for the function</param>
        /// <returns>Return of function</returns>
        public object ExecuteFunction(string functionName, IList<object> parameters)
        {
            return null;
        }

        /// <summary>
        /// Adds all resources of the resourceset
        /// </summary>
        /// <returns>Sum of all values</returns>
        public double Sum()
        {
            var result = 0.0;
            foreach (var pair in this.resources)
            {
                result += pair.Value;
            }

            return result;
        }

        /// <summary>
        /// Converts the resource set to string. 
        /// </summary>
        /// <returns>Joined string</returns>
        public override string ToString()
        {
            return StringManipulation.Join(
                this.Resources.Where(z => z.Value != 0.0),
                x => string.Format("{0}={1}", x.Key, x.Value),
                ", ");
        }

        /// <summary>
        /// Checks, if the resourceset is empty
        /// </summary>
        /// <returns>true, if all resources are 0.0</returns>
        public bool IsEmpty()
        {
            return this.resources.All(x => x.Value == 0.0);
        }

        /// <summary>
        /// Gets or sets a value indicating whether any of the resources are negative
        /// </summary>
        /// <returns>true, if negative</returns>
        public bool HasNegativeResources()
        {
            return this.resources.Any(x => x.Value < 0);
        }

        /// <summary>
        /// Sets all resource of this resourceset to the given maximum
        /// </summary>
        /// <param name="maximum">Resourceset which defines the maximum border</param>
        /// <param name="all">true, if all resource types of the the current resources set will be checked for maximum. 
        /// false, if only the resource type in maximum shall be checked</param>
        internal void ApplyMaximum(ResourceSet maximum, bool all)
        {
            if (all)
            {
                foreach (var pair in this.resources.ToList())
                {
                    this.resources[pair.Key] = Math.Min(pair.Value, maximum[pair.Key]);
                }
            }
            else
            {
                foreach (var pair in maximum.Resources)
                {
                    this.resources[pair.Key] = Math.Min(pair.Value, maximum[pair.Key]);
                }
            }
        }
    }
}
