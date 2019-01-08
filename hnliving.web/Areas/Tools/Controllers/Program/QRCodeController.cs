using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using QRCoder;

namespace hnliving.web.Areas.Tools.Controllers.Program
{
    public class QRCodeController : BaseWebController
    {
        // GET: Tools/Programming/QRCode
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 二维码图片
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <returns></returns>
        public ActionResult GetQRCode(string content)
        {
            QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            QRCode qrcode = new QRCode(qrCodeData);
            Bitmap icon = new Bitmap(Server.MapPath("~/Images/Icons/logo32.ico"));
            

            // qrcode.GetGraphic 方法可参考最下发“补充说明”
            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, icon, 15, 6, false);
            MemoryStream ms = new MemoryStream();
            qrCodeImage.Save(ms, ImageFormat.Jpeg);

            // 如果想保存图片 可使用  qrCodeImage.Save(filePath);

            return new ImageResult(ms.ToArray(), "image/Jpeg");

            /// GetGraphic方法参数说明
            /// public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 6, bool drawQuietZones = true)
            /// int pixelsPerModule:生成二维码图片的像素大小 ，我这里设置的是5
            /// Color darkColor：暗色 一般设置为Color.Black 黑色
            /// Color lightColor:亮色 一般设置为Color.White 白色
            /// Bitmap icon :二维码 水印图标 例如：Bitmap icon = new Bitmap(context.Server.MapPath("~/images/zs.png")); 默认为NULL ，加上这个二维码中间会显示一个图标
            /// int iconSizePercent： 水印图标的大小比例 ，可根据自己的喜好设置
            /// int iconBorderWidth： 水印图标的边框
            /// bool drawQuietZones:静止区，位于二维码某一边的空白边界,用来阻止读者获取与正在浏览的二维码无关的信息 即是否绘画二维码的空白边框区域 默认为true

        }
    }
}