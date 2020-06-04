﻿using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Linq;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class AddressesRepository : BaseRepository<Address>, IEntityRepository<Address>
    {
        public AddressesRepository(SaDbContext context) : base(context)
        { }

        protected override IQueryable<Address> GetIncludedAll()
            => _context.Addresses.Include(x => x.Country);

    }
}
