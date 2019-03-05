﻿using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.UIMetaData.ListView;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract
{
    public interface IDataAccessService
    {
        ResultModel Add(string metaObjectCode, BsonDocument bsons);
        ResultModel Add(MetaObject metaObject, BsonDocument bsons);

        ResultModel BatchAdd(string metaObjectCode, List<BsonDocument> bsons);
        ResultModel BatchAdd(MetaObject metaObject, List<BsonDocument> bsons);

        ResultModel Update(int metaObjectId, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        ResultModel Update(string metaObjectCode, FilterDefinition<BsonDocument> condition, BsonDocument bsons);
        ResultModel Update(MetaObject metaObject, FilterDefinition<BsonDocument> condition, BsonDocument bsons);

        ResultModel Delete(int metaObjectId, FilterDefinition<BsonDocument> condition);
        ResultModel Delete(string metaObjectCode, FilterDefinition<BsonDocument> condition);
        ResultModel Delete(MetaObject metaObject, FilterDefinition<BsonDocument> condition);

        BsonDocument Get(string metaObjectCode, FilterDefinition<BsonDocument> condition);
        BsonDocument Get(int metaObjectId, FilterDefinition<BsonDocument> condition);
        BsonDocument Get(MetaObject metaObject, FilterDefinition<BsonDocument> condition);
        SingleObjectComponent GetSingleObjectComponent(int metaObjectId, int InterfaceFieldId, FilterDefinition<BsonDocument> condition);

        BsonDocument GetById(string metaObjectCode, string _id);
        List<BsonDocument> GetByIds(string metaObjectCode, string[] _ids, SortDefinition<BsonDocument> sort);
        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, SortDefinition<BsonDocument> sort);

        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort);
        List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort);
        List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort);

        List<BsonDocument> GetList(int metaObjectId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);
        List<BsonDocument> GetList(string metaObjectCode, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);
        List<BsonDocument> GetList(MetaObject metaObject, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);
        TableListComponent GetTableListComponent(int metaObjectId, int InterfaceFieldId, FilterDefinition<BsonDocument> condition, int pageIndex, int pageSize, SortDefinition<BsonDocument> sort, out int count);

        int GetCount(int metaObjectId, FilterDefinition<BsonDocument> condition);
        int GetCount(string metaObjectCode, FilterDefinition<BsonDocument> condition);
        int GetCount(MetaObject metaObject, FilterDefinition<BsonDocument> condition);
    }
}
