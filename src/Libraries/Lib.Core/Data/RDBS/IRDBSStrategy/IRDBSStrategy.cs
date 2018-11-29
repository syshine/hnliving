using System;
using System.Data.Common;

namespace Lib.Core
{
    /// <summary>
    /// HNLiving��ϵ���ݿ����֮���ݿ�ֲ��ӿ�
    /// </summary>
    public partial interface IRDBSStrategy
    {
        /// <summary>
        /// ������ݿ��ṩ���򹤳�
        /// </summary>
        DbProviderFactory GetDbProviderFactory();

        /// <summary>
        /// ����SQL���
        /// </summary>
        /// <param name="sql">SQL���</param>
        /// <returns></returns>
        string RunSql(string sql);
    }
}