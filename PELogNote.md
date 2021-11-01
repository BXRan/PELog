# 开发跨平台通用日志插件

## Unity客户端与.net服务器日志工具（PELog）

> 作者：Plane
> 邮箱：1785275942@qq.com
> 微信：PlaneZhong
> 时间：2020/10/18 04:47

### 备课讲义提纲：

#### 课程介绍

##### 为什么需要一个通用日志工具？

- Unity自带日志工具限制：
  - 时间无毫秒显示，无色差控制，无线程数据；
  - 干扰信息过多，不方便分析查证；
- 为了与服务端统一日志格式与模版，方便分析定位问题。

##### 开发功能列表：

- 方便自定义前缀、间隔标记等格式；
- 附带时间信息，线程信息，堆栈信息；
- 支持多颜色区分显示，突显重要日志；
- 支持日志文件保存及覆盖；

##### 效果演示：

- Uniy中显示效果；
- 服务器控制台中显示效果；