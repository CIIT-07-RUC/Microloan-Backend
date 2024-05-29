using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace MicroLoanAPI
{
	public class ChatHub : Hub
    {

        private readonly RedisCacheService _redisCacheService;

        public ChatHub(RedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }


        private string GenerateGroupName(string userId1, string userId2)
        {
            var userIds = new List<string> { userId1, userId2 };
            userIds.Sort();
            return string.Join("-", userIds);
        }


        public async Task SendPrivateMessage(string userId1, string userId2, string user, string message, string dateTime)
        {
            string groupName = GenerateGroupName(userId1, userId2);
            await _redisCacheService.SaveMessageAsync(groupName, user, message, dateTime);
            await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
        }

        public async Task AddToGroup(string userId1, string userId2 )
        {
            string groupName = GenerateGroupName(userId1, userId2);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LoadPreviousMessages(string userId1, string userId2)
        {
            string groupName = GenerateGroupName(userId1, userId2);
            var messages = await _redisCacheService.GetMessagesAsync(groupName);
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "", message);
            }
        }



        public async Task<string[]> GetConversationsByUser(string userId)
        {
            return await _redisCacheService.GetConversationsByUserAsync(userId);
        }


    }
}

