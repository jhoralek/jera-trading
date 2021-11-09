using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Linq;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class BidsRepository : BaseRepository<Bid>, IEntityRepository<Bid>
    {
        public BidsRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }

        protected override IQueryable<Bid> GetIncludedAll()
            => _context.Bids
                .Include(x => x.Record)
                .Include(x => x.User)
                .Include(x => x.User.Customer)
                .Include(x => x.User.Customer.Address)
                .Include(x => x.User.Customer.Address.Country);
    }
}
