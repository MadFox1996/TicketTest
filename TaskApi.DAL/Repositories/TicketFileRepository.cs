using Microsoft.EntityFrameworkCore;
using TaskApi.DAL.EF;
using TaskApi.DAL.Entities;
using TaskApi.DAL.Interfaces;

namespace TaskApi.DAL.Repositories
{
    public class TicketFileRepository : IRepository<TicketFile, Guid>
    {
        private TicketDbContext _dbContext;

        public TicketFileRepository(TicketDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(ICollection<TicketFile> items)
        {
            if (items != null)
                await _dbContext.AddRangeAsync(items);
        }

        public async Task CreateAsync(TicketFile item)
        {
            _dbContext.TicketFiles.Add(item);
        }

        public async Task<bool> DeleteAsync(ICollection<Guid> guids)
        {
            var toDelete = await _dbContext.TicketFiles.Where(x => guids.Contains(x.Id)).ToListAsync();
            if (toDelete != null)
            {
                _dbContext.TicketFiles.RemoveRange(toDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Task<IEnumerable<TicketFile>> GetAsync(int pageNumber, int pageSize, bool orderByDesc)
        {
            throw new NotImplementedException();
        }

        public Task<TicketFile?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(TicketFile item)
        {
            throw new NotImplementedException();
        }
    }
}
