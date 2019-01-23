﻿using MongoDB.Bson;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.CloudEntity;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.ServiceContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Service
{
    public class FieldBizDataService : IFieldBizDataService
    {
        readonly IFieldAggregationService fieldAggregationService;
        public FieldBizDataService(
            IFieldAggregationService _fieldAggregationService
            )
        {
            fieldAggregationService = _fieldAggregationService;
        }

        public Dictionary<string, FieldBizData> ConvertToDictionary(int InterfaceFieldId, BsonDocument bsonElement)
        {
            //接口配置的字段字典
            var interfaceMetaFieldsDic = fieldAggregationService.GetMetaFieldsDicByInterfaceFieldId(InterfaceFieldId);
            Dictionary<string, FieldBizData> keyValuePairs = new Dictionary<string, FieldBizData>();
            foreach (var field in interfaceMetaFieldsDic)
            {
                //如果当前结果集包含字段
                if (bsonElement.Contains(field.Key))
                {
                    keyValuePairs.Add(field.Key, new FieldBizData
                    {
                        Name = field.Key,
                        Text = field.Value.Name,
                        Value = bsonElement[field.Key]
                    });
                }
            }
            return keyValuePairs;
        }

        public List<Dictionary<string, FieldBizData>> ConvertToDictionaryList(int InterfaceFieldId, List<BsonDocument> bsonElements)
        {
            //接口配置的字段字典
            var interfaceMetaFieldsDic = fieldAggregationService.GetMetaFieldsDicByInterfaceFieldId(InterfaceFieldId);
            List<Dictionary<string, FieldBizData>> resultList = new List<Dictionary<string, FieldBizData>>();
            foreach (var elements in bsonElements)
            {
                Dictionary<string, FieldBizData> keyValuePairs = new Dictionary<string, FieldBizData>();
                foreach (var field in interfaceMetaFieldsDic)
                {
                    //如果当前结果集包含字段
                    if (elements.Contains(field.Key))
                    {
                        keyValuePairs.Add(field.Key, new FieldBizData
                        {
                            Name = field.Key,
                            Text = field.Value.Name,
                            Value = elements[field.Key]
                        });
                    }
                }
                resultList.Add(keyValuePairs);
            }
            return resultList;
        }
    }
}
