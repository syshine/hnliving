using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibLtr;
using Lib.Core;

namespace Lib.Data
{
    /// <summary>
    /// 用户数据访问类
    /// </summary>
    public partial class Ltr : ILtrStrategy
    {
        #region Qxc
        /// <summary>
        /// 获得Qxc数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetQxcData()
        {
            return Lib.Core.MngData.RDBS.GetQxcData();
        }

        /// <summary>
        /// 新增Qxc数据
        /// </summary>
        /// <returns></returns>
        public static int AddQxcNumb(QxcEntity entity)
        {
            return Lib.Core.MngData.RDBS.AddQxcNumb(entity);
        }
        #endregion


        #region Pl5
        /// <summary>
        /// 获得Pl5数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetPl5Data()
        {
            return Lib.Core.MngData.RDBS.GetPl5Data();
        }

        /// <summary>
        /// 新增Pl5数据
        /// </summary>
        /// <returns></returns>
        public static int AddPl5Numb(Pl5Entity entity)
        {
            return Lib.Core.MngData.RDBS.AddPl5Numb(entity);
        }
        #endregion
    }
}
