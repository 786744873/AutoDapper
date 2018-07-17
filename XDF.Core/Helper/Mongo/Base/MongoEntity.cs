using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace XDF.Core.Helper.Mongo.Base
{
    public class MongoEntity
    {
        public MongoEntity()
        {
            Id = Guid.NewGuid().ToString("N");
        }

        [BsonElement("_id")]
        public string Id { get; set; }
    }
}
