using TaskApi.DAL.EF;
using TaskApi.DAL.Entities;
using TaskApi.DAL.Interfaces;

namespace TaskApi.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly TicketDbContext _db;
        private readonly IDateHelper _dateHelper;

        private TicketRepository ticketRepository;
        private TicketFileRepository ticketFileRepository;

        public EFUnitOfWork(TicketDbContext db, IDateHelper dateHelper)
        {
            _db = db;
            _dateHelper = dateHelper;
        }
        public IRepository<Ticket,int> Tickets
        {
            get
            {
                if (ticketRepository == null)
                    ticketRepository = new TicketRepository(_db, _dateHelper);
                return ticketRepository;
            }
        }

        public IRepository<TicketFile, Guid> TicketFiles
        {
            get
            {
                if (ticketFileRepository == null)
                    ticketFileRepository = new TicketFileRepository(_db);
                return ticketFileRepository;
            }
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
