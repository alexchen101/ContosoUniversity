using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace ContosoUniversity.Pages.Chats
{
    public class IndexModel : PageModel
    {
        public string MyMessage { get; set; }

        public string UserID {
            get { return "user" + (new Random().Next(1, 1000000)); }
        }
    }
}
