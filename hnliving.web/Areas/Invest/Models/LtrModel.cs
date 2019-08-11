using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hnliving.web.Areas.Invest.Models
{
    public class LtrModel
    {
    }

    #region Qxc
    /// <summary>
    /// 添加Qxc模型类
    /// </summary>
    public class AddLtrQxcModel
    {
        public AddLtrQxcModel()
        {
            Issue = -1;
        }

        /// <summary>
        /// 期号
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请输入期号")]
        [DisplayName("期号")]
        public int Issue { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Required(ErrorMessage = "日期不能为空")]
        [DisplayName("日期")]
        public DateTime Dt { get; set; }

        /// <summary>
        /// 号码
        /// </summary>
        [Required(ErrorMessage = "号码不能为空")]
        [StringLength(7, MinimumLength=7, ErrorMessage = "长度必须是7")]
        [DisplayName("号码")]
        public string Numb { get; set; }

    }
    #endregion

    #region Pl5
    /// <summary>
    /// 添加Pl5模型类
    /// </summary>
    public class AddLtrPl5Model
    {
        public AddLtrPl5Model()
        {
            Issue = -1;
        }

        /// <summary>
        /// 期号
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请输入期号")]
        [DisplayName("期号")]
        public int Issue { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Required(ErrorMessage = "日期不能为空")]
        [DisplayName("日期")]
        public DateTime Dt { get; set; }

        /// <summary>
        /// 号码
        /// </summary>
        [Required(ErrorMessage = "号码不能为空")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "长度必须是5")]
        [DisplayName("号码")]
        public string Numb { get; set; }

    }
    #endregion
}