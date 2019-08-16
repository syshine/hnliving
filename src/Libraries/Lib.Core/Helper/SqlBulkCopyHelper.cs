using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Lib.Core
{
    public static class SqlBulkCopyHelper
    {
        #region SqlBulkCopy批量插入数据
        /// <summary>
        /// 执行SqlBulkCopy批量插入，执行事务。
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="dt">要插入的数据</param>
        /// <returns></returns>
        public static int SqlBulkCopy(string TableName, DataTable dt)
        {
            return SqlBulkCopy(RDBSHelper.ConnectionString, TableName, dt);
        }

        /// <summary>
        /// 执行SqlBulkCopy批量插入，执行事务。
        /// </summary>
        /// <param name="connectionString">数据连接</param>
        /// <param name="TableName">表名</param>
        /// <param name="dt">要插入的数据</param>
        /// <returns></returns>
        public static int SqlBulkCopy(string connectionString, string TableName, DataTable dt)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy sqlbulkcopy = new SqlBulkCopy(connectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    try
                    {
                        sqlbulkcopy.DestinationTableName = TableName;
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sqlbulkcopy.ColumnMappings.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName);
                        }
                        sqlbulkcopy.WriteToServer(dt);
                        return 1;
                    }
                    catch (System.Exception ex)
                    {
                        return 0;
                    }
                }
            }
        }

        /// <summary>  
        /// 批量插入（注意类型T里的属性名称大小写必须跟数据库的字段名称一模一样）
        /// </summary>  
        /// <typeparam name="T">泛型集合的类型</typeparam>  
        /// <param name="conn">连接对象</param>  
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>  
        /// <param name="list">要插入大泛型集合</param>  
        public static void BulkInsert<T>(string tableName, IList<T> list)
        {
            SqlConnection conn = new SqlConnection(RDBSHelper.ConnectionString);
            BulkInsert<T>(conn, tableName, list);
        }

        /// <summary>  
        /// 批量插入（注意类型T里的属性名称大小写必须跟数据库的字段名称一模一样）
        /// </summary>  
        /// <typeparam name="T">泛型集合的类型</typeparam>  
        /// <param name="conn">连接对象</param>  
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>  
        /// <param name="list">要插入大泛型集合</param>  
        public static void BulkInsert<T>(SqlConnection conn, string tableName, IList<T> list)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open(); //打开Connection连接  
            }
            using (var bulkCopy = new SqlBulkCopy(conn))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))

                    .Cast<PropertyDescriptor>()
                    .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                    .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }

            if (conn.State != ConnectionState.Closed)
            {
                conn.Close(); //关闭Connection连接  
            }
        }
        #endregion















        ///// <summary>
        ///// 使用 SqlBulkCopy 向 destinationTableName 表插入数据
        ///// </summary>
        ///// <typeparam name="TModel">必须拥有与目标表所有字段对应属性</typeparam>
        ///// <param name="conn"></param>
        ///// <param name="modelList">要插入的数据</param>
        ///// <param name="batchSize">SqlBulkCopy.BatchSize</param>
        ///// <param name="destinationTableName">如果为 null，则使用 TModel 名称作为 destinationTableName</param>
        ///// <param name="bulkCopyTimeout">SqlBulkCopy.BulkCopyTimeout</param>
        ///// <param name="externalTransaction">要使用的事务</param>
        //public static void BulkCopy<TModel>(this SqlConnection conn, List<TModel> modelList, int batchSize, string destinationTableName = null, int? bulkCopyTimeout = null, SqlTransaction externalTransaction = null)
        //{
        //    bool shouldCloseConnection = false;

        //    if (string.IsNullOrEmpty(destinationTableName))
        //        destinationTableName = typeof(TModel).Name;

        //    DataTable dtToWrite = ToSqlBulkCopyDataTable(modelList, conn, destinationTableName);

        //    SqlBulkCopy sbc = null;

        //    try
        //    {
        //        if (externalTransaction != null)
        //            sbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, externalTransaction);
        //        else
        //            sbc = new SqlBulkCopy(conn);

        //        using (sbc)
        //        {
        //            sbc.BatchSize = batchSize;
        //            sbc.DestinationTableName = destinationTableName;

        //            if (bulkCopyTimeout != null)
        //                sbc.BulkCopyTimeout = bulkCopyTimeout.Value;

        //            if (conn.State != ConnectionState.Open)
        //            {
        //                shouldCloseConnection = true;
        //                conn.Open();
        //            }

        //            sbc.WriteToServer(dtToWrite);
        //        }
        //    }
        //    finally
        //    {
        //        if (shouldCloseConnection && conn.State == ConnectionState.Open)
        //            conn.Close();
        //    }
        //}

        //public static DataTable ToSqlBulkCopyDataTable<TModel>(List<TModel> modelList, SqlConnection conn, string tableName)
        //{
        //    DataTable dt = new DataTable();

        //    Type modelType = typeof(TModel);

        //    List<SysColumn> columns = GetTableColumns(conn, tableName);
        //    List<PropertyInfo> mappingProps = new List<PropertyInfo>();

        //    var props = modelType.GetProperties();
        //    for (int i = 0; i < columns.Count; i++)
        //    {
        //        var column = columns[i];
        //        PropertyInfo mappingProp = props.Where(a => a.Name == column.Name).FirstOrDefault();
        //        if (mappingProp == null)
        //            throw new Exception(string.Format("model 类型 '{0}'未定义与表 '{1}' 列名为 '{2}' 映射的属性", modelType.FullName, tableName, column.Name));

        //        mappingProps.Add(mappingProp);
        //        Type dataType = GetUnderlyingType(mappingProp.PropertyType);
        //        if (dataType.IsEnum)
        //            dataType = typeof(int);
        //        dt.Columns.Add(new DataColumn(column.Name, dataType));
        //    }

        //    foreach (var model in modelList)
        //    {
        //        DataRow dr = dt.NewRow();
        //        for (int i = 0; i < mappingProps.Count; i++)
        //        {
        //            PropertyInfo prop = mappingProps[i];
        //            object value = prop.GetValue(model);

        //            if (GetUnderlyingType(prop.PropertyType).IsEnum)
        //            {
        //                if (value != null)
        //                    value = (int)value;
        //            }

        //            dr[i] = value ?? DBNull.Value;
        //        }

        //        dt.Rows.Add(dr);
        //    }

        //    return dt;
        //}
        //static List<SysColumn> GetTableColumns(SqlConnection sourceConn, string tableName)
        //{
        //    string sql = string.Format("select * from syscolumns inner join sysobjects on syscolumns.id=sysobjects.id where sysobjects.xtype='U' and sysobjects.name='{0}' order by syscolumns.colid asc", tableName);

        //    List<SysColumn> columns = new List<SysColumn>();
        //    using (SqlConnection conn = (SqlConnection)((ICloneable)sourceConn).Clone())
        //    {
        //        //SqlCommand cmd = new SqlCommand(sql, conn);
        //        //using (var reader = cmd.ExecuteReader())
        //        conn.Open();
        //        using (var reader = conn.ExecuteReader(sql))
        //        {
        //            while (reader.Read())
        //            {
        //                SysColumn column = new SysColumn();
        //                column.Name = reader.GetDbValue("name");
        //                column.ColOrder = reader.GetDbValue("colorder");

        //                columns.Add(column);
        //            }
        //        }
        //        conn.Close();
        //    }

        //    return columns;
        //}

        //static Type GetUnderlyingType(Type type)
        //{
        //    Type unType = Nullable.GetUnderlyingType(type); ;
        //    if (unType == null)
        //        unType = type;

        //    return unType;
        //}

        //class SysColumn
        //{
        //    public string Name { get; set; }
        //    public int ColOrder { get; set; }
        //}
    }
}
