using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace hnliving.web.Models
{
    public class UEditorModel
    {
    }

    /// <summary>
    /// 添加ueditor模型类
    /// </summary>
    public class AddUEditorModel
    {
        public AddUEditorModel()
        {
            TypeId = 1;
        }

        /// <summary>
        /// 分类id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择分类")]
        [DisplayName("分类")]
        public int TypeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能大于100")]
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [AllowHtml]
        public string Content { get; set; }
        
    }

    /// <summary>
    /// 编辑ueditor模型类
    /// </summary>
    public class EditUEditorModel
    {
        public EditUEditorModel()
        {
        }

        /// <summary>
        /// id
        /// </summary>
        public int UeId { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "请选择分类")]
        [DisplayName("分类")]
        public int TypeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(100, ErrorMessage = "标题长度不能大于100")]
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [AllowHtml]
        public string Content { get; set; }

    }
}