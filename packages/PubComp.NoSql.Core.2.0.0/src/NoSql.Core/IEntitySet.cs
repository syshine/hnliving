using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubComp.NoSql.Core
{
    public interface IEntitySet
    {
        Type KeyType
        {
            get;
        }

        Type EntityType
        {
            get;
        }

        IQueryable<IEntity> AsQueryable();

        bool AddIfNotExists(IEntity entity);

        void AddOrUpdate(IEntity entity);

        IEntity GetOrAdd(IEntity entity);

        void Add(IEntity entity);

        void Add(IEnumerable<IEntity> entities);

        bool Contains(Object key);

        IEntity Get(Object key);

        IEnumerable<IEntity> Get(IEnumerable keys);

        void Update(IEntity entity);

        void Update(IEnumerable<IEntity> entities);

        void Delete(IEntity entity);

        void Delete(IEnumerable<IEntity> entities);

        void Delete(Object key);

        void Delete(IEnumerable<Object> keys);

#if DEBUG
        void DeleteAll();
#endif
    }

    public interface IEntitySet<TKey, TEntity> : IEntitySet where TEntity : class, IEntity<TKey>
    {
        new IQueryable<TEntity> AsQueryable();

        bool AddIfNotExists(TEntity entity);

        void AddOrUpdate(TEntity entity);

        TEntity GetOrAdd(TEntity entity);

        void Add(TEntity entity);

        void Add(IEnumerable<TEntity> entities);

        bool Contains(TKey key);

        TEntity Get(TKey key);

        IEnumerable<TEntity> Get(IEnumerable<TKey> keys);

        void Update(TEntity entity);

        void Update(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        void Delete(TKey key);

        void Delete(IEnumerable<TKey> keys);

        void LoadNavigation(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames);

        void SaveNavigation(IEnumerable<TEntity> entities, IEnumerable<string> propertyNames);

        event EventHandler<AccessEventArgs<TEntity>> OnModifying;

        event EventHandler<AccessEventArgs<TEntity>> OnDeleting;

        event EventHandler<AccessEventArgs<TEntity>> OnGetting;
    }

    public class AccessEventArgs<TEntity> : EventArgs
    {
        public readonly TEntity Entity;
        public bool CanAccess;

        public AccessEventArgs(TEntity entity)
        {
            Entity = entity;
            CanAccess = true;
        }
    }
}
