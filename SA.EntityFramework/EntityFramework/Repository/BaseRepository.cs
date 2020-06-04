﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public abstract class BaseRepository<T> where T : Entity<int>
    {
        protected readonly SaDbContext _context;
        public SaDbContext Context { get { return _context; } }
        public BaseRepository(SaDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> AddAsync(T item)
        {
            item.Created = DateTime.Now;
            var added = await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public virtual async Task<T> RemoveAsync(int id)
        {
            var itemToDelte = await _context.FindAsync<T>(id);

            if (itemToDelte != null)
            {
                var deleted = _context.Remove<T>(itemToDelte);
                await _context.SaveChangesAsync();
                return deleted.Entity;
            }
            return null;
        }

        public virtual async Task<T> UpdateAsync(T item)
        {
            var itemToUpdate = await _context.FindAsync<T>(item.Id);

            if (itemToUpdate != null)
            {
                Mapper.Map(item, itemToUpdate);
                await _context.SaveChangesAsync();
                return itemToUpdate;
            }
            return null;
        }

        public virtual async Task<IEnumerable<TResult>> GetAllAsync<TResult, TOrder>(
            Expression<Func<T, bool>> query = null,
            Expression<Func<T, TOrder>> orderAsc = null,
            Expression<Func<T, TOrder>> orderDesc = null,
            int? take = null)
                where TResult : class
        {
            var request = query != null
                ? GetIncludedAll().Where(query)
                : GetIncludedAll();

            request = orderAsc != null
                ? request.OrderBy(orderAsc)
                : request;

            request = orderDesc != null
                ? request.OrderByDescending(orderDesc)
                : request;

            if (take.HasValue)
            {
                request = request.Take(take.Value);
            }

            return await request.ProjectTo<TResult>().ToListAsync();
        }

        public virtual async Task<TResult> GetOneAsync<TResult>(Expression<Func<T, bool>> query)
            where TResult : class
            => await GetIncludedAll().Where(query)
                    .ProjectTo<TResult>()
                    .FirstOrDefaultAsync();

        protected virtual IQueryable<T> GetIncludedAll()
            => throw new NotImplementedException();
    }
}
