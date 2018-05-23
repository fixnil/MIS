# MIS

基于 Zigbee 的生态养猪场信息管理系统，包含上位机和 Web 部分。

说明：当时命名时匆忙将 MIS 写成了 GIS，后便一直使用该名称。

该项目共包含三个部分，GISCore(核心，公共的)、GISWebApp(Web部分)、GISWinApp(WinForm部分)。

整个项目并没有什么亮点，就是简单的串口读取数据，保存到数据库中，网页显示；

但是整个项目结构却是很有意思

通过 asp.net core 的自带宿主发布方式发布，使用者并不需要安装 Web 服务器和 .net 环境。
通过 ef 的 Migrate 方法自动迁移数据库，不需要将数据库拷贝给客户或者要求使用者新建数据库。
通过上位机启动 Web，并打开浏览器，更加方便。

总的来说，使用者只需在第一次运行时选择一下 Web 文件夹，就会自动配置好 Web 和 数据库。
之后每次启动程序均会自动启动 Web 并且打开浏览器。

要完成以上的操作，需要建立一个 .net standard 项目存放数据库上下文，供 .net core 的 GISWebApp 和 .net framework 的 GISWinApp 使用。
发布 GISWebApp 需要使用命令

```bash
    dotnet publish -r:win-x64
```

值得注意的是，调用 Migrate 方法迁移数据库依赖之前有没有执行 Add-Migration 命令生成相应的迁移类
由于微软取消了直接在 .net standard 中直接迁移数据库的功能，所以需要一系列复杂的操作

1. 根据微软提示，在上下文中配置 optionsBuilder.UseSqlServer(constr, b => b.MigrationsAssembly("GISWebApp"));
2. 设置 GISWebApp 为启动项目，到 GISWebApp 项目中 执行 Add-Migrations 命令；
3. 将生成的 Migrations 文件夹拷贝到上下文所在的类库项目中；
4. 去掉步骤 2 中的修改；
5. 现在便可以在 winform 中调用 Migrate 方法了。

发布步骤

1. 在命令行执行发布命令发布 GISWebApp；
2. 直接在 VS 中发布 GISWinApp；
3. 双击步骤 2 中的到的安装程序；
4. 安装后运行程序，选择步骤 1 中得到的 publish 文件以配置 Web。