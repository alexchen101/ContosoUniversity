using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ContosoUniversity.Hubs
{
    public class ChatHubs : Hub
    {
        public async Task SendMessage(string user,string msg)
        {
            await Clients.All.SendAsync("RM",user, msg);
        }
    }
}
