using FileManager.Core.Data.Repositories;
using FileManager.Core.Entities;

namespace FileManager.Data.Repositories
{
    public class ContactRepository : BaseRepository<Contact>, IContactRepository
    {
        public ContactRepository(IDbContext context) : base (context)
        {
        }
    }
}
