using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory_Management_System_PC.Models
{
    public static class SessionManager
    {
        // Holds the logged-in user
        public static User LoggedInUser { get; private set; }

        // Stores the user after login
        public static void SetUser(User user)
        {
            LoggedInUser = user;
        }

        // Clears session on logout
        public static void Logout()
        {
            LoggedInUser = null;
        }
    }
}
