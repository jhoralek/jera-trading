using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class UsersRepository : BaseRepository<User>, IEntityRepository<User>
    {
        
        public UsersRepository(SaDbContext context, IMapper mapper) : base(context, mapper)
        { }      

        // TODO: find out what is setting up and what not
        public override async Task<User> UpdateAsync(User item)
        {
            var itemToUpdate = await _context.Users
                .Include(x => x.Records)
                .Include(x => x.Bids)
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate.IsActive = item.IsActive;
                itemToUpdate.SendingNews = item.SendingNews;
                itemToUpdate.Token = item.Token;
                itemToUpdate.Password = item.Password;
                itemToUpdate.Language = item.Language;
                _context.Entry(itemToUpdate).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return itemToUpdate;
            }
            return null;
        }

        protected override IQueryable<User> GetIncludedAll()
            => _context.Users
                .Include(x => x.Customer)
                .Include(x => x.Customer.Address)
                .Include(x => x.Customer.Address.Country);
    }
}
