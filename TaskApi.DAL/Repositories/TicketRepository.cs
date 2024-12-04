using Microsoft.EntityFrameworkCore;
using TaskApi.DAL.EF;
using TaskApi.DAL.Entities;
using TaskApi.DAL.Interfaces;

namespace TaskApi.DAL.Repositories
{
    public class TicketRepository : IRepository<Ticket, int>
    {
        private TicketDbContext _dbContext;
        private readonly IDateHelper _dateHelper;
        
        public TicketRepository(TicketDbContext dbContext, IDateHelper dateHelper)
        {
            _dbContext = dbContext;
            _dateHelper = dateHelper;
        }

        public async Task<Ticket?> GetAsync(int id)
        {
            return await _dbContext.Tickets
                .Include(x => x.TicketFiles)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task CreateAsync(Ticket item)
        {
            item.CreatedDate = _dateHelper.TodayDate;
            _dbContext.Tickets.Add(item);
        }

        public async Task<bool> DeleteAsync(ICollection<int> ids)
        {
            var toDelete = await _dbContext.Tickets.Where(x => ids.Contains(x.Id)).ToListAsync();
            if (toDelete != null) {
                _dbContext.Tickets.RemoveRange(toDelete);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<int> UpdateAsync(Ticket item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
            return item.Id;
        }

        public Task CreateAsync(ICollection<Ticket> items)
        {
            throw new NotImplementedException();
        }
    }
}
