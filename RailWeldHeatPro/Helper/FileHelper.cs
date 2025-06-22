using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RailWeldHeatPro.Helper
{
    public static class FileHelper
    {
        //写出
        public static void WriteToFile(string path, string content, bool isAppend = false)
        {
            //创建文件流，写出器，写出，关闭写出器，关闭文件流
            FileStream fileStream = new FileStream(path, isAppend ? FileMode.Append : FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(content);
            streamWriter.Close();
            fileStream.Close();
        }
        //读入
        public static string ReadFromFile(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            string content = streamReader.ReadToEnd();
            streamReader.Close();
            fileStream.Close();
            return content;
        }
    }
}
