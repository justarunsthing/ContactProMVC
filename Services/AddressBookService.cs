using ContactProMVC.Models;
using ContactProMVC.Interaces;
using ContactProMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace ContactProMVC.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly ApplicationDbContext _context;

        public AddressBookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddContactToCategoryAsync(int categoryId, int contactId)
        {
            try
            {
                if (!await IsContactOnCategoryAsync(categoryId, contactId))
                {
                    Contact? contact = await _context.Contacts.FindAsync(contactId);
                    Category? category = await _context.Categories.FindAsync(categoryId);

                    if (contact != null && category != null)
                    {
                        category.Contacts.Add(contact);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<ICollection<Category>> GetContactCategoriesAsync(int contactId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<int>> GetContactCategoriesIdsAsync(int contactId)
        {
            try
            {
                var contact = await _context.Contacts
                                            .Include(c => c.Categories)
                                            .FirstOrDefaultAsync(c => c.Id == contactId);

                List<int> categoryIds = contact.Categories.Select(c => c.Id).ToList();

                return categoryIds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetUserCategoriesAsync(string userId)
        {
            var categories = new List<Category>();

            try
            {
                categories = await _context.Categories.Where(c => c.AppUserId == userId)
                                                      .OrderBy(c => c.Name)
                                                      .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }

            return categories;
        }

        public async Task<bool> IsContactOnCategoryAsync(int categoryId, int contactId)
        {
            Contact? contact = await _context.Contacts.FindAsync(contactId);

            return await _context.Categories.Include(c => c.Contacts)
                                            .Where(c => c.Id == categoryId && c.Contacts.Contains(contact))
                                            .AnyAsync();
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