using AutoMapper;
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
        protected readonly IMapper _mapper;

        public SaDbContext Context { get { return _context; } }
        public BaseRepository(SaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public virtual async Task<T> AddAsync(T item)
        {
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
                _mapper.Map(item, itemToUpdate);
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

            return await _mapper.ProjectTo<TResult>(request).ToListAsync();
        }

        public virtual async Task<TResult> GetOneAsync<TResult>(Expression<Func<T, bool>> query)
            where TResult : class
            => await _mapper.ProjectTo<TResult>(GetIncludedAll().Where(query)).FirstOrDefaultAsync();
                    
        protected virtual IQueryable<T> GetIncludedAll()
            => throw new NotImplementedException();
    }
}
