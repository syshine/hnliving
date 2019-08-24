using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core.Domain.ActiveMQ
{
    [Serializable]
    class ActiveMQModel
    {
        /// <summary>
        /// guid
        /// </summary>
        public string guid { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// 接口参数（T转json）
        /// </summary>
        public string json { get; set; }
    }
}
