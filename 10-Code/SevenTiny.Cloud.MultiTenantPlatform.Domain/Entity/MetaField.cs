﻿using SevenTiny.Bantina.Bankinate;
using SevenTiny.Cloud.MultiTenantPlatform.Domain.Enum;
using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Domain.Entity
{
    [TableCaching]
    public class MetaField : MetaObjectManageInfo
    {
        //=DataType
        [Column]
        public int FieldType { get; set; }
        //if field type is datasource
        [Column]
        public int DataSourceId { get; set; } = -1;
        [Column]
        public int IsMust { get; set; }
        [Column]
        public int IsSystem { get; set; } = (int)TrueFalse.False;
    }
}
