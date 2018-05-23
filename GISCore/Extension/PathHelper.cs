using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GISCore
{
    public class PathHelper
    {
        /// <summary>
        /// 连接两个地址，会处理 .. 的连接;
        /// 如："\\f\\u\\c\\k" + "../../" 为 \\f\\u
        /// </summary>
        /// <param name="source">源地址</param>
        /// <param name="path">影响地址</param>
        /// <returns></returns>
        public static string Combine(string source, string path)
        {
            source = source.TrimEnd('\\');

            (path.Contains("/") ? path.Split('/') : path.Split('\\')).ToList().ForEach(dir =>
            {
                source = dir == ".." ? source.Remove(source.LastIndexOf('\\')) : source + "\\" + dir;
            });

            return source;
        }
    }
}
