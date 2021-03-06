﻿using SevenTiny.Bantina;
using SevenTiny.Cloud.MultiTenantPlatform.Core.Entity;
using System.Collections.Generic;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Repository
{
    public interface IMetaObjectManageRepository<TEntity> : ICommonInfoRepository<TEntity> where TEntity : MetaObjectManageInfo
    {
        void DeleteByMetaObjectId(int metaObjectId);
        List<TEntity> GetEntitiesByMetaObjectId(int metaObjectId);
        List<TEntity> GetEntitiesDeletedByMetaObjectId(int metaObjectId);
        List<TEntity> GetEntitiesUnDeletedByMetaObjectId(int metaObjectId);
        Result<TEntity> CheckSameCodeOrName(int metaObjectId, TEntity entity);
    }
}
