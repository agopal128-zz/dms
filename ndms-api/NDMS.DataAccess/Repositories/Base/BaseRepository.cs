using NDMS.DataAccess.Interfaces;
using NDMS.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace NDMS.DataAccess.Repositories
{
    /// <summary>
    /// Implements a generic base repository
    /// </summary>
    /// <typeparam name="T">Type parameter for the entity</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Constructor(s)
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseRepository()
        {
            this.Context = new NDMSDataContext();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="context">Database Context</param>
        public BaseRepository(NDMSDataContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="disableAutoDetectChanges">Flag which says whether "AutoDetectChangesEnabled" is
        /// enabled or disabled for the DbContext</param>
        public BaseRepository(bool disableAutoDetectChanges)
        {
            this.Context = new NDMSDataContext(disableAutoDetectChanges);
        }
        #endregion

        #region Propertie(s)
        /// <summary>
        /// DBContext associated with this repository
        /// </summary>
        public NDMSDataContext Context
        {
            get;
            protected set;
        }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Retrieves an entity based on ID
        /// </summary>
        /// <param name="id">Id of the entity to match</param>
        /// <returns>Entity with matching identifier</returns>
        public T Get(int id)
        {
            return Context.Set<T>().Find(id);
        }

        /// <summary>
        /// Retrieves all entities
        /// </summary>
        /// <returns>All entities available in the repository</returns>
        public IQueryable<T> GetAll()
        {
            return Context.Set<T>();
        }

        /// <summary>
        /// Adds or updates an existing entity
        /// </summary>
        /// <param name="entity">Entity to add/update</param>
        public void AddOrUpdate(T entity)
        {
            // Assuming that new entity always has ID as zero
            Context.Entry(entity).State = ((BaseEntity)entity).Id == 0 ?
                    EntityState.Added : EntityState.Modified;
        }

        /// <summary>
        /// Updates only the selected properties of an entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        /// <param name="propertyExpression">Properties to update as lambda expressions</param>
        public void Update(T entity, params Expression<Func<T, object>>[] propertyExpression)
        {
            //turning off Entity validation so that partial update is through without validations
            Context.Configuration.ValidateOnSaveEnabled = false;
            Context.Set<T>().Attach(entity);
            var entry = Context.Entry(entity);
            //Iterate and mark the requested properties as modified
            foreach (var expression in propertyExpression)
            {
                entry.Property(expression).IsModified = true;
            }
        }

        /// <summary>
        /// Removes an entity
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        /// <summary>
        /// Save all changes made to the repository
        /// </summary>
        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                     .SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message,
                    " The validation errors are: ", fullErrorMessage);
                // Throw a new DataAccessException with the improved exception message.
                throw new DataAccessException(exceptionMessage, ex);
            }
            catch (Exception exception)
            {
                var exceptionTypes = new Type[]
                   {
                        typeof(DbUpdateException), typeof(DbUpdateConcurrencyException),
                        typeof(NotSupportedException), typeof(ObjectDisposedException),
                        typeof(InvalidOperationException)
                   };
                // if not of the expected exceptions, re throw 
                if (exceptionTypes.Contains(exception.GetType()))
                {
                    throw new DataAccessException(exception.Message, exception.InnerException);
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
