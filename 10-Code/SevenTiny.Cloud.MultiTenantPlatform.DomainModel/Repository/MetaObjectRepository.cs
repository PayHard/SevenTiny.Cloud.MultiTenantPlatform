﻿using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Entities;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Enums;
using SevenTiny.Cloud.MultiTenantPlatform.DomainModel.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SevenTiny.Cloud.MultiTenantPlatform.DomainModel.Repository
{
    public class MetaObjectRepository : IMetaObjectRepository
    {
        private readonly MultiTenantPlatformDbContext _context;
        public MetaObjectRepository(MultiTenantPlatformDbContext context)
        {
            _context = context;
        }

        public List<MetaObject> GetList(Expression<Func<MetaObject, bool>> filter)
        {
            return _context.QueryList<MetaObject>(filter);
        }

        public MetaObject GetEntity(Expression<Func<MetaObject, bool>> filter)
        {
            return _context.QueryOne(filter);
        }

        public void Add(MetaObject entity)
        {
            _context.Add(entity);
        }

        public void Add(IEnumerable<MetaObject> entities)
        {
            _context.Add(entities);
        }

        public void Update(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            _context.Update(filter, entity);
        }

        public void LogicDelete(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            entity.IsDeleted = (int)IsDeleted.Deleted;
            Update(filter, entity);
        }

        public void Recover(Expression<Func<MetaObject, bool>> filter, MetaObject entity)
        {
            entity.IsDeleted = (int)IsDeleted.NotDeleted;
            Update(filter, entity);
        }

        public void Delete(Expression<Func<MetaObject, bool>> filter)
        {
            _context.Delete(filter);
        }

        public bool Exist(Expression<Func<MetaObject, bool>> filter)
        {
            return _context.QueryCount(filter) > 0;
        }
    }
}