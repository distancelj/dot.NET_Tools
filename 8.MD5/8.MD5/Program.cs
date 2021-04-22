using System;
using System.Text;

namespace _8.MD5
{
    class Program
    {
        static void Main(string[] args)
        {
            //bb=MD5(Base64(json)+key);
            //sign是MD5签名，签名策略：
            //1.对源明文数据字符串A进行Base64，得到字符串B。
            //2.在字符串B后面拼上MD5 - KEY，得到字符串C。
            //3.对字符串C进行MD5，得到的字符串就是sign的值
            //sign = 1d9a238d95ba673d21b2c490d6c1380a
            string a = "xyz123";
            string md5Key = "12345";
            string origin = "1d9a238d95ba673d21b2c490d6c1380a";
            Console.WriteLine("明文:" + a);


            string b = Base64Helper.Base64Encode(Encoding.UTF8, a);
            Console.WriteLine("Base64后的b:" + b);

            string c = b + md5Key;
            Console.WriteLine("拼接后的c:" + c);

            string d = GetMD5(c);
            Console.WriteLine("对c进行md5:" + d);

            Console.WriteLine("是否与原始签名匹配:" + origin.Equals(d));

            Console.ReadKey();
        }

        /// <summary>
        /// 获取MD5值
        /// </summary>
        /// <param name="source">加密的字符串</param>
        /// <returns>返回MD5值</returns>
        public static string GetMD5(string source)
        {
            byte[] sor = Encoding.UTF8.GetBytes(source);
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位

            }
            return strbul.ToString();
        }
    }


    class Base64Helper
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode = string.Empty;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encodeType">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string Base64Decode(Encoding encodeType, string result)
        {
            string decode = string.Empty;
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encodeType.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }
    }
}
