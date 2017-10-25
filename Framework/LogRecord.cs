using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace Framework
{
    public static class LogRecord
    {
        private static string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
        private static Thread LogTimer = null;
        private static object LogLock = new object();
        private static LogQueue Queue = new LogQueue();

        #region 文本日志

        /// <summary>
        /// 将字符串数组作为日志记录。每个字符串占用一行。
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLog(string[] content, string type = null)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string dirPath = LogPath;
            if (!string.IsNullOrWhiteSpace(type))
                dirPath = Path.Combine(LogPath, type);
            WriteFile(dirPath, fileName, content);
        }

        /// <summary>
        /// 记录自动附加时间的简单日志。
        /// </summary>
        /// <param name="content"></param>
        public static void WriteLog(string content, string type = null, bool isSingleLine = true)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string dirPath = LogPath;
            if (!string.IsNullOrWhiteSpace(type))
                dirPath = Path.Combine(LogPath, type);
            if (isSingleLine)
                content = string.Format("[{0}]\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
            else
                content = string.Format("[{0}]\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
            WriteFile(dirPath, fileName, content);
        }

        public static void WriteLogExt(string content, string type = null, bool isSingleLine = true)
        {
            if (LogTimer == null)
            {
                lock (LogLock)
                {
                    if (LogTimer == null)
                    {
                        LogTimer = new Thread(() =>
                        {
                            while (true)
                            {
                                LogTimer_Elapsed();
                                Thread.Sleep(2000);
                            }
                        });
                        LogTimer.IsBackground = true;
                        LogTimer.Start();
                    }
                }
            }
            if (isSingleLine)
                content = string.Format("[{0}]\t{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
            else
                content = string.Format("[{0}]\r\n{1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
            Log l = new Log();
            l.DirPath = string.IsNullOrWhiteSpace(type) ? LogPath : Path.Combine(LogPath, type);
            l.FileName = DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            l.Content = content;
            lock (LogLock)
            {
                Queue.StorageQueue.Enqueue(l);
            }
        }

        static void LogTimer_Elapsed()
        {
            if (Queue.WorkingQueue.Count == 0)
            {
                if (Queue.StorageQueue.Count == 0)
                    return;
                lock (LogLock)
                {
                    Queue.SwitchQueue();
                }
            }
            try
            {
                while (Queue.WorkingQueue.Count > 0)
                {
                    var log = Queue.WorkingQueue.Dequeue();
                    WriteFile(log.DirPath, log.FileName, log.Content);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        public static void WriteLog(LogInfo log)
        {
            if (log != null)
                WriteLog(log.ToStringArray(), log.Type.ToString());
        }

        /// <summary>
        /// 记录字符串数组到文件。每个字符串占用一行。
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="fileMode"></param>
        private static void WriteFile(string dirPath, string fileName, string[] content, string encoding = "UTF-8", FileMode fileMode = FileMode.OpenOrCreate)
        {
            if (content == null || content.Length == 0)
                return;

            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                stream = new FileStream(Path.Combine(dirPath, fileName), fileMode, FileAccess.Write);
                writer = new StreamWriter(stream, Encoding.GetEncoding(encoding));
                writer.BaseStream.Seek(0, SeekOrigin.End);
                foreach (string str in content)
                    writer.WriteLine(str);
                writer.Flush();
            }
            catch { }
            if (writer != null)
                writer.Dispose();
            if (stream != null)
                stream.Dispose();
        }

        /// <summary>
        /// 记录字符串到文件。
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="fileMode"></param>
        private static bool WriteFile(string dirPath, string fileName, string content, string encoding = "ISO-8859-1", FileMode fileMode = FileMode.OpenOrCreate)
        {
            FileStream stream = null;
            StreamWriter writer = null;
            try
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                stream = new FileStream(Path.Combine(dirPath, fileName), fileMode, FileAccess.Write);
                writer = new StreamWriter(stream, Encoding.GetEncoding(encoding));
                writer.BaseStream.Seek(0, SeekOrigin.End);
                writer.WriteLine(content);
                writer.Flush();
            }
            catch
            {
                return false;
            }
            if (writer != null)
                writer.Dispose();
            if (stream != null)
                stream.Dispose();
            return true;
        }

        public static string ReadFile(string filePath, string encoding = "UTF-8")
        {
            string result = null;
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(filePath, UnicodeEncoding.GetEncoding(encoding));
                result = reader.ReadToEnd();
            }
            catch { }
            if (reader != null)
                reader.Dispose();
            return result;
        }

        #endregion

        #region XML日志

        /// <summary>
        /// 记录不包含子节点简单格式XML日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="subfolder"></param>
        public static void WriteSampleXmlLog<T>(T item, string subfolder = "")
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);
            XmlElement root;
            try { root = doc.CreateElement(typeof(T).ToString()); }
            catch { root = doc.CreateElement("DefaultRootNode"); }
            doc.AppendChild(root);

            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);
            PropertyInfo[] Props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in Props)
            {
                string ElementName = p.Name.Substring(0);
                object ElementValue = p.GetValue(item, null);
                XmlElement element = doc.CreateElement(ElementName);
                element.InnerText = ElementValue == null ? "" : ElementValue.ToString();
                root.AppendChild(element);
            }
            string Path = LogPath + "\\" + subfolder;
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
            doc.Save(Path + (subfolder != "" ? "\\" : "") + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml");
        }

        /// <summary>
        /// 读取指定文件夹中指定数目的XML日志
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="subfolder"></param>
        /// <returns></returns>
        public static List<XmlDocument> ReadXmlLogs(int amount, string subfolder = "")
        {
            List<XmlDocument> Result = new List<XmlDocument>();
            try
            {
                string[] Files = System.IO.Directory.GetFiles(LogPath + (string.IsNullOrWhiteSpace(subfolder) ? "" : "\\") + subfolder, "*.xml");
                if (amount > Files.Length)
                    amount = Files.Length;
                for (int i = Files[0].Length - 1; i >= Files.Length - amount; i--)
                {
                    XmlDocument Doc = new XmlDocument();
                    Doc.Load(Files[i]);
                    Result.Add(Doc);
                }
            }
            catch { }
            return Result;
        }

        /// <summary>
        /// 读取指定文件夹中指定数目的日志实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="amount"></param>
        /// <param name="subfolder"></param>
        /// <returns></returns>
        public static List<T> ReadXmlLogs<T>(int amount, string subfolder = "")
        {
            List<T> Result = new List<T>();
            XmlReader xr = null;
            try
            {
                string[] Files = System.IO.Directory.GetFiles(LogPath + (string.IsNullOrWhiteSpace(subfolder) ? "" : "\\") + subfolder, "*.xml");
                if (amount > Files.Length)
                    amount = Files.Length;
                for (int i = Files.Length - 1; i >= Files.Length - amount; i--)
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    xr = XmlReader.Create(Files[i], null);
                    T x = (T)serializer.Deserialize(xr);
                    Result.Add(x);
                }
            }
            catch { }
            if (xr != null)
                xr.Close();
            return Result;
        }

        /// <summary>
        /// 写可序列化实体的XML日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <param name="subfolder"></param>
        public static void WriteSerXmlLog<T>(T item, string subfolder = null, string fileName = null)
        {
            XmlDocument xml = new XmlDocument();
            XmlWriter xw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                string dir = LogPath;
                if (!string.IsNullOrWhiteSpace(subfolder))
                    dir += "\\" + subfolder;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                if (string.IsNullOrWhiteSpace(fileName))
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml";
                xw = XmlWriter.Create(Path.Combine(dir, fileName));
                serializer.Serialize(xw, item);
            }
            catch { }
            if (xw != null)
                xw.Close();
        }

        /// <summary>
        /// 读可序列化实体的XML日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T ReadSerXmlLog<T>(string path)
        {
            T result = default(T);
            XmlReader xr = null;
            try
            {
                if (File.Exists(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    xr = XmlReader.Create(path, null);
                    result = (T)serializer.Deserialize(xr);
                }
            }
            catch { }
            if (xr != null)
                xr.Close();
            return result;
        }

        #endregion

        private class Log
        {
            public string DirPath { get; set; }

            public string FileName { get; set; }

            public string Content { get; set; }
        }

        private class LogQueue
        {
            public LogQueue()
            {
                workIndex = 0;
                queues = new Queue<Log>[] { new Queue<Log>(), new Queue<Log>() };
            }

            private int workIndex { get; set; }

            private Queue<Log>[] queues { get; set; }

            public Queue<Log> StorageQueue
            {
                get { return queues[1 - workIndex]; }
            }

            public Queue<Log> WorkingQueue
            {
                get { return queues[workIndex]; }
            }

            public void SwitchQueue()
            {
                workIndex = 1 - workIndex;
            }
        }

    }


    public partial class LogInfo
    {
        public DateTime Time { get; set; }

        public string Message { get; set; }

        public LogType Type { get; set; }

        public virtual string[] ToStringArray()
        {
            string[] result = new string[3];
            DateTime d = DateTime.Now;
            if (Time != default(DateTime))
                d = Time;
            result[0] = "Time:\t" + d.ToString("yyyy-MM-dd HH:mm:ss.fff");
            result[1] = "Message:\r\n" + Message;
            result[2] = "";
            return result;
        }
    }

    public enum LogType
    {
        Debug = 0,
        Error = 1,
        Warn = 2,
        Info = 3
    }
}
