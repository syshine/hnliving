using System;
using System.Web.Mvc;

namespace hnliving.web
{
    /// <summary>
    /// 分页Html扩展
    /// </summary>
    public static class PagerHtmlExtension
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="helper">HtmlHelper</param>
        /// <param name="pageModel">分页对象</param>
        /// <returns></returns>
        public static WebPager WebPager(this HtmlHelper helper, PageModel pageModel)
        {
            return new WebPager(pageModel, helper.ViewContext);
        }
    }
}
