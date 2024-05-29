using System;
using StackExchange.Redis;

namespace MicroLoanAPI
{
	public class RedisCacheService
	{
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task SaveMessageAsync(string conversationId, string user, string message, string dateTime)
        {
            var messageData = $"{message}|{dateTime}|{user}";
            await _db.ListRightPushAsync(conversationId, messageData);
        }

        public async Task<string[]> GetMessagesAsync(string conversationId)
        {
            return (await _db.ListRangeAsync(conversationId)).ToStringArray();
        }

        public async Task<string[]> GetConversationsByUserAsync(string userId)
        {
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            var allKeys = server.Keys().Where(key => key.ToString().Contains($"-{userId}") || key.ToString().Contains($"{userId}-"));

            var conversationUsers = new HashSet<string>();

            foreach (var key in allKeys)
            {
                var parts = key.ToString().Split('-');
                if (parts[0] == userId)
                {
                    conversationUsers.Add(parts[1]);
                }
                else if (parts[1] == userId)
                {
                    conversationUsers.Add(parts[0]);
                }
            }

            return conversationUsers.ToArray();
        }
    }
}

