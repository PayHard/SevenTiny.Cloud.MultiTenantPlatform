﻿namespace SevenTiny.Cloud.MultiTenantPlatform.UIModel.CloudEntity
{
    /// <summary>
    /// MetaData
    /// </summary>
    public class Data
    {
        public Data(string dataKey, int dataType, object dataValue)
        {
            this.DataKey = dataKey;
            this.DataType = dataType;
            this.DataValue = dataValue;
        }
        public string DataKey { get; set; }
        public int DataType { get; set; }
        public object DataValue { get; set; }
    }
}
