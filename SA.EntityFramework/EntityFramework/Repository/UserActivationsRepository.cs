using AutoMapper;
using SA.Core.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class UserActivationsRepository : BaseRepository<UserActivation>, IEntityRepository<UserActivation>
    {
        public UserActivationsRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }

        public override async Task<UserActivation> UpdateAsync(UserActivation item)
        {
            item.IsUsed = item.IsUsed;
            item.Verified = DateTime.Now;

            return await base.UpdateAsync(item);
        }

        protected override IQueryable<UserActivation> GetIncludedAll()
            => _context.UserActivations;
    }
}
