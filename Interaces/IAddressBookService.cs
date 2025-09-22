using ContactProMVC.Models;

namespace ContactProMVC.Interaces
{
    public interface IAddressBookService
    {
        Task AddContactToCategoryAsync(int categoryId, int contactId);
        Task<bool> IsContactOnCategoryAsync(int categoryId, int contactId);
        Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId);
        Task<ICollection<int>> GetContactCategoriesIdsAsync(int contactId);
        Task<ICollection<Category>> GetContactCategoriesAsync(int contactId);
        Task RemoveContactFromCategoryAsync(int categoryId, int contactId);
        IEnumerable<Contact> SearchContacts(string searchString, string userId);
    }
}