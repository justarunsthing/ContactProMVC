using ContactProMVC.Models;
using ContactProMVC.Interaces;
using ContactProMVC.Data;

namespace ContactProMVC.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly ApplicationDbContext _context;

        public AddressBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddContactToCategoryAsync(int categoryId, int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Category>> GetContactCategoriesAsync(int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<int>> GetContactCategoriesIdsAsync(int contactId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsContactOnCategoryAsync(int categoryId, int contactId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveContactFromCategoryAsync(int categoryId, int contactId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> SearchContacts(string searchString, string userId)
        {
            throw new NotImplementedException();
        }
    }
}