using System;
using Microsoft.AspNetCore.SignalR;

namespace MicroLoanAPI
{
	public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendSignal(string user, string signal)
        {
            await Clients.User(user).SendAsync("ReceiveSignal", signal);
        }
    }
}

