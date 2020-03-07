using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NDMS.DataAccess.Interfaces
{
    /// <summary>
    /// Defines a generic base repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T>:IDisposable
    {
        #region Method(s)
        /// <summary>
        /// Retrieves an entity based on ID
        /// </summary>
        /// <param name="id">Id of the entity to match</param>
        /// <returns>Entity with matching identifier</returns>
        T Get(int id);

        /// <summary>
        /// Retrieves all entities
        /// </summary>
        /// <returns>All entities available in the repository</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Adds or updates an existing entity
        /// </summary>
        /// <param name="entity">Entity to add/update</param>
        void AddOrUpdate(T entity);

        /// <summary>
        /// Updates only the selected properties of an entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="propertyExpression">Properties to update as lambda expressions</param>
        void Update(T entity, params Expression<Func<T, object>>[] propertyExpression);

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(T entity);

        /// <summary>
        /// Save all changes made to the repository
        /// </summary>
        void Save();
        #endregion
    }
}
