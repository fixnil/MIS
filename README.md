# MIS

该项目是一个基于 Zigbee 的生态养猪场信息管理系统，仅含上位机和网站部分。

**说明：当时命名时匆忙将 MIS 写成了 GIS，后便一直使用该名称。**

## 对使用者友好

整个项目并没有什么亮点，就是简单的串口读取数据，保存到数据库中，网页显示；但是整个**项目结构**却是很有意思：

1. 通过 asp.net core 的自带宿主发布方式发布网站，使用者并不需要安装 web 服务器和 .net 环境；
2. 通过 ef 的 migrate 方法自动迁移数据库，不需要将数据库拷贝给使用者；
3. 通过上位机启动网站，并打开浏览器，一件操作。

总的来说，使用者只需在第一次运行时选择一下 web 文件夹，就会自动配置好 web 和 数据库。
之后每次启动程序均会自动启动 web 并且打开浏览器。

## 项目结构

要完成以上的操作，需要建立一个 .net standard 类库(GISCore)存放数据库上下文，供 .net core 的网站(GISWebApp) 和 .net framework 的 Winform(GISWinApp) 引用。

## 迁移数据库

值得注意的是，调用 Migrate 方法迁移数据库依赖执行 `Add-Migration` 命令后生成的迁移类，由于微软取消了接在 .net standard 中直接迁移数据库的功能，所以需要一系列骚操作：

1. 根据微软提示，在上下文中配置 `optionsBuilder.UseSqlServer(constr, b => b.MigrationsAssembly("GISWebApp"))`;
2. 设置 GISWebApp 为启动项目，到 GISWebApp 项目中执行 `Add-Migrations` 命令；
3. 将生成的 migrations 文件夹拷贝到上下文所在的类库项目中；
4. 去掉步骤 2 中的修改；
5. 现在便可以在 winform 中调用 migrate 方法了。

## 发布步骤

1. 在命令行执行发布命令 `dotnet publish -r:win-x64` 发布 GISWebApp；
2. 直接在 VS 中发布 GISWinApp；
3. 双击步骤 2 中的到的安装程序；
4. 安装后运行程序，选择步骤 1 中得到的 publish 文件以配置网站。

## 使用步骤

1. 按操作配置网站和数据库
2. 
3. 

## 数据格式

数据格式(以 $ 结束)：温度,湿度,NH3,光照[,警报]$

例如: 23,60,100,1000[,1]$

说明：(,为半角); [] 表示可选

## 演示

[好像 Markdown 不能链接视频](http://t.cn/R3swG9R)