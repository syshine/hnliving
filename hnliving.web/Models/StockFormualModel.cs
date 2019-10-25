using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace hnliving.web.Models
{
    public class StockFormulaModel
    {
    }

    /// <summary>
    /// 添加股票公式模型类
    /// </summary>
    public class AddStockFormulaModel
    {
        public AddStockFormulaModel()
        {
            GroupId = 1;
        }

        /// <summary>
        /// 分组id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择分组")]
        [DisplayName("分组：")]
        public int GroupId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能大于100")]
        [DisplayName("标题")]
        public string Name { get; set; }

        /// <summary>
        /// 使用前复权数据
        /// </summary>
        public bool UseFormerComplexRights { get; set; }

        /// <summary>
        /// 使用涨跌幅
        /// </summary>
        public bool PCHGEnable { get; set; }

        /// <summary>
        /// 连续N日
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 是否涨
        /// </summary>
        public string IsRise { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal PCHG { get; set; }

        /// <summary>
        /// 使用价格区间
        /// </summary>
        public bool PriceEnable { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal PriceLow { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public decimal PriceHigh { get; set; }

        /// <summary>
        /// 使用价格区间
        /// </summary>
        public bool MCAPEnable { get; set; }

        /// <summary>
        /// 最低流通市值(亿)
        /// </summary>
        public decimal MCAPLow { get; set; }

        /// <summary>
        /// 最高流通市值(亿)
        /// </summary>
        public decimal MCAPHigh { get; set; }

        /// <summary>
        /// 使用公式
        /// </summary>
        public bool FormulaEnable { get; set; }

        /// <summary>
        /// 预处理哪几天均线
        /// </summary>
        public string PreAvgLines { get; set; }

        /// <summary>
        /// 预处理均线天数
        /// </summary>
        public int PreDays { get; set; }

        /// <summary>
        /// 公式
        /// </summary>
        public string Formula { get; set; }

    }

    /// <summary>
    /// 编辑股票公式模型类
    /// </summary>
    public class EditStockFormulaModel
    {
        public EditStockFormulaModel()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        public int FId { get; set; }

        /// <summary>
        /// 分组id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择分组")]
        [DisplayName("分组：")]
        public int GroupId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "名称不能为空")]
        [StringLength(100, ErrorMessage = "名称长度不能大于100")]
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 使用前复权数据
        /// </summary>
        public bool UseFormerComplexRights { get; set; }

        /// <summary>
        /// 使用涨跌幅
        /// </summary>
        public bool PCHGEnable { get; set; }

        /// <summary>
        /// 连续N日
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 是否涨
        /// </summary>
        public string IsRise { get; set; }

        /// <summary>
        /// 操作符
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 涨跌幅
        /// </summary>
        public decimal PCHG { get; set; }

        /// <summary>
        /// 使用价格区间
        /// </summary>
        public bool PriceEnable { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public decimal PriceLow { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public decimal PriceHigh { get; set; }

        /// <summary>
        /// 使用价格区间
        /// </summary>
        public bool MCAPEnable { get; set; }

        /// <summary>
        /// 最低流通市值(亿)
        /// </summary>
        public decimal MCAPLow { get; set; }

        /// <summary>
        /// 最高流通市值(亿)
        /// </summary>
        public decimal MCAPHigh { get; set; }

        /// <summary>
        /// 使用公式
        /// </summary>
        public bool FormulaEnable { get; set; }

        /// <summary>
        /// 预处理哪几天均线
        /// </summary>
        public string PreAvgLines { get; set; }

        /// <summary>
        /// 预处理均线天数
        /// </summary>
        public int PreDays { get; set; }

        /// <summary>
        /// 公式
        /// </summary>
        public string Formula { get; set; }

    }
}