using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMP.Core.extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 최대 byte 길이를 넘지않게 문자열 자르기
        /// </summary>
        /// <param name="string"></param>
        /// <param name="maxByteLength">최대 byte 길이</param>
        /// <returns></returns>
        public static string WithMaxByteLength(this string @string, int maxByteLength)
        {
            for (int i = @string.Length; i >= 0; i--)
            {
                if (Encoding.UTF8.GetByteCount(@string.Substring(0, i + 1)) <= maxByteLength)
                {
                    return @string.Substring(0, i + 1);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 최대 길이를 넘지않게 문자열 자르기
        /// </summary>
        /// <param name="string"></param>
        /// <param name="maxLength">최대 길이</param>
        /// <returns></returns>
        public static string WithMaxLength(this string @string, int maxLength)
        {
            return @string?.Substring(0, Math.Min(@string.Length, maxLength));
        }
    }
}
