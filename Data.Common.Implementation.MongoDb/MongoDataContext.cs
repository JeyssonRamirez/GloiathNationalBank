using Core.Entities;
using Data.Common.Definition;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Configuration;

namespace Data.Common.Implementation.MongoDb
{

    public class MongoDataContext : UnitOfWorkMongo, IUnitOfWork
    {
        string _mongoServerUrl;

        string _mongoDbName;
        MongoClient _client;

        //private readonly IConfiguration _config;
        public MongoDataContext()
        {
       
            _mongoServerUrl = ConfigurationManager.ConnectionStrings["DBCrawler"].ConnectionString;
            _mongoDbName = ConfigurationManager.AppSettings["DBName"].ToString();
            _client = new MongoClient(_mongoServerUrl);
        }

        public MongoDatabase GetDatabase() { return _client.GetServer().GetDatabase(_mongoDbName); }

        public void DropDatabase(string dbName)
        {
            var server = _client.GetServer();
            server.DropDatabase(dbName);
        }

        public void DropCollection<T>() where T : Entity
        {
            var database = GetDatabase();
            var collectionName = typeof(T).Name.ToLower();

            if (database.CollectionExists(collectionName))
            {
                database.DropCollection(collectionName);
            }
        }
    }
}
