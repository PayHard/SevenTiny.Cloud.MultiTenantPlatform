﻿using SevenTiny.Cloud.MultiTenant.Domain.DbContext;
using SevenTiny.Cloud.MultiTenant.Domain.Entity;
using SevenTiny.Cloud.MultiTenant.Domain.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTiny.Cloud.MultiTenant.Domain.Repository
{
    internal class ListViewRepository : MetaObjectCommonRepositoryBase<ListView>, IListViewRepository
    {
        public ListViewRepository(MultiTenantPlatformDbContext multiTenantPlatformDbContext) : base(multiTenantPlatformDbContext)
        {
        }
    }
}
