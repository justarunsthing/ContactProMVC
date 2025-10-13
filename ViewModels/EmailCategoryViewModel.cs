using ContactProMVC.Models;

namespace ContactProMVC.ViewModels
{
    public class EmailCategoryViewModel
    {
        public List<Contact>? Contacts { get; set; }
        public EmailData? EmailData { get; set; }
    }
}