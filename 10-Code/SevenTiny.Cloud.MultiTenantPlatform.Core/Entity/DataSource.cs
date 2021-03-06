﻿using SevenTiny.Bantina.Bankinate.Attributes;

namespace SevenTiny.Cloud.MultiTenantPlatform.Core.Entity
{
    [TableCaching]
    public class DataSource : CommonInfo
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Column]
        public int ApplicationId { get; set; }
        /// <summary>
        /// 触发器类型
        /// </summary>
        [Column]
        public int DataSourceType { get; set; }
        /// <summary>
        /// 脚本语言
        /// </summary>
        [Column]
        public int Language { get; set; }
        /// <summary>
        /// 脚本内容
        /// </summary>
        [Column]
        public string Script { get; set; }
    }
}
