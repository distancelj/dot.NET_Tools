using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rename
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"E:\Devloper Tools\音乐迷1.3\音乐迷\lyric";
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fi = di.GetFiles();
            foreach (FileInfo filename in fi)
            {
                string f1 = filename.Name;
                Console.WriteLine("调整前" + f1);
                string tempname = f1.Substring(6, filename.Name.Length - 6);
                Console.WriteLine("临时文件名" + tempname);
                filename.MoveTo(path + tempname);
                Console.WriteLine("调整后" + path + tempname + "\n");
            }

            Console.ReadKey();
        }
    }
}
