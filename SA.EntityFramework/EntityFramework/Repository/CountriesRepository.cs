using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class CountriesRepository : BaseRepository<Country>, IEntityRepository<Country>
    {
        public CountriesRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }

        // TODO: find out why this is here
        public async Task UpdateAsync(int id, Country item)
        {
            var itemToUpdate = await _context.Countries
                .FirstOrDefaultAsync(x => x.Id == id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Name = item.Name;
                itemToUpdate.Language = item.Language;

                await _context.SaveChangesAsync();
            }
        }

        protected override IQueryable<Country> GetIncludedAll()
            => _context.Countries;
    }
}
