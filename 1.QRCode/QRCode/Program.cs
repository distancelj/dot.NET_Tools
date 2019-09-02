using System.Drawing;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;//引用的第三方dll

namespace QRCode
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = "嘿嘿嘿!\n科为创通科技股份有限公司(techcreate)\n项目总监:赵鲲翔";
            QRCodeEncoderUtil(content);
        }

        /// <summary> 
        /// 生成二维码 
        /// </summary> 
        /// <param name="qrCodeContent">要编码的内容</param> 
        /// <returns>返回二维码位图</returns> 
        public static Bitmap QRCodeEncoderUtil(string qrCodeContent)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeScale = 5;
            qrCodeEncoder.QRCodeVersion = 0;
            Bitmap img = qrCodeEncoder.Encode(qrCodeContent, Encoding.UTF8);//指定utf-8编码， 支持中文 
            img.Save(@"E:\temp.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return img;
        }

        /// <summary> 
        /// 解析二维码 
        /// </summary> 
        /// <param name="bitmap">要解析的二维码位图</param> 
        /// <returns>解析后的字符串</returns> 
        public static string QRCodeDecoderUtil(Bitmap bitmap)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new QRCodeBitmapImage(bitmap), Encoding.UTF8);//指定utf-8编码， 支持中文
            return decodedString;
        }
    }
}
