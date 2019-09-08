using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    [Serializable]
    public class ResultEntity
    {
        private bool _isSuccess;    // 成功标志
        private string _status;     // 处理结果状态
        private object _data;       // 返回的数据
        private string _message;    // 消息（错误消息）

        #region 构造函数
        public ResultEntity()
        {
            _isSuccess = false;
            _status = "fail";
        }

        public ResultEntity(string status, object data, string message)
        {
            Set(status, data, message);
        }

        public ResultEntity(bool success, string status, object data, string message)
        {
            Set(success, status, data, message);
        }
        #endregion

        public static ResultEntity ParamsError(string message)
        {
            return new ResultEntity("params error", "", message);
        }

        public void SetError(string message, object data = null)
        {
            Set("error", data, message);
        }

        public void SetSuccess(object data = null, string message = null)
        {
            Set(true, data, message);
        }

        /// <summary>
        /// 设置结果
        /// status为success时是成功
        /// </summary>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public void Set(string status, object data, string message)
        {
            if (status == "success")
            {
                _isSuccess = true;
            }
            _status = status;
            _data = data;
            _message = message;
        }

        public void Set(bool success, object data, string message)
        {
            if (success)
            {
                _status = "success";
            }
            else
            {
                _status = "fail";
            }
            _isSuccess = success;
            _data = data;
            _message = message;
        }

        /// <summary>
        /// 设置结果
        /// </summary>
        /// <param name="success"></param>
        /// <param name="status"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public void Set(bool success, string status, object data, string message)
        {
            _isSuccess = success;
            _status = status;
            _data = data;
            _message = message;
        }

        public bool IsSuccess
        {
            get
            {
                return _isSuccess;
            }

            //set
            //{
            //    _isSuccess = value;
            //}
        }

        public string Status
        {
            get
            {
                return _status;
            }

            //set
            //{
            //    _status = value;
            //}
        }

        public object Data
        {
            get
            {
                return _data;
            }

            set
            {
                _data = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }
    }
}
