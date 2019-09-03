using System;
using System.IO;
using System.Net;
using System.Text;

namespace MyHttpServer
{
    class Program
    {
        //create listener instance
        static HttpListener listener = new HttpListener();

        static void Main(string[] args)
        {
            //init addr.
            listener.Prefixes.Add("http://127.0.0.1:8888/");//必须以'/'结尾

            //start
            listener.Start();

            //get context
            listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);//多个监听地址可以循环添加.
            Console.WriteLine("已启动监听，访问http://127.0.0.1:8888/");
            Console.ReadKey();

        }

        static void GetContextCallBack(IAsyncResult ar)
        {
            try
            {
                listener = ar.AsyncState as HttpListener;
                HttpListenerContext context = listener.EndGetContext(ar);
                //再次监听请求
                listener.BeginGetContext(new AsyncCallback(GetContextCallBack), listener);
                //处理请求
                string requestContent = Request(context.Request);
                byte[] data = Encoding.Default.GetBytes(requestContent);
                //存储到本地
                FileStream fs = new FileStream(@"E:\httplogs.txt", FileMode.OpenOrCreate);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
                Console.WriteLine(requestContent);
                //输出请求
                Response(context.Response, "response content is :\n" + requestContent);
            }
            catch { }
        }

        /// <summary>
        /// 处理输入参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        static string Request(HttpListenerRequest request)
        {
            string temp = "welcome to linezero!";
            if (request.HttpMethod.ToLower().Equals("get"))
            {
                //GET请求处理
                if (!string.IsNullOrEmpty(request.QueryString["linezero"]))
                    temp = request.QueryString["linezero"];
            }
            else if (request.HttpMethod.ToLower().Equals("post"))
            {
                //这是在POST请求时必须传参的判断默认注释掉
                //if (!request.HasEntityBody) 
                //{
                //	temp = "请传入参数";
                //	return temp;
                //}
                //POST请求处理
                Stream SourceStream = request.InputStream;
                byte[] currentChunk = ReadLineAsBytes(SourceStream);
                //获取数据中有空白符需要去掉，输出的就是post请求的参数字符串 如：username=linezero
                temp = Encoding.Default.GetString(currentChunk).Replace("", "");
            }
            return temp;
        }

        static byte[] ReadLineAsBytes(Stream SourceStream)
        {
            var resultStream = new MemoryStream();
            while (true)
            {
                int data = SourceStream.ReadByte();
                resultStream.WriteByte((byte)data);
                if (data <= 10)
                    break;
            }
            resultStream.Position = 0;
            byte[] dataBytes = new byte[resultStream.Length];
            resultStream.Read(dataBytes, 0, dataBytes.Length);
            return dataBytes;
        }


        /// <summary>
        /// 输出方法
        /// </summary>
        /// <param name="response">response对象</param>
        /// <param name="responseString">输出值</param>
        /// <param name="contenttype">输出类型默认为json</param>
        static void Response(HttpListenerResponse response, string responsestring, string contenttype = "application/json")
        {
            response.StatusCode = 200;
            response.ContentType = contenttype;
            response.ContentEncoding = Encoding.UTF8;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responsestring);
            //对客户端输出相应信息.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            //关闭输出流，释放相应资源
            output.Close();
        }
    }
}




//NameValueCollection postData = new NameValueCollection();
//if (context.Request.HasEntityBody)
//{
// System.IO.Stream body = context.Request.InputStream;
//// System.Text.Encoding encoding = context.Request.ContentEncoding;
//System.Text.Encoding encoding = Encoding.UTF8;
//System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
//string s = reader.ReadToEnd();
//body.Close();
// reader.Close();
// postData = System.Web.HttpUtility.ParseQueryString(s);
// //Console.WriteLine(postData.Get("name"));
//}