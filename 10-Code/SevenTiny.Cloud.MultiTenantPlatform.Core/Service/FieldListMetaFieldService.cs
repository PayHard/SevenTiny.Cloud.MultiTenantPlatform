﻿using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Repository;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
using SevenTiny.Cloud.MultiTenantPlatform.Core.ValueObject;
using SevenTiny.Cloud.MultiTenantPlatform.UIModel.UIMetaData.ListView;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Service
{
    public class FieldListMetaFieldService : Repository<FieldListMetaField>, IFieldListMetaFieldService
    {
        public FieldListMetaFieldService(
            MultiTenantPlatformDbContext multiTenantPlatformDbContext,
            IMetaFieldService _metaFieldService
            ) : base(multiTenantPlatformDbContext)
        {
            dbContext = multiTenantPlatformDbContext;
            metaFieldService = _metaFieldService;
        }

        readonly MultiTenantPlatformDbContext dbContext;
        readonly IMetaFieldService metaFieldService;

        public Result<IList<FieldListMetaField>> Add(int metaObjectId, IList<FieldListMetaField> entities)
        {
            var metaFieldIds = entities.Select(t => t.MetaFieldId).ToArray();
            var metaFields = metaFieldService.GetByIds(metaObjectId, metaFieldIds);
            foreach (var item in entities)
            {
                var meta = metaFields.FirstOrDefault(t => t.Id == item.MetaFieldId);
                if (meta != null)
                {
                    item.Name = meta.Code;
                    item.Text = meta.Name;
                    item.FieldType = meta.FieldType;
                }
            }
            return base.Add(entities);
        }

        public List<FieldListMetaField> GetByFieldListId(int fieldListId)
        {
            return dbContext.Queryable<FieldListMetaField>().Where(t => t.FieldListId == fieldListId).ToList();
        }

        public void DeleteByMetaFieldId(int metaFieldId)
        {
            dbContext.Delete<FieldListMetaField>(t => t.MetaFieldId == metaFieldId);
        }

        public List<Column> GetColumnDataByFieldListId(QueryPiplineContext queryPiplineContext)
        {
            var fieldListMetaFields = queryPiplineContext.FieldListMetaFieldsOfFieldListId;
            if (fieldListMetaFields != null && fieldListMetaFields.Any())
            {
                List<Column> columns = new List<Column>();
                foreach (var item in fieldListMetaFields)
                {
                    columns.Add(new Column
                    {
                        CmpData = new ColumnCmpData
                        {
                            Name = item.Name,
                            Title = item.Text,
                            Type = item.FieldType.ToString(),
                            Visible = TrueFalseTranslator.ToBoolean(item.IsVisible),
                            IsLink = TrueFalseTranslator.ToBoolean(item.IsLink),
                            ShowIndex = item.SortNumber
                        }
                    });
                }
                return columns;
            }
            return null;
        }

        public FieldListMetaField GetById(int id)
        {
            return dbContext.Queryable<FieldListMetaField>().Where(t => t.Id == id).ToOne();
        }

        public new Result<FieldListMetaField> Update(FieldListMetaField entity)
        {
            var entityExist = GetById(entity.Id);
            if (entityExist != null)
            {
                entityExist.IsLink = entity.IsLink;
                entityExist.IsVisible = entity.IsVisible;
                entityExist.Text = entity.Text;
            }
            return base.Update(entityExist);
        }

        public void SortFields(int interfaceFieldId, int[] currentOrderMetaFieldIds)
        {
            //异步方法mysql超时!!!
            //await Task.Run(() =>
            //{
            var fieldList = GetByFieldListId(interfaceFieldId);
            if (fieldList != null && fieldList.Any())
            {
                //i为当前应该保持的顺序
                for (int i = 0; i < currentOrderMetaFieldIds.Length; i++)
                {
                    //寻找第i个字段
                    var item = fieldList.FirstOrDefault(t => t.MetaFieldId == currentOrderMetaFieldIds[i]);
                    //如果该字段的排序值!=当前应该保持的顺序，则加到更新队列
                    if (item != null && item.SortNumber != i)
                    {
                        item.SortNumber = i;
                        base.Update(item);
                    }
                }
            }
            //});
        }
    }
}
