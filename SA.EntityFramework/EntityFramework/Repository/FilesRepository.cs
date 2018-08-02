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
    public class FilesRepository : IEntityRepository<File>
    {
        private readonly SaDbContext _context;
        public FilesRepository(SaDbContext context)
        {
            _context = context;
        }

        public async Task<File> AddAsync(File item)
        {
            item.Created = DateTime.Now;
            var added = await _context.Files.AddAsync(item);
            await _context.SaveChangesAsync();
            return added.Entity;
        }

        public async Task<File> RemoveAsync(int id)
        {
            var itemToDelte = await _context.Files
                .FirstOrDefaultAsync(x => x.Id == id);

            if (itemToDelte != null)
            {
                var deleted = _context.Files.Remove(itemToDelte);
                await _context.SaveChangesAsync();
                return deleted.Entity;
            }
            return null;
        }

        public async Task<File> UpdateAsync(File item)
        {
            var itemToUpdate = await _context.Files
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (itemToUpdate != null)
            {
                Mapper.Map(item, itemToUpdate);
                await _context.SaveChangesAsync();
                return itemToUpdate;
            }
            return null;
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult, TOrder>(
            Expression<Func<File, bool>> query = null,
            Expression<Func<File, TOrder>> order = null)
                where TResult : class
        {
            var request = query != null
                ? GetIncludedAll().Where(query)
                : GetIncludedAll();

            request = order != null
                ? request.OrderBy(order)
                : request;

            return await request.ProjectTo<TResult>().ToListAsync();
        }

        public async Task<TResult> GetOneAsync<TResult>(Expression<Func<File, bool>> query)
            where TResult : class
            => await GetIncludedAll().Where(query)
                    .ProjectTo<TResult>()
                    .FirstOrDefaultAsync();

        private IQueryable<File> GetIncludedAll()
            => _context.Files
                .Include(x => x.User)
                .Include(x => x.Record);
    }
}