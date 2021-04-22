using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


/** 
 * 该应用是从Java版本转换过来的AES-SHA1PRNG-128加密算法 , 该项目中包含的AES+Base64的算法.
 * 原需求如下:
 * data字段为加密数据，加密策略：
    1.	对源明文数据字符串A进行Base64，得到字符串B。
    2.	对字符串B通过AES-SHA1PRNG-128加密，得到byte[] C。
    3.	对byte[] C进行Base64得到的字符串就是data。
 * 
 * JAVA转C#的问题点在于Key的设定. 实际上传入的明文密钥需要进行 GetAesSha1PrngKey(seed) , 这样得到的结果才是AES加密中真正使用到的Key
 *  
 */
namespace _7.AES
{

    class Program
    {
        private static string aesKey = "0123456789ABCDEF";
        private static Encoding encoding = Encoding.UTF8;


        static void Main(string[] args)
        {
            //data = Base64(AES(Base64(json)));
            //data= GmPTFiiStyRdWeE2pmMpLw==
            string json = "xyz123";
            Console.WriteLine("明文json:" + json);

            string a = Base64Encode(json);
            Console.WriteLine("json的Base64:" + a);

            byte[] b = AesEncrypt(a); // AES加密
            string c = Base64Encode(b);
            Console.WriteLine("Base64(AES加密):" + c);


            Console.WriteLine("-----------------------------我是分割线-----------------------------");

            //先解base64
            byte[] unc = Base64Decode(c);
            string bb = AesDecypt(unc);//解密
            Console.WriteLine("AES解密:" + bb);

            byte[] cc = Base64Decode(bb);//解密
            Console.WriteLine("EnBase64(AES解密):" + encoding.GetString(cc));

            Console.ReadKey();
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encrypt">要加密的内容</param>
        /// <returns></returns>
        public static byte[] AesEncrypt(string encrypt)
        {
            using (RijndaelManaged cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.ECB;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = GetAesSha1PrngKey(aesKey);
                //cipher.IV = GetAesSha1PrngKey(secretKey);

                byte[] valueBytes = encoding.GetBytes(encrypt);

                byte[] encrypted;
                using (ICryptoTransform encryptor = cipher.CreateEncryptor())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream writer = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(valueBytes, 0, valueBytes.Length);
                            writer.FlushFinalBlock();
                            encrypted = ms.ToArray();
                            return encrypted;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encrypted">要解密的密文</param>
        /// <returns></returns>
        public static string AesDecypt(byte[] encrypted)
        {
            using (RijndaelManaged cipher = new RijndaelManaged())
            {
                cipher.Mode = CipherMode.ECB;
                cipher.Padding = PaddingMode.PKCS7;
                cipher.KeySize = 128;
                cipher.BlockSize = 128;
                cipher.Key = GetAesSha1PrngKey(aesKey);
                //cipher.IV = GetAesSha1PrngKey(secretKey);

                using (ICryptoTransform decryptor = cipher.CreateDecryptor())
                {
                    using (MemoryStream msDecrypt = new MemoryStream(encrypted.ToArray()))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 对明文密钥进行二次处理,在此方法返回得到的才是真正的密钥Key
        /// </summary>
        /// <param name="seed"></param>
        /// <returns></returns>
        private static byte[] GetAesSha1PrngKey(string seed)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] result = new byte[16];
                Array.Copy(sha1.ComputeHash(sha1.ComputeHash(encoding.GetBytes(seed))), result, result.Length);
                return result;
            }
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="content">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(object content)
        {
            string encode = string.Empty;
            byte[] bytes = null;
            if (content.GetType() == typeof(string)) bytes = encoding.GetBytes((string)content);
            else bytes = (byte[])content;

            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                encode = "Base64加密异常:" + e.Message + (string)content;
            }
            return encode;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="content">待解密的密文</param>
        /// <returns></returns>
        public static byte[] Base64Decode(string content)
        {
            return Convert.FromBase64String(content);
        }
    }
}
