using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace RiverLinkReporter.service
{

    #region IRepository
    public interface IRepository : IReadOnlyRepository
    {
        void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class;

        void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class;

        void MarkDeleted<TEntity>(object id, string modifiedBy = null) where TEntity : class;

        void Delete<TEntity>(object id) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Executes a SQL command with Parameters.  Only use this method if there is NO other option!!!  
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="SqlCommand">The SQL command.</param>
        /// <param name="Parameters">The parameters.</param>
        void ExcuteSQL<TEntity>(string SqlCommand, SqlParameter Parameters) where TEntity : class;


        void ExcuteSQL<TEntity>(string SqlCommand, SqlParameter[] Parameters) where TEntity : class;


        /// <summary>
        /// Excutes a SQL command. Never use with input from UI!!!  
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="SqlCommand">The SQL command.</param>
        void ExcuteSQL<TEntity>(string SqlCommand) where TEntity : class;

        void Save();

        Task SaveAsync();

    }
    #endregion IRepository

    public class EFRepository<TContext> : EFReadOnlyRepository<TContext>, IRepository where TContext : DbContext
    {
        public EFRepository(TContext context) : base(context)
        {
        }

        public virtual void Create<TEntity>(TEntity entity, string createdBy = null) where TEntity : class
        {
            context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity, string modifiedBy = null) where TEntity : class
        {
            context.Set<TEntity>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void MarkDeleted<TEntity>(object id, string modifiedBy = null) where TEntity : class
        {
            //get the entity
            TEntity entity = context.Set<TEntity>().Find(id);

            //make sure it's not null
            if (entity != null)
            {
                TrySetProperty(entity, "Deleted", true);
                TrySetProperty(entity, "LastModifiedByDate", DateTime.Now);

                if (modifiedBy != null)
                {
                    TrySetProperty(entity, "LastModifiedById", modifiedBy);
                }

                //update it
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual async Task MarkDeletedAsync<TEntity>(object id, string modifiedBy = null) where TEntity : class
        {
            //get the entity
            TEntity entity = await context.Set<TEntity>().FindAsync(id);

            //make sure it's not null
            if (entity != null)
            {
                TrySetProperty(entity, "Deleted", true);
                TrySetProperty(entity, "LastModifiedByDate", DateTime.Now);

                if (modifiedBy != null)
                {
                    TrySetProperty(entity, "LastModifiedById", modifiedBy);
                }

                //update it
                context.Set<TEntity>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            }
        }

        public virtual void Delete<TEntity>(object id) where TEntity : class
        {
            TEntity entity = context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="SqlCommand"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>

        //Only use this method if there is NO other option!!!
        public virtual void ExcuteSQL<TEntity>(string SqlCommand, SqlParameter Parameters) where TEntity : class
        {
            context.Database.ExecuteSqlCommand(SqlCommand, Parameters);
        }

        public virtual void ExcuteSQL<TEntity>(string SqlCommand, SqlParameter[] Parameters) where TEntity : class
        {
            context.Database.ExecuteSqlCommand(SqlCommand, Parameters);
        }

        //Never use with input from UI!!!
        public virtual void ExcuteSQL<TEntity>(string SqlCommand) where TEntity : class
        {
            context.Database.ExecuteSqlCommand(SqlCommand);
        }

        public virtual void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = context.Set<TEntity>();
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public virtual void Save()
        {
            try
            {

                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw (e);
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }

            return Task.FromResult(0);
        }

        private void TrySetProperty(object obj, string property, object value)
        {
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
                prop.SetValue(obj, value, null);
        }
    }
}