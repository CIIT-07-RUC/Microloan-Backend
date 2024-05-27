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

        public async Task SaveMessageAsync(string conversationId, string user, string message)
        {
            var messageData = $"{user}: {message}";
            await _db.ListRightPushAsync(conversationId, messageData);
        }

        public async Task<string[]> GetMessagesAsync(string conversationId)
        {
            return (await _db.ListRangeAsync(conversationId)).ToStringArray();
        }

        public async Task<string[]> GetMessagesByUser(string userId)
        {
            return (await _db.ListRangeAsync(userId)).ToStringArray();
        }
    }
}

