using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace hnliving.web.WebServices
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "hnliving.web.WebServices")]//http://tempuri.org/
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World from c# webservice";
        }

        [WebMethod]
        public string HelloWorldByJava()
        {
            string ret = "";
            try
            {
                //类调用
                //ServiceReferenceJava.WebServiceImplClient wsic = new ServiceReferenceJava.WebServiceImplClient();
                //ret = wsic.sayHello();

                //接口调用
                ServiceReferenceJava.WebServiceImpl wsi = new ServiceReferenceJava.WebServiceImplClient();
                ServiceReferenceJava.sayHelloRequest shr = new ServiceReferenceJava.sayHelloRequest();
                ServiceReferenceJava.sayHelloResponse shres = wsi.sayHello(shr);
                ret = shres.Body.@return;
            }
            catch (Exception ex)
            {

                ret = ex.Message;
            }

            return "调用sayHello结果：" + ret;
        }

        [WebMethod]
        public string SavebyJava(string name, string str)
        {
            string ret = "";
            if(string.IsNullOrWhiteSpace(name))
                name = "allen";
            if (string.IsNullOrWhiteSpace(str))
                str = "zhangsan";
            try
            {
                //类调用
                ServiceReferenceJava.WebServiceImplClient wsic = new ServiceReferenceJava.WebServiceImplClient();
                ret = wsic.save(name, str);

                //接口调用
                //ServiceReferenceJava.WebServiceImpl wsi = new ServiceReferenceJava.WebServiceImplClient();
                //ServiceReferenceJava.saveRequest shr = new ServiceReferenceJava.saveRequest();
                //ServiceReferenceJava.saveRequestBody srb = new ServiceReferenceJava.saveRequestBody();
                //srb.arg0 = "allen";
                //srb.arg1 = "zhangsan";
                //shr.Body = srb;
                //ServiceReferenceJava.saveResponse shres = wsi.save(shr);
                //ret = shres.Body.@return;
            }
            catch (Exception ex)
            {

                ret = ex.Message;
            }

            return "调用保存结果：" + ret;
        }
    }
}
