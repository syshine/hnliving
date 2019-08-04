using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lib.Core;
using Lib.Services;
using hnliving.web.Models;
using System.Data;

namespace hnliving.web.Controllers
{
    public class ContentController : BaseWebController
    {
        // GET: Content
        public ActionResult Index(int id)
        {
            // 获取内容的ID
            int uid = UserRanks.IsContentEditor(WorkContext.Uid) ? -1 : WorkContext.Uid;

            UEditorEntity uee = UEditorSer.GetById(id);

            if (uee.Uid != uid)
            {
                return PromptView("没有此内容");
            }

            ShowUEditorModel model = new ShowUEditorModel()
            {
                UeId = id,
                Title = uee.Title,
                TypeName = UEditorSer.GetName(uee.Typeid),
                Content = uee.Ue_content,
            };

            return View(model);
        }

        public ActionResult List(int typeid)
        {
            return View();
        }
    }
}