using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SA.Core.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SA.EntityFramework.EntityFramework.Repository
{
    public class RecordsRepository : BaseRepository<Record>, IEntityRepository<Record>
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public RecordsRepository(
            SaDbContext context,
            IHostingEnvironment hostingEnvironment, IMapper mapper) : base(context, mapper)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override async Task<Record> AddAsync(Record item)
        {
            var addedEntity = await base.AddAsync(item);

            if (addedEntity.Files.Any())
            {
                var root = _hostingEnvironment.WebRootPath;
                foreach (var file in addedEntity.Files)
                {
                    CopyFile(root, file);
                }
            }

            return addedEntity;
        }

        public override async Task<Record> RemoveAsync(int id)
        {
            var itemToDelte = await _context.Records
                .FirstOrDefaultAsync(x => x.Id == id);

            if (itemToDelte != null)
            {
                var deleted = _context.Records.Remove(itemToDelte);
                var root = _hostingEnvironment.WebRootPath;

                if (itemToDelte.Files.Any())
                {
                    var item = itemToDelte.Files.FirstOrDefault();
                    var dirPath = Path.Combine(root, $"{item.Path}/{item.RecordId}");
                    if (Directory.Exists(dirPath))
                    {
                        Directory.Delete(dirPath, true);
                    }
                }

                await _context.SaveChangesAsync();
                return deleted.Entity;
            }
            return null;
        }

        public override async Task<Record> UpdateAsync(Record item)
        {
            var root = _hostingEnvironment.WebRootPath;

            var itemToUpdate = await _context.Records.Include(x => x.Files)
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (itemToUpdate != null)
            {
                var filesForDelete = new List<Core.Model.File>();
                var filesForAdding = new List<Core.Model.File>();
                foreach (var file in itemToUpdate.Files)
                {
                    if (!item.Files.Select(x => x.Name).Contains(file.Name))
                    {
                        filesForDelete.Add(file);
                    }
                }

                foreach (var file in item.Files.Where(x => x.Id <= 0))
                {
                    if (!itemToUpdate.Files.Select(x => x.Name).Contains(file.Name))
                    {
                        filesForAdding.Add(file);
                    }
                }

                _mapper.Map(item, itemToUpdate);                

                if (filesForDelete.Any())
                {
                    foreach (var file in filesForDelete)
                    {
                        itemToUpdate.Files.Remove(file);
                        DeleteFile(root, file);
                    }
                }

                if (filesForAdding.Any())
                {
                    foreach (var file in filesForAdding)
                    {
                        itemToUpdate.Files.Add(file);
                        CopyFile(root, file);
                    }
                }

                await _context.SaveChangesAsync();

                return itemToUpdate;
            }
            return null;
        }

        protected override IQueryable<Record> GetIncludedAll()
            => _context.Records
                .Include(x => x.User)
                .Include(x => x.Bids)
                .Include(x => x.Files);

        private void CopyFile(string root, Core.Model.File file)
        {
            var tempFullPath = Path.Combine(root, $"tempFiles/{file.Name}");
            var targetPath = Path.Combine(root, $"{file.Path}/{file.RecordId}/images/");
            var destFullPath = Path.Combine(targetPath, file.Name);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            System.IO.File.Copy(tempFullPath, destFullPath, true);
        }

        private void DeleteFile(string root, Core.Model.File file)
        {
            var fileFullPath = Path.Combine(root, $"{file.Path}/{file.RecordId}/images/{file.Name}");

            if (System.IO.File.Exists(fileFullPath))
            {
                System.IO.File.Delete(fileFullPath);
            }
        }
    }
}
