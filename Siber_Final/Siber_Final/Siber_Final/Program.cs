using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Siber_Final
{
    class Program
    {
        static void Main(string[] args)
        {
            string address = "testphp.vulnweb.com";
            int port = 80;

            //Burada bir process oluşturdum, nmap uygulamasını çalıştırdım.
            Process process = new Process();
            ProcessStartInfo process_startInfo = new ProcessStartInfo();
            process_startInfo.FileName = "nmap.exe";
            process_startInfo.Arguments = "-p " + port + " --script http-sql-injection " + address + " -oX \"C:/Users/Okan/OneDrive/Masaüstü/output_xml.xml\"";


            process.StartInfo = process_startInfo;
            process.Start();
            process.WaitForExit();


            //process de oluşturduğum xml dosyasını okutuyorum.
            XmlDocument doc = new XmlDocument();
            doc.Load("C:/Users/Okan/OneDrive/Masaüstü/output_xml.xml");
            XmlNodeList list = doc.GetElementsByTagName("script");

            JArray result = new JArray( //Verileri bir json array oluşturup içine kaydettim.
               new JObject(
                        new JProperty("Nmap command", process_startInfo.Arguments)),
                new JObject(
                        new JProperty("Url", list[0].Attributes["output"].Value))); //Script alanından sadece output kısmını çağırıyorum.


            File.WriteAllText(@"C:\Users\Okan\OneDrive\Masaüstü\output_json.json", result.ToString()); //Kullanan kişiler kendi bilgisayarına göre ayarlamalıdır.
            using (StreamWriter file = File.CreateText(@"C:\Users\Okan\OneDrive\Masaüstü\output_json.json"))
            using (JsonTextWriter print = new JsonTextWriter(file))
            {
                result.WriteTo(print);
            }


        }
    }

}
