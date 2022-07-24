using MongoDB.Driver;
using pressF.API.Interfaces;
using pressF.API.Model;
using pressF.API.Repository.Interfaces;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pressF.API.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly IMongoContext Context;
        protected IMongoCollection<TEntity> DbSet;

        protected BaseRepository(IMongoContext context)
        {
            Context = context;

            DbSet = Context.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public virtual void Add(TEntity obj)
        {
            if (obj is BaseDocument) (obj as BaseDocument).Id = MongoID.Create();
            if (obj is ITraceable) (obj as ITraceable).InsertDate = DateTimeOffset.Now;

            Context.AddCommand(() => DbSet.InsertOneAsync(obj));
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
            return data.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public virtual void Update(TEntity obj)
        {
            if (obj is ITraceable) (obj as ITraceable).UpdateDate = DateTimeOffset.Now;

            Context.AddCommand(() => DbSet.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", obj.GetId()), obj));
        }

        public virtual void Remove(string id)
        {
            var target = GetById(id).Result;
            if (target != null)
            {
                if (target is IRemoveable) { (target as IRemoveable).Excluded = true; (target as IRemoveable).ExcludedDate = DateTimeOffset.Now; Update(target); }
                else { Context.AddCommand(() => DbSet.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", id))); }
            }
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
