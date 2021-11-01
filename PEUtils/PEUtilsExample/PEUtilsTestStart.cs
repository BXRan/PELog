/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2020/10/13 19:50
	功能: 测试案例启动入口

    //=================*=================\\
           关注微信公众号: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System;
using PEUtils;

namespace PEUtilsExample {
    class Root {
        public void Init() {
            this.Log("Init Root Log.");
            Mgr mgr = new Mgr();
            mgr.Init();
        }
    }
    class Mgr {
        public void Init() {
            this.Warn("Init Mgr Warn");
            Item item = new Item();
            item.Init();
        }
    }
    class Item {
        public void Init() {
            this.Error("Init Item Error");
            this.Trace("Trace This Function.");
        }
    }
    class PEUtilsTestStart {
        static void Main(string[] args) {
            PELog.InitSettings();
            PELog.Log("{0} start...", "ServerPELog");
            PELog.ColorLog(LogColor.Red, "Color Log:Red.");
            PELog.ColorLog(LogColor.Green, "Color Log:Green.");
            PELog.ColorLog(LogColor.Blue, "Color Log:Blue.");
            PELog.ColorLog(LogColor.Cyan, "Color Log:Cyan.");
            PELog.ColorLog(LogColor.Magenta, "Color Log:Magenta.");
            PELog.ColorLog(LogColor.Yellow, "Color Log:Yellow.");
            PELog.ColorLog(LogColor.Green, "\n{0}，专注实战，只讲干货。\nEmail:{1}", "Plane老师", "1785275942@qq.com");
            Root rt = new Root();
            rt.Init();

            Console.ReadKey();
        }
    }
}
