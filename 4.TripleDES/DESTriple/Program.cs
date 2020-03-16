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

            encriptResult = "ED7AE4AFE62D55BCC69FAF3B10B3885743A05077B3EE548A89C952FFB360F75A993066E4D2DD8226C7773A143AB3D3E25E87A08D2FE6E6E35E87A08D2FE6E6E34504933961209CE7395FD4E87546FEA47E214DC6CB2829F8BFEB9BD4F777F8352E47D0D2464D1BE57C7A86980C31551258CAF4DA7ECC82271F4AC027DEBCACB9C214233166CFA0ACE92AACC2BD7F0541076848579F451C9D945DF09DE6EDD7FE6D5E36FF87BFE41F5584BD08DD6DB9E491CD6DA5D0C93CA533C477F4C9EAA939F7D7E19DE699F14B71DE3B0ACDB09735182F940DFE9C9724A10E0C6D4E287008EAED52520E03B7108510801E72BDB98CDAD3869CCEA938B33DF05F8DA6018D7F0A896AAAF25E6B1755177B5191AA42FD8EE1191427A18756D775D3BEFF3A1B51AFBA416D0A62DA9EAC4E3AD148843A97CFFACD30B13A1C614B1D976F527B27A107091FF901FF62FAAB2645948D1C4887B5371C379DF5444406E5B185D7CFB5A0EAEF87EF76DE9E2C86635BA31D0E0F9A8F0871F8420FCA6D8203CFAE08243C019982AD87A7BFD37A19F785DFCB1BB30D9EF48D23E74FA7633E34A0DA24C0ACACB497F5D6BDE79FE8B17779A20BB4387B9FF36CEF0CC60CDFB8253EDF71DA12283903885E17EEFF2D37E802CB7A614B352BC414E7667289DB9A37B431B5A3C75F2DB6D9827C994BD6FAC117417C830A08CAA4854E4E9571A140325852C734AD0C0289D39C02E8C7E2CFFACD30B13A1C61B6AFCFBA0BF26333F4E4960EC3662E03081862B0EB022B13F53F8A01F92162F25335B1AF1E7AA2AC081350CB3A3FA9593F8A3CCC9052287534CB22F38D604EA8D5A40C712F7AF4582E90C6D2022BAB4A93065CA103C949D4635DDFCCAA2319F17D0CE6F58E802FDB7A6F9F88DBE09352546150213E4F188FC943C44C11E694D196C8C73A19D08FEB33509D1D2AEC327FC24315F02CCD250C2289DB30B1F99A7ACD27E2B9C9E843FFEA48C81A51072609226A9786B65C72BC9873849C2B728A13DF75C60B4B2EF778F1972ADF022ED42EEC1142CDE4096FA6D1275EF11C9D9E4461BA8DDD720FDC618118CF5D324CF0C9D96932F0CD47708E20E91239DE38088FBFF29268591AB51A65BE0E66BD0DC68D59248E170596F5EC6CC831B06BA7527F853AD051BB8322263F1E9E6620108FE2";
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
