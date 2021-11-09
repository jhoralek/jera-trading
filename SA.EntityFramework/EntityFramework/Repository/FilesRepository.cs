using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Linq;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class FilesRepository : BaseRepository<File>, IEntityRepository<File>
    {
        public FilesRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }

        protected override IQueryable<File> GetIncludedAll()
            => _context.Files
                .Include(x => x.User)
                .Include(x => x.Record);
    }
}
