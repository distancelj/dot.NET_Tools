using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace _6.JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime time = DateTime.Now;
            Console.WriteLine(time.ToLocalTime());
            Console.WriteLine(time.ToString("yyyyMMddHHmmss"));

            string str = "1AE7239D848FD618F7E26ADCF18B1885:" + time.ToString("yyyyMMddhhmmss");
            Console.WriteLine(str);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            string converted = Convert.ToBase64String(bytes);
            Console.WriteLine("Base64:" + converted);

            Console.WriteLine("UnBase64" + Encoding.UTF8.GetString(Convert.FromBase64String(converted)));
            Console.WriteLine();

            string pwd = "";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            Console.WriteLine("MD5:" + pwd);

            Console.ReadKey();


            //string temp = "{\"预约日期\":\"\",\"职业\":{\"11代码\":{\"15sEff\":\"151515\"},\"名称\":\"无业游民\"},\"生日\":\"\",\"联系方式\":\"13429113802\",\"身份证号码\":\"\",\"性别\":\"男\",\"姓名\":\"杭白\",\"婚姻状况\":\"已婚\",\"手机号\":\"13429113802\",\"月收入\":\"\",\"年龄\":\"70\"}";
            string temp = "{\"msg\":\"success\",\"code\":\"0\",\"data\":{\"result\":[{\"groupName\":\"技能组名称\",\"callinNum\":\"来电量（转座席电话全量）\",\"answerNum\":\"接听量\",\"waittingNum\":\"当前等待量\",\"answerEff\":\"接通率\",\"15sEff\":\"服务水平（15s接通率）\"}],\"pageSize\":\"10\",\"total\":\"10\",\"pageNumber\":\"1\",\"pageTotal\":\"1\"}}";

            JObject res = JsonConvert.DeserializeObject(temp) as JObject;
            Console.WriteLine(res["msg"].ToString());
            Console.WriteLine(res["code"].ToString());
            Console.WriteLine(res["data"]["result"][0]["groupName"].ToString());
            Console.WriteLine(res["data"]["result"][0]["15sEff"].ToString());

            //Console.WriteLine(res["职业"]["11代码"] != null ? res["职业"]["11代码"]["15sEff"].ToString() : "null111");

            //Console.WriteLine(res["姓名"] != null ? res["姓名"].ToString() : "null111");
            //Console.WriteLine(res["备注"] != null ? res["备注"].ToString() : "null111");
            //if (res["备注"] != null && !string.IsNullOrEmpty(res["备注"].ToObject<string>()))
            //    res["备注"].ToString();


            //if (!string.IsNullOrEmpty(res["年龄"].ToObject<string>()))
            //{
            //    Console.WriteLine("年龄:" + Int32.Parse(res["年龄"].ToString()));
            //}
            //if (!string.IsNullOrEmpty(res["生日"].ToObject<string>()))
            //{
            //    Console.WriteLine("生日" + DateTime.Parse(res["生日"].ToString()));
            //}
            Console.ReadKey();
        }
    }
}
