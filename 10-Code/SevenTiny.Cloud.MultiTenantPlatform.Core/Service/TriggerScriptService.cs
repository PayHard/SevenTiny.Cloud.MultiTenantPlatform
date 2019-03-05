﻿using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.Infrastructure.Caching;
using System;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class TriggerScriptService : MetaObjectManageRepository<TriggerScript>, ITriggerScriptService
    {
        public TriggerScriptService(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
        }

        readonly MultiTenantPlatformDbContext dbContext;

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="triggerScript"></param>
        public new Result Update(TriggerScript triggerScript)
        {
            TriggerScript myfield = GetById(triggerScript.Id);
            if (myfield != null)
            {
                //编码不允许修改
                //脚本类型不允许修改

                //如果脚本有改动，则清空脚本缓存
                if (!myfield.Script.Equals(triggerScript.Script))
                    TriggerScriptCache.ClearCache(triggerScript.Script);

                myfield.Script = triggerScript.Script;
                myfield.FailurePolicy = triggerScript.FailurePolicy;

                myfield.Name = triggerScript.Name;
                myfield.Group = triggerScript.Group;
                myfield.SortNumber = triggerScript.SortNumber;
                myfield.Description = triggerScript.Description;
                myfield.ModifyBy = -1;
                myfield.ModifyTime = DateTime.Now;
            }
            base.Update(myfield);
            return Result.Success();
        }

        public List<TriggerScript> GetTriggerScriptsUnDeletedByMetaObjectIdAndScriptType(int metaObjectId, int scriptType)
        {
            return dbContext.QueryList<TriggerScript>(t => t.MetaObjectId == metaObjectId && t.ScriptType == scriptType);
        }

        public string GetDefaultTriggerScriptByScriptType(int scriptType)
        {
            switch ((ScriptType)scriptType)
            {
                case ScriptType.Add_Before: return DefaultAddBeforeTriggerScript;

                case ScriptType.Update_Before:
                case ScriptType.Delete_Before:
                case ScriptType.TableList_Before:
                case ScriptType.SingleObject_Before:
                case ScriptType.Count_Before: return DefaultQueryBeforeTriggerScript;

                case ScriptType.TableList_After: return DefaultTableListAfterTriggerScript;
                case ScriptType.SingleObject_After: return DefaultSingleObjectAfterTriggerScript;
                case ScriptType.Count_After: return DefaultCountAfterTriggerScript;
                default: return null;
            }
        }

        public string GetDefaultTriggerScriptDataSourceScript() => DefaultDataSourceTriggerScript;

        private string DefaultQueryBeforeTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public FilterDefinition<BsonDocument> QueryBefore(string operateCode,FilterDefinition<BsonDocument> condition)
{
	//这里写业务逻辑
	//...
	return condition;
}
";
        private string DefaultBatchAddBeforeTriggerScript
    => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug()
public List<BsonDocument> BatchAddBefore(string operateCode,List<BsonDocument> bsonElementsList)
{
	//这里写业务逻辑
	//...
	return bsonElementsList;
}
";
        private string DefaultAddBeforeTriggerScript
    => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public BsonDocument AddBefore(string operateCode,BsonDocument bsonElements)
{
	//这里写业务逻辑
	//...
	return bsonElements;
}
";
        private string DefaultTableListAfterTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public TableListComponent TableListAfter(string operateCode,TableListComponent tableListComponent)
{
	//这里写业务逻辑
	//...
	return tableListComponent;
}
";
        private string DefaultSingleObjectAfterTriggerScript
            => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
using MongoDB.Bson;
using MongoDB.Driver;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public SingleObjectComponent SingleObjectAfter(string operateCode,SingleObjectComponent singleObjectComponent)
{
	//这里写业务逻辑
	//...
	return singleObjectComponent;
}
";
        private string DefaultCountAfterTriggerScript
            => @"
using System;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public int CountAfter(string operateCode,int count)
{
	//这里写业务逻辑
	//...
	return count;
}
";
        private string DefaultDataSourceTriggerScript
    => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using SevenTiny.Cloud.MultiTenantPlatform.Core.CloudEntity;
//end using
//注释：上面的end using注释为using分隔符，请不要删除；
//注释：输出日志请使用 logger.Error(),logger.Debug(),logger.Info()
public object TriggerScriptDataSource(string operateCode, Dictionary<string, object> argumentsDic)
{
	//这里写业务逻辑
	//...
	return null;
}
";
    }
}
