using ContactProMVC.Models;
using ContactProMVC.Interaces;

namespace ContactProMVC.Services
{
    public class AddressBookService : IAddressBookService
    {
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