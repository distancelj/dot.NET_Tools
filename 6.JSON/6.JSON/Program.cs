using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace _6.JSON
{
    class Program
    {
        static void Main(string[] args)
        {


            string temp = "{\"预约日期\":\"\",\"职业\":{\"代码\":\"001\",\"名称\":\"无业游民\"},\"生日\":\"\",\"联系方式\":\"13429113802\",\"身份证号码\":\"\",\"性别\":\"男\",\"姓名\":\"杭白\",\"婚姻状况\":\"已婚\",\"手机号\":\"13429113802\",\"月收入\":\"\",\"年龄\":\"70\"}";

            JObject res = JsonConvert.DeserializeObject(temp) as JObject;

            Console.WriteLine(res["姓名"] != null ? res["姓名"].ToString() : "null111");
            Console.WriteLine(res["备注"] != null ? res["备注"].ToString() : "null111");
            if (res["备注"] != null && !string.IsNullOrEmpty(res["备注"].ToObject<string>()))
                res["备注"].ToString();

            if (!string.IsNullOrEmpty(res["年龄"].ToObject<string>()))
            {
                Console.WriteLine("年龄:" + Int32.Parse(res["年龄"].ToString()));
            }
            if (!string.IsNullOrEmpty(res["生日"].ToObject<string>()))
            {
                Console.WriteLine("生日" + DateTime.Parse(res["生日"].ToString()));
            }
            Console.ReadKey();
        }
    }
}
