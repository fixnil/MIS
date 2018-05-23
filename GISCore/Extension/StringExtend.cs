using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GISCore
{
    public static class StringExtend
    {
        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="source">数据</param>
        /// <returns></returns>
        public static string Encryption(this string source)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(source))).Replace("-", "");
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="source">数据</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Encryption(this string source, string salt)
        {
            return (source + salt.Encryption()).Encryption();
        }
    }
}
