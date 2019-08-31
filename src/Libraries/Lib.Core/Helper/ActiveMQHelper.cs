using Apache.NMS;
using Apache.NMS.Util;
using Lib.Core.Domain.ActiveMQ;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Core
{
    public class ActiveMQHelper
    {
        private static IConnectionFactory connFac;
        
        private static IConnection connection;
        private static ISession session;
        private static IDestination destination;
        private static IMessageProducer producer;
        private static IMessageConsumer consumer;
        
        /// <summary>
        /// 初始化ActiveMQ
        /// </summary>
        public static void initAMQ()
        {
            string strsendTopicName = "A";//推送方topic名
            string strreceiveTopicName = "B";//接受方toptic名
            var url = "localhost:61616";//activemq地址
            var userid = "system";//帐户
            var pwd = "system";//密码

            try
            {
                connFac = new NMSConnectionFactory(new Uri("activemq:failover:(tcp://" + url + ")")); //new NMSConnectionFactory(new Uri("activemq:failover:(tcp://localhost:61616)"));
                
                //新建连接
                connection = connFac.CreateConnection(userid, pwd);//connFac.CreateConnection("oa", "oa");//设置连接要用的用户名、密码
                
                //如果你要持久“订阅”，则需要设置ClientId，这样程序运行当中被停止，恢复运行时，能拿到没接收到的消息！
                connection.ClientId = "ClientId_" + strsendTopicName;
                //connection = connFac.CreateConnection();//如果你是缺省方式启动Active MQ服务，则不需填用户名、密码
                
                //创建Session
                session = connection.CreateSession();
                
                //发布/订阅模式，适合一对多的情况
                destination = SessionUtil.GetDestination(session, "topic://" + strreceiveTopicName);
                
                //新建生产者对象
                producer = session.CreateProducer(destination);
                producer.DeliveryMode = MsgDeliveryMode.Persistent;//ActiveMQ服务器停止工作后，消息不再保留
                
                //新建消费者对象:普通“订阅”模式
                //consumer = session.CreateConsumer(destination);//不需要持久“订阅”      
                
                //新建消费者对象:持久"订阅"模式：
                //    持久“订阅”后，如果你的程序被停止工作后，恢复运行，
                //从第一次持久订阅开始，没收到的消息还可以继续收
                //consumer = session.CreateDurableConsumer(
                //    session.GetTopic(strsendTopicName)
                //    , connection.ClientId, null, false);
                consumer = session.CreateDurableConsumer(
                    session.GetTopic(strreceiveTopicName)
                    , connection.ClientId, null, false);
                
                //设置消息接收事件
                consumer.Listener += new MessageListener(OnMessage);
                
                //启动来自Active MQ的消息侦听
                connection.Start();
            }
            catch (Exception e)
            {
                MngLog.Instance.Write("初始化ActiveMQ失败:" + e.Message);
                //System.Diagnostics.Debug.WriteLine("初始化ActiveMQ失败:" + e.Message);
                //SysErrorLog.SaveErrorInfo(e, "初始化ActiveMQ失败");
            }
        }



        /// <summary>
        /// 推送ActiveMQ
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="t"></param>
        /// <param name="method"></param>
        public static void Send(string guid, object t, string method)
        {

            if (producer == null)
            {
                initAMQ();
            }

            if (session == null)
            {
                throw new Exception("请初始化ActiveMQ！");
            }

            if (producer == null)
            {
                throw new Exception("请初始化ActiveMQ！");
            }

            var model = new ActiveMQModel();
            model.guid = guid;
            model.method = method;
            model.json = JsonConvert.SerializeObject(t);
            //var i = session.CreateObjectMessage(model);
            string jsonText = JsonConvert.SerializeObject(model);
            var i = session.CreateTextMessage(jsonText);
            producer.Send(i);
        }
        
        /// <summary>
        /// 接收ActiveMQ消息
        /// </summary>
        /// <param name="receivedMsg"></param>
        protected static void OnMessage(IMessage receivedMsg)
        {
            if (receivedMsg is IObjectMessage)
            {
                var message = receivedMsg as IObjectMessage;
                if (message.Body is ActiveMQModel)
                {
                    MngLog.Instance.Write("ActiveMQModel=" + JsonConvert.SerializeObject(message.Body));
                    //System.Diagnostics.Debug.WriteLine("ActiveMQModel=" + JsonConvert.SerializeObject(message.Body));
                    //SysErrorLog.SaveErrorInfo("ActiveMQModel=" + JsonConvert.SerializeObject(message.Body));
                }
            }
            else if (receivedMsg is ITextMessage)
            {
                ITextMessage message = receivedMsg as ITextMessage;
                string msg = message.NMSDestination + " : " + message.Text;
                MngLog.Instance.Write(msg);
            }
        }
    }
}
