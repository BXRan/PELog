/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2020/10/13 22:00
	功能: 日志工具

    //=================*=================\\
           关注微信公众号: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;

public static class ExtensionMethonds {
    public static void Log(this object obj, object log) {
        PEUtils.PELog.Log(log);
    }
    public static void Log(this object obj, string msg, params object[] args) {
        PEUtils.PELog.Log(string.Format(msg, args));
    }
    public static void ColorLog(this object obj, PEUtils.LogColor color, object log) {
        PEUtils.PELog.ColorLog(color, log);
    }
    public static void ColorLog(this object obj, PEUtils.LogColor color, string msg, params object[] args) {
        PEUtils.PELog.ColorLog(color, string.Format(msg, args));
    }
    public static void Trace(this object obj, object log) {
        PEUtils.PELog.Trace(log);
    }
    public static void Trace(this object obj, string msg, params object[] args) {
        PEUtils.PELog.Trace(string.Format(msg, args));
    }
    public static void Warn(this object obj, object log) {
        PEUtils.PELog.Warn(log);
    }
    public static void Warn(this object obj, string msg, params object[] args) {
        PEUtils.PELog.Warn(string.Format(msg, args));
    }
    public static void Error(this object obj, string log) {
        PEUtils.PELog.Error(log);
    }
    public static void Error(this object obj, string msg, params object[] args) {
        PEUtils.PELog.Error(string.Format(msg, args));
    }
}
namespace PEUtils {
    public static class PELog {
        class UnityLogger : ILogger {
            Type type = Type.GetType("UnityEngine.Debug, UnityEngine");
            public void Log(string msg, LogColor color = LogColor.None) {
                if(color != LogColor.None) {
                    msg = ColorUnityLog(msg, color);
                }
                type.GetMethod("Log", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }
            public void Warn(string msg) {
                type.GetMethod("LogWarning", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }
            public void Error(string msg) {
                type.GetMethod("LogError", new Type[] { typeof(object) }).Invoke(null, new object[] { msg });
            }
            private string ColorUnityLog(string msg, LogColor color) {
                switch(color) {
                    case LogColor.Red:
                        msg = string.Format("<color=#FF0000>{0}</color>", msg);
                        break;
                    case LogColor.Green:
                        msg = string.Format("<color=#00FF00>{0}</color>", msg);
                        break;
                    case LogColor.Blue:
                        msg = string.Format("<color=#0000FF>{0}</color>", msg);
                        break;
                    case LogColor.Cyan:
                        msg = string.Format("<color=#00FFFF>{0}</color>", msg);
                        break;
                    case LogColor.Magenta:
                        msg = string.Format("<color=#FF00FF>{0}</color>", msg);
                        break;
                    case LogColor.Yellow:
                        msg = string.Format("<color=#FFFF00>{0}</color>", msg);
                        break;
                    case LogColor.None:
                    default:
                        break;
                }
                return msg;
            }
        }

        class ConsoleLogger : ILogger {
            public void Log(string msg, LogColor color = LogColor.None) {
                WriteConsoleLog(msg, color);
            }
            public void Warn(string msg) {
                WriteConsoleLog(msg, LogColor.Yellow);
            }
            public void Error(string msg) {
                WriteConsoleLog(msg, LogColor.Red);
            }
            private void WriteConsoleLog(string msg, LogColor color) {
                switch(color) {
                    case LogColor.Red:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Green:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Blue:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Cyan:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Magenta:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.Yellow:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogColor.None:
                    default:
                        Console.WriteLine(msg);
                        break;
                }
            }
        }

        private static ILogger logger;
        public static LogConfig cfg;
        private static StreamWriter LogFileWriter = null;
        private const string logLock = "PELogLock";

        public static void InitSettings(LogConfig cfg = null) {
            if(cfg == null) {
                cfg = new LogConfig();
            }
            PELog.cfg = cfg;

            if(cfg.loggerEnum == LoggerType.Console) {
                logger = new ConsoleLogger();
            }
            else {
                logger = new UnityLogger();
            }

            if(cfg.enableSave == false) {
                return;
            }
            if(cfg.enableCover) {
                string path = cfg.savePath + cfg.saveName;
                try {
                    if(Directory.Exists(cfg.savePath)) {
                        if(File.Exists(path)) {
                            File.Delete(path);
                        }
                    }
                    else {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFileWriter = File.AppendText(path);
                    LogFileWriter.AutoFlush = true;
                }
                catch(Exception e) {
                    LogFileWriter = null;
                }
            }
            else {
                string prefix = DateTime.Now.ToString("yyyyMMdd@HH-mm-ss");
                string path = cfg.savePath + prefix + cfg.saveName;
                try {
                    if(Directory.Exists(cfg.savePath) == false) {
                        Directory.CreateDirectory(cfg.savePath);
                    }
                    LogFileWriter = File.AppendText(path);
                    LogFileWriter.AutoFlush = true;
                }
                catch(Exception e) {
                    LogFileWriter = null;
                }
            }
        }

        /// <summary>
        /// 常规支持Format的日志
        /// </summary>
        public static void Log(string msg, params object[] args) {
            if(cfg.enableLog == false) {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            lock(logLock) {
                logger.Log(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[L]{0}", msg));
                }
            }
        }
        public static void Log(object obj) {
            if(cfg.enableLog == false) {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            lock(logLock) {
                logger.Log(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[L]{0}", msg));
                }
            }
        }
        /// <summary>
        /// 支持自定义颜色的日志
        /// </summary>
        public static void ColorLog(LogColor color, string msg, params object[] args) {
            if(cfg.enableLog == false) {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            lock(logLock) {
                logger.Log(msg, color);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[L]{0}", msg));
                }
            }
        }
        public static void ColorLog(LogColor color, object obj) {
            if(cfg.enableLog == false) {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            lock(logLock) {
                logger.Log(msg, color);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[L]{0}", msg));
                }
            }
        }
        /// <summary>
        /// 支持Format的堆栈日志
        /// </summary>:
        public static void Trace(string msg, params object[] args) {
            if(cfg.enableLog == false) {
                return;
            }
            msg = DecorateLog(string.Format(msg, args), cfg.enableTrace);
            lock(logLock) {
                logger.Log(msg, LogColor.Magenta);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[T]{0}", msg));
                }
            }
        }
        public static void Trace(object obj) {
            if(cfg.enableLog == false) {
                return;
            }
            string msg = DecorateLog(obj.ToString(), cfg.enableTrace);
            lock(logLock) {
                logger.Log(msg, LogColor.Magenta);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[T]{0}", msg));
                }
            }
        }
        /// <summary>
        /// 警告日志（黄色）
        /// </summary>
        public static void Warn(string msg, params object[] args) {
            if(cfg.enableLog == false) {
                return;
            }
            msg = DecorateLog(string.Format(msg, args));
            lock(logLock) {
                logger.Warn(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[W]{0}", msg));
                }
            }
        }
        public static void Warn(object obj) {
            if(cfg.enableLog == false) {
                return;
            }
            string msg = DecorateLog(obj.ToString());
            lock(logLock) {
                logger.Warn(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[W]{0}", msg));
                }
            }
        }
        /// <summary>
        /// 错误日志（红色，输出堆栈）
        /// </summary>
        public static void Error(string msg, params object[] args) {
            if(cfg.enableLog == false) {
                return;
            }
            msg = DecorateLog(string.Format(msg, args), cfg.enableTrace);
            lock(logLock) {
                logger.Error(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[E]{0}", msg));
                }
            }
        }
        public static void Error(object obj) {
            if(cfg.enableLog == false) {
                return;
            }
            string msg = DecorateLog(obj.ToString(), cfg.enableTrace);
            lock(logLock) {
                logger.Error(msg);
                if(cfg.enableSave) {
                    WriteToFile(string.Format("[E]{0}", msg));
                }
            }
        }

        //Tool
        private static string DecorateLog(string msg, bool isTrace = false) {
            StringBuilder sb = new StringBuilder(cfg.logPrefix, 100);
            if(cfg.enableTime) {
                sb.AppendFormat(" {0}", DateTime.Now.ToString("hh:mm:ss-fff"));
            }
            if(cfg.enableThreadID) {
                sb.AppendFormat(" {0}", GetThreadID());
            }
            sb.AppendFormat(" {0} {1}", cfg.logSeparate, msg);
            if(isTrace) {
                sb.AppendFormat("\nStackTrace:{0}", GetLogTrace());
            }
            return sb.ToString();
        }
        private static string GetThreadID() {
            return string.Format(" ThreadID:{0}", Thread.CurrentThread.ManagedThreadId);
        }
        private static string GetLogTrace() {
            StackTrace st = new StackTrace(3, true);//跳跃3帧
            string traceInfo = "";
            for(int i = 0; i < st.FrameCount; i++) {
                StackFrame sf = st.GetFrame(i);
                traceInfo += string.Format("\n    {0}::{1} Line:{2}", sf.GetFileName(), sf.GetMethod(), sf.GetFileLineNumber());
            }
            return traceInfo;
        }
        private static void WriteToFile(string msg) {
            if(cfg.enableSave && LogFileWriter != null) {
                try {
                    LogFileWriter.WriteLine(msg);
                }
                catch(Exception) {
                    LogFileWriter = null;
                    return;
                }
            }
        }

        //打印数组数据For Debug
        public static void PrintBytesArray(byte[] bytes, string prefix, Action<string> printer = null) {
            string str = prefix + "->\n";
            for(int i = 0; i < bytes.Length; i++) {
                if(i % 10 == 0) {
                    str += bytes[i] + "\n";
                }
                str += bytes[i] + " ";
            }
            if(printer != null) {
                printer(str);
            }
            else {
                Log(str);
            }
        }
    }
}