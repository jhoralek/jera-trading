using AutoMapper;
using SA.Core.Model;
using System.Linq;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class GdprRecordsRepository : BaseRepository<GdprRecord>, IEntityRepository<GdprRecord>
    {
        public GdprRecordsRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }

        protected override IQueryable<GdprRecord> GetIncludedAll()
            => _context.GdprRecords;
    }
}
