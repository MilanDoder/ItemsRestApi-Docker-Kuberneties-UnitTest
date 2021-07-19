using Catalog.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Repositories
{
    public class MongoDbPlayersRepository : IPlayerRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "players";

        private readonly IMongoCollection<Player> playersCollection;
        private readonly FilterDefinitionBuilder<Player> filterBuilder = Builders<Player>.Filter;

        public MongoDbPlayersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);

            playersCollection = database.GetCollection<Player>(collectionName);
        }

        public async Task CreatePlayerAsync(Player item)
        {
            await playersCollection.InsertOneAsync(item);
        }

        public async Task DeletePlayerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            await playersCollection.DeleteOneAsync(filter);
        }

        public async Task<Player> GetPlayerAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item => item.Id, id);
            return await playersCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayersAsync()
        {
            return await playersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task UpdatePlayerAsync(Player item)
        {
            var filter = filterBuilder.Eq(existingItem => existingItem.Id, item.Id);
            await playersCollection.ReplaceOneAsync(filter, item);
        }
    }
}
