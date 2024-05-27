using System;
using Microsoft.AspNetCore.SignalR;

namespace MicroLoanAPI
{
	public class ChatHub : Hub
    {

        private readonly RedisCacheService _redisCacheService;

        public ChatHub(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }




        public async Task SendPrivateMessage(string groupName, string user, string message)
        {
            await _redisCacheService.SaveMessageAsync(groupName, user, message);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LoadPreviousMessages(string groupName)
        {
            var messages = await _redisCacheService.GetMessagesAsync(groupName);
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "", message);
            }
        }

    }
}

