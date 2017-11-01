using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BizLayer
{
    public static class EmailHelper
    {
        public static bool SendEmail(string title, string name, string email, string telNo, string content)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<b>{0}：</b>{1}<br/>", "姓名", name));
            sb.Append(string.Format("<b>{0}：</b>{1}<br/>", "邮箱", email));
            sb.Append(string.Format("<b>{0}：</b>{1}<br/>", "联系电话", telNo));
            sb.Append(string.Format("<b>{0}：</b><br/>{1}", "正文", content));
            return SendEmail(title, sb.ToString());
        }



        public static bool SendEmail(string subject, string body)
        {
            string userName = "envrnotice@163.com";// 发送端账号   
            string password = "1qazxsw2";// 发送端密码(这个客户端重置后的密码)
            string targetEmail = userName;

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式    
            client.Host = "smtp.163.com";//邮件服务器
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(userName, password);//用户名、密码

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(userName);
            msg.To.Add(targetEmail);
            msg.Subject = subject;//邮件标题   
            msg.Body = body;//邮件内容   
            msg.BodyEncoding = Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   

            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {
                LogRecord.WriteLogExt(ex.ToString(), "Email");
                return false;
            }
            return true;
        }

    }
}
