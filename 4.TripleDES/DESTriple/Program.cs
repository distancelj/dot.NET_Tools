using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TripleDES
{
    class Program
    {
        static Encoding utf8 = Encoding.UTF8;

        static void Main(string[] args)
        {
            string text = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ORDER><ORDERNO>今天是个好日子</ORDERNO><ORDERTYPE>A01</ORDERTYPE><VIPCODE>QN</VIPCODE><VIPPASSWORD>QN201710260917</VIPPASSWORD></ORDER>";
            string key = "E61F4AD90161A7615DA131C2F4021C867332FDC15BE9BF1A";// 长度控制为24，作为3DES加密用的key

            Console.WriteLine("ECB模式:");

            byte[] str1 = Des3EncodeECB(key, text);
            string encriptResult = ToHex(str1);
            Console.WriteLine(encriptResult);

            encriptResult = "2BA87488BFCE0CB3";
            byte[] str2 = Des3DecodeECB(key, encriptResult);
            Console.WriteLine(Encoding.GetEncoding("GBK").GetString(str2));

            Console.ReadLine();
        }

        #region ECB模式  
        /// <summary>  
        /// DES3 ECB模式加密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">明文的byte数组</param>  
        /// <returns>密文的byte数组</returns>  
        public static byte[] Des3EncodeECB(string stringKey, string content)
        {
            byte[] iv = new byte[] { };
            byte[] key = UnHex(stringKey);
            if (key.Length < 24) throw new InvalidDataException("密钥长度有误.");
            else key = key.Skip(0).Take(24).ToArray();

            try
            {
                // Create a MemoryStream.  
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                // Create a CryptoStream using the MemoryStream and the passed key and initialization vector (IV).  
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.  
                cStream.Write(utf8.GetBytes(content), 0, utf8.GetBytes(content).Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the MemoryStream that holds the encrypted data.  
                byte[] ret = mStream.ToArray();
                cStream.Close();
                mStream.Close();

                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("加密过程异常: {0}", e.Message);
                return null;
            }
        }
        /// <summary>  
        /// DES3 ECB模式解密  
        /// </summary>  
        /// <param name="key">密钥</param>  
        /// <param name="iv">IV(当模式为ECB时，IV无用)</param>  
        /// <param name="str">密文的byte数组</param>  
        /// <returns>明文的byte数组</returns>  
        public static byte[] Des3DecodeECB(string stringKey, string content)
        {
            byte[] desContent = UnHex(content);

            byte[] iv = new byte[] { };
            byte[] key = UnHex(stringKey);
            if (key.Length < 24) throw new InvalidDataException("密钥长度有误.");
            else key = key.Skip(0).Take(24).ToArray();
            try
            {
                // Create a new MemoryStream using the passed   
                // array of encrypted data.  
                MemoryStream msDecrypt = new MemoryStream(desContent);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider
                {
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };
                // Create a CryptoStream using the MemoryStream   
                // and the passed key and initialization vector (IV).  
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.  
                byte[] fromEncrypt = new byte[desContent.Length];
                // Read the decrypted data out of the crypto stream  
                // and place it into the temporary buffer.  
                csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.  
                return fromEncrypt;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("解密过程异常: {0}", e.Message);
                return null;
            }
        }
        #endregion

        /// <summary>
        /// 转化为十六进制
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder(32);
            for (int i = 0; i < bytes.Length; i++)
            {
                int v = bytes[i] & 0xFF;
                string hv = v.ToString("X2").ToUpper();

                if (hv.Length < 2)
                {
                    stringBuilder.Append(0);
                }
                stringBuilder.Append(hv);
            }
            return stringBuilder.ToString().ToUpper();
        }

        /// <summary>
        /// 将十六进制数转化为字节
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] UnHex(string hexString)
        {
            if (hexString == null || "".Equals(hexString))
            {
                return null;
            }

            // 去除空格、制表符、换行、回车
            hexString = hexString.Replace("\\s*|\t|\r|\n", "").ToUpper();
            hexString = hexString.Replace(" ", "");
            int length = hexString.Length / 2;
            char[] hexChars = hexString.ToCharArray();
            byte[] bytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                int pos = i * 2;
                byte t = (byte)(CharToByte(hexChars[pos]) << 4 | CharToByte(hexChars[pos + 1]));
                bytes[i] = t;
            }
            return bytes;
        }

        public static byte CharToByte(char i)
        {
            return (byte)"0123456789ABCDEF".IndexOf(i);
        }

    }
}
