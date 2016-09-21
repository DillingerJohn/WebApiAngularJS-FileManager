using FileManager.Core.Data;
using FileManager.Core.Entities;
using FileManager.Core.Services;

namespace FileManager.Services.Services
{
    public class ContactService : BaseService<Contact>, IContactService
    {
        public ContactService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
