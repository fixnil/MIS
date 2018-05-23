using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GISCore
{
    public class FileHepler
    {
        /// <summary>
        /// 复制指定目录下的文件到指定路径
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="logger">记录者(当前文件名, 当前进度)</param>
        public static void CopyDir(string sourceDir, string targetDir, Action<string> logger = null)
        {
            DirectoryInfo directory = new DirectoryInfo(sourceDir);

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // 复制文件
            foreach (var file in directory.GetFiles())
            {
                logger?.Invoke(file.FullName);

                File.Copy(file.FullName, Path.Combine(targetDir, file.Name), true);
            }

            // 复制文件夹
            foreach (var subDir in directory.GetDirectories())
            {
                CopyDir(subDir.FullName, Path.Combine(targetDir, subDir.Name), logger);
            }
        }

        /// <summary>
        /// 统计指定目录下的文件数
        /// </summary>
        /// <param name="dirPath">目录路径</param>
        /// <returns></returns>
        public static int CountFile(string dirPath)
        {
            int count = 0;

            DirectoryInfo directory = new DirectoryInfo(dirPath);

            count += directory.GetFiles().Count();

            foreach (var subDir in directory.GetDirectories())
            {
                count += CountFile(subDir.FullName);
            }

            return count;
        }
    }
}
