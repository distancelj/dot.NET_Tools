# .NET-Tools
.net平台下的小工具类,使用C#语言开发的前/后端小工具类.

##1.生成二维码
ThoughtWorks.QRCode.dll是第三方组件.

##2.HttpServer
Http服务器,用于获取HttpWebRequest请求.
输出结果在console中会打印,并且会存储在文件中: E:\httplogs.txt

##3.MyHttpClient
Http请求,使用JSON格式传输报文体.
内容使用的是方正人寿的接口.

##4.TripleDES
TripleDES加密,解密.
该加密算法是DES加密算法的升级版,三重DES加密.密钥采用24位,短了异常,长了截取.
该程序中对结果还进行了Hex(十六进制),也可以改成Base64.