using System;
using System.Runtime.Serialization;
namespace NDMS.DomainModel
{
    /// <summary>
    /// Base class to hold all the common features of the entities.
    /// All business entities must inherit from BaseEntity
    /// </summary>
    [Serializable]
    public abstract class BaseEntity
    {
        /// <summary>
        /// Identifier of the entity
        /// </summary>
        public int Id { get; set; }                

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var task = (BaseEntity)obj;
            return Id.Equals(task.Id);
        }

        /// <summary>
        /// Returns the hash code of the object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
