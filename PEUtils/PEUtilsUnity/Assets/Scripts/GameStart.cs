/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com
	日期: 2020/10/14 14:02
	功能: 启动入口

    //=================*=================\\
           关注微信公众号: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using PEUtils;
using UnityEngine;

class Root {
    public void Init() {
        PELog.Log("Init Root Log.");
        Mgr mgr = new Mgr();
        mgr.Init();
    }
}
class Mgr {
    public void Init() {
        PELog.Warn("Init Mgr Warn");
        Item item = new Item();
        item.Init();
    }
}
class Item {
    public void Init() {
        PELog.Error("Init Item Error");
        PELog.Trace("Trace This Function.");
    }
}

public class GameStart : MonoBehaviour {
    void Start() {
        LogConfig cfg = new LogConfig {
            enableLog = true,
            logPrefix = "",
            enableTime = true,
            logSeparate = ">",
            enableThreadID = true,
            enableTrace = false,
            enableSave = true,
            enableCover = true,
            savePath = Application.persistentDataPath + "/PELog/",
            saveName = "ClientPELog.txt",
            loggerEnum = LoggerType.Unity,
        };
        PELog.InitSettings(cfg);

        PELog.Log("{0} start...", "ServerPELog");
        PELog.ColorLog(LogColor.Red, "Color Log:Red.");
        PELog.ColorLog(LogColor.Green, "Color Log:Green.");
        PELog.ColorLog(LogColor.Blue, "Color Log:Blue.");
        PELog.ColorLog(LogColor.Cyan, "Color Log:Cyan.");
        PELog.ColorLog(LogColor.Magenta, "Color Log:Magenta.");
        PELog.ColorLog(LogColor.Yellow, "Color Log:Yellow.");

        Root rt = new Root();
        rt.Init();
    }
}
