﻿//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using Seventiny.Cloud.DataApi.Models;
//using SevenTiny.Cloud.MultiTenantPlatform.Core.DataAccess;
//using SevenTiny.Cloud.MultiTenantPlatform.Core.Enum;
//using SevenTiny.Cloud.MultiTenantPlatform.Core.ServiceContract;
//using SevenTiny.Cloud.MultiTenantPlatform.TriggerScriptEngine.ServiceContract;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Seventiny.Cloud.DataApi.Controllers
//{
//    [EnableCors("AllowSameDomain")]
//    [Route("api/UI/[controller]")]
//    [ApiController]
//    public class IndexPageController : ControllerBase
//    {
//        public IndexPageController(
//            IDataAccessService _dataAccessService,
//            ISearchConditionService _searchConditionService,
//            ISearchConditionNodeService _conditionAggregationService,
//            IIndexViewService _indexViewService,
//            IFieldBizDataService _fieldBizDataService,
//            ITriggerScriptExecuteService _triggerScriptEngineService,
//            IMetaObjectService _metaObjectService,
//            IMetaFieldService _metaFieldService
//            )
//        {
//            dataAccessService = _dataAccessService;
//            conditionAggregationService = _conditionAggregationService;
//            indexViewService = _indexViewService;
//            fieldBizDataService = _fieldBizDataService;
//            triggerScriptEngineService = _triggerScriptEngineService;
//            searchConditionService = _searchConditionService;
//            metaObjectService = _metaObjectService;
//            metaFieldService = _metaFieldService;
//        }

//        readonly IDataAccessService dataAccessService;
//        readonly IIndexViewService indexViewService;
//        readonly ISearchConditionNodeService conditionAggregationService;
//        readonly IFieldBizDataService fieldBizDataService;
//        readonly ITriggerScriptExecuteService triggerScriptEngineService;
//        readonly ISearchConditionService searchConditionService;
//        readonly IMetaObjectService metaObjectService;
//        readonly IMetaFieldService metaFieldService;


//        [HttpGet]
//        public IActionResult Get([FromQuery]UIIndexPageQueryArgs queryArgs)
//        {
//            try
//            {
//                //args check
//                if (queryArgs == null)
//                {
//                    return JsonResultModel.Error($"Parameter invalid:queryArgs = null");
//                }

//                var checkResult = queryArgs.ArgsCheck();

//                if (!checkResult.IsSuccess)
//                {
//                    return checkResult.ToJsonResultModel();
//                }

//                //argumentsDic generate
//                Dictionary<string, object> argumentsDic = new Dictionary<string, object>();
//                foreach (var item in Request.Query)
//                {
//                    if (!argumentsDic.ContainsKey(item.Key))
//                    {
//                        argumentsDic.Add(item.Key.ToUpperInvariant(), item.Value);
//                    }
//                }

//                //get filter
//                var indexView = indexViewService.GetByCode(queryArgs.ViewName);
//                if (indexView == null)
//                {
//                    return JsonResultModel.Error($"未能找到视图编码为[{queryArgs.ViewName}]对应的视图信息");
//                }

//                var indexPageComponent = indexViewService.GetIndexPageComponentByIndexView(indexView);

//                //IndexView触发器
//                //filter = triggerScriptEngineService.TableListBefore(indexView.MetaObjectId, indexView.Code, filter);
//                //var sort = metaFieldService.GetSortDefinitionBySortFields(indexView.MetaObjectId, queryArgs.SortFields);
//                //var tableListComponent = dataAccessService.GetTableListComponent(indexView.MetaObjectId, indexView.FieldListId, filter, queryArgs.PageIndex, queryArgs.PageSize, sort, out int totalCount);
//                //tableListComponent = triggerScriptEngineService.TableListAfter(indexView.MetaObjectId, indexView.Code, tableListComponent);

//                return JsonResultModel.Success("get index page component success", indexPageComponent);
//            }
//            catch (ArgumentNullException argNullEx)
//            {
//                return JsonResultModel.Error(argNullEx.Message);
//            }
//            catch (ArgumentException argEx)
//            {
//                return JsonResultModel.Error(argEx.Message);
//            }
//            catch (Exception ex)
//            {
//                return JsonResultModel.Error(ex.Message);
//            }
//        }
//    }
//}