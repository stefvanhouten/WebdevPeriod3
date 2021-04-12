using WebdevPeriod3.Areas.Identity.Entities;

namespace WebdevPeriod3.Models
{
    public class UserTableRowModel
    {
        public User User { get; set; }
        public bool IsAdmin { get; set; }
        public bool DisableCheckbox { get; set; }

        public UserTableRowModel(User user, bool isAdmin, bool disableCheckbox)
        {
            User = user;
            IsAdmin = isAdmin;
            DisableCheckbox = disableCheckbox;
        }
    }
}
