using System;
using System.Collections.Generic;
using System.Text;

namespace GISCore
{
    public static class GISConst
    {
        // log 文件
        public const string AppData = "srs.data";
        // web 文件名
        public const string WebAppName = "GISWebApp.exe";
        // web 目录名
        public const string WebRoot = "Web";
        // 是否是 sqlserver
        public const string IsSql = "false";
        // localdb 连接字符串
        public const string local = "Server=(localdb)\\mssqllocaldb;Database=GISContext;Trusted_Connection=True;MultipleActiveResultSets=true";
        // sqlserver 连接字符串
        public const string sql = "Server=.;Database=GISContext;Trusted_Connection=True;MultipleActiveResultSets=true";
        // 打开浏览器命令
        public const string command = "/c start http://localhost:5000/info";

        // 单位
        public const string CO2 = "  ppm";
        public const string Hum = "  %RH";
        public const string Light = "  Lx";
        public const string NH3 = "  ppm";
        public const string Temp = "  ℃";
    }
}
