﻿using core.Extensions.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace core.Extensions.Data.Repository
{
    public class EF<T, TKey> : IRepository<T, TKey> where T : Entity<TKey> where TKey : IEquatable<TKey>
    {
        private readonly AppDbContext _context;
        private DbSet<T> _collection;

        public EF(AppDbContext context)
        {
            _context = context;
            _collection = _context.Set<T>();
        }

        public IQueryable<T> List => _collection.AsNoTracking().AsQueryable();

        public T Find(TKey Id)
        {
            return List.SingleOrDefault(_ => _.Id.Equals(Id));
        }

        public IQueryable<T> Query(FormattableString command)
        {
            return _context.Set<T>().FromSql(command);
        }

        public void Add(T entity)
        {
            if (entity != null)
            {
                _collection.Add(entity);
                _context.SaveChanges();
                //entity.OnChange(EntityChangeEventContext<TKey>.ActionTypes.Create);
            }
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                _collection.Update(entity);
                _context.SaveChanges();
                //entity.OnChange(EntityChangeEventContext<TKey>.ActionTypes.Update);
            }
        }

        public void Merge(IEnumerable<T> entities)
        {
            if (entities != null && entities.Any())
            {
                var joined = _collection
                    .Join(entities, c => c.Id, e => e.Id, (c, e) => new { c, e })
                    .ToList();

                var toUpdate = joined
                    .Where(_ => !_.c.Equals(_.e)) // First fast check
                    .Where(_ => Newtonsoft.Json.JsonConvert.SerializeObject(_.c) != Newtonsoft.Json.JsonConvert.SerializeObject(_.e)) // Deeper comparison
                    .Select(_ => _.e);
                if (toUpdate != null && toUpdate.Any())
                    _collection.UpdateRange(toUpdate);

                var toAdd = entities.Except(joined.Select(_ => _.e), new EntityComparer<T, TKey>());
                if (toAdd != null && toAdd.Any())
                    _collection.AddRange(toAdd);                   

                _context.SaveChanges();
            }
        }

        public void Delete(T entity)
        {
            if (entity != null)
            {
                _collection.Remove(entity);
                _context.SaveChanges();
                //entity.OnChange(EntityChangeEventContext<TKey>.ActionTypes.Delete);
            }
        }

    }
}

