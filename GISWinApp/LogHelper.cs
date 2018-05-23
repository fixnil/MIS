using GISCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISWinApp
{
    public class LogHelper
    {
        static LogHelper()
        {
            IsFirstTime = !File.Exists(LOG_PATH);
        }

        public static readonly string LOG_PATH = Path.Combine(AppContext.BaseDirectory, GISConst.AppData);

        private static string GetCode()
        {
            if (!File.Exists(LOG_PATH))
            {
                using (var sw = File.CreateText(LOG_PATH))
                {
                    sw.Write("00");
                }
            }

            var code = File.ReadAllText(LOG_PATH);

            if (string.IsNullOrWhiteSpace(code) || code.Length != 2)
            {
                code = "00";
            }

            return code;
        }

        private static void Log(LogType type)
        {
            // 00 未操作
            // 01 web 复制成功
            // 10 数据库迁移成功
            // 11 web 和数据库迁移成功

            var code = GetCode().ToArray();

            if (type == LogType.web)
            {
                code[1] = '1';
            }
            else if (type == LogType.db)
            {
                code[0] = '1';
            }
            else if (type == LogType.rsweb)
            {
                code[1] = '0';
            }

            File.WriteAllText(LOG_PATH, string.Join("", code));
        }

        public static bool IsSuccess => GetCode() == "11";

        public static void CopyWebOk()
        {
            Log(LogType.web);
        }

        public static void MigrateDbOk()
        {
            Log(LogType.db);
        }

        public static void ReSelectWeb()
        {
            Log(LogType.rsweb);
        }

        public static bool WebIsOk => GetCode()[1] == '1';

        public static bool DbIsOk => GetCode()[0] == '1';

        public static bool IsFirstTime { get; }
    }

    public enum LogType
    {
        // web 复制成功
        web,
        // 数据库迁移成功
        db,
        // 撤销 web 复制
        rsweb
    }
}
