﻿using Microsoft.AspNetCore.Mvc;
using SevenTiny.Bantina.Validation;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.ServiceContract;
using SevenTiny.Cloud.MultiTenant.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace SevenTiny.Cloud.MultiTenant.Development.Controllers
{
    public class SearchConditionController : WebControllerBase
    {
        private readonly ISearchConditionService searchConditionService;
        private readonly IMetaFieldService metaFieldService;
        readonly ISearchConditionNodeService conditionAggregationService;

        public SearchConditionController(
            ISearchConditionService _searchConditionService,
            IMetaFieldService _metaFieldService,
            ISearchConditionNodeService _conditionAggregationService
            )
        {
            searchConditionService = _searchConditionService;
            metaFieldService = _metaFieldService;
            conditionAggregationService = _conditionAggregationService;
        }

        public IActionResult List()
        {
            return View(searchConditionService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId));
        }

        public IActionResult SearchItemList(int id)
        {
            return View(conditionAggregationService.GetConditionItemsBySearchConditionId(id));
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult AddLogic(SearchCondition entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Add", ResponseModel.Error("名称不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Add", ResponseModel.Error("编码不能为空", entity));
            }
            //校验code格式
            if (!entity.Code.IsAlnum(2, 50))
            {
                return View("Add", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            }

            //检查编码或名称重复
            var checkResult = searchConditionService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Add", checkResult.ToResponseModel());
            }

            entity.MetaObjectId = CurrentMetaObjectId;
            //组合编码
            entity.Code = $"{CurrentMetaObjectCode}.SearchCondition.{entity.Code}";
            entity.CreateBy = CurrentUserId;
            searchConditionService.Add(entity);
            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            var entity = searchConditionService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult UpdateLogic(SearchCondition entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("MetaField Id 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Name))
            {
                return View("Update", ResponseModel.Error("MetaField Name 不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Code))
            {
                return View("Update", ResponseModel.Error("MetaField Code 不能为空", entity));
            }
            //校验code格式，编码不更新
            //if (!entity.Code.IsAlnum(2, 50))
            //{
            //    return View("Update", ResponseModel.Error("编码不合法，2-50位且只能包含字母和数字（字母开头）", entity));
            //}

            //检查编码或名称重复
            var checkResult = searchConditionService.CheckSameCodeOrName(CurrentMetaObjectId, entity);
            if (!checkResult.IsSuccess)
            {
                return View("Update", checkResult.ToResponseModel());
            }

            entity.ModifyBy = CurrentUserId;
            //更新操作
            searchConditionService.Update(entity);

            return RedirectToAction("List");
        }

        public IActionResult SearchItemUpdate(int id)
        {
            var entity = conditionAggregationService.GetById(id);
            return View(ResponseModel.Success(entity));
        }

        public IActionResult SearchItemUpdateLogic(SearchConditionNode entity)
        {
            if (entity.Id == 0)
            {
                return View("Update", ResponseModel.Error("Id不能为空", entity));
            }
            if (string.IsNullOrEmpty(entity.Text))
            {
                return View("Update", ResponseModel.Error("显示名称不能为空", entity));
            }

            //更新操作
            var result = conditionAggregationService.Update(entity);
            if (result.IsSuccess)
            {
                return View("SearchItemUpdate", ResponseModel.Success(1, "修改成功"));
            }
            return View("SearchItemUpdate", result.ToResponseModel());
        }

        public IActionResult Delete(int id)
        {
            return searchConditionService.Delete(id).ToJsonResultModel();
        }

        public IActionResult AggregationCondition(int id)
        {
            ViewData["AggregationConditions"] = conditionAggregationService.GetListBySearchConditionId(id);
            ViewData["MetaFields"] = metaFieldService.GetEntitiesUnDeletedByMetaObjectId(CurrentMetaObjectId);
            ViewData["searchConditionId"] = id;
            return View();
        }

        /// <summary>
        /// 添加组织条件树
        /// </summary>
        /// <param name="id">搜索条件id，指明该条件组织树是属于那个搜索条件的</param>
        /// <returns></returns>
        public IActionResult AggregateConditionAddLogic(int id)
        {
            int brotherNodeId = int.Parse(Request.Form["brotherNodeId"]);
            int conditionJointTypeId = int.Parse(Request.Form["conditionJointTypeId"]);
            int fieldId = int.Parse(Request.Form["fieldId"]);
            int conditionTypeId = int.Parse(Request.Form["conditionTypeId"]);
            int conditionValueTypeId = int.Parse(Request.Form["conditionValueTypeId"]);
            string conditionValue = Request.Form["conditionValue"];

            var result = conditionAggregationService.AggregateCondition(id, brotherNodeId, conditionJointTypeId, fieldId, conditionTypeId, conditionValue, conditionValueTypeId);
            return result.ToJsonResultModel();
        }

        /// <summary>
        /// 删除组织条件树
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="searchConditionId">搜索条件id</param>
        /// <returns></returns>
        public IActionResult AggregateConditionDeleteLogic(int id, int searchConditionId)
        {
            //将要删除的节点id集合
            return conditionAggregationService.DeleteAggregateCondition(id, searchConditionId).ToJsonResultModel();
        }

        [HttpGet]
        public IActionResult AggregateConditionTreeView(int id)
        {
            List<SearchConditionNode> conditions = conditionAggregationService.GetListBySearchConditionId(id);

            SearchConditionNode condition = conditions?.FirstOrDefault(t => t.ParentId == -1);
            if (condition != null)
            {
                condition.Children = GetTree(conditions, condition.Id);
            }

            //Tree Search
            List<SearchConditionNode> GetTree(List<SearchConditionNode> source, int parentId)
            {
                var childs = source.Where(t => t.ParentId == parentId).ToList();
                if (childs == null)
                {
                    return new List<SearchConditionNode>();
                }
                else
                {
                    childs.ForEach(t => t.Children = GetTree(source, t.Id));
                }
                return childs;
            }

            if (condition != null)
            {
                return JsonResultModel.Success("构造树成功！", new List<SearchConditionNode> { condition });
            }
            else
            {
                return JsonResultModel.Success("构造树成功！", new List<SearchConditionNode>());
            }
        }
    }
}