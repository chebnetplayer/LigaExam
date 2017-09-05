using System;
using System.Net;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Human> peoples = new List<Human>();
            string uri = @"http://testlodtask20172.azurewebsites.net/task";
            string txt = DownloadWebString(uri);
            txt = txt.Remove(0, 1);
            txt = txt.Remove(txt.Length - 1, 1);
            while (txt != "")
            {
                string a = txt.Substring(0, txt.IndexOf("}")+1);
                peoples.Add(JsonConvert.DeserializeObject<Human>(a));
                try{ txt = txt.Remove(0, txt.IndexOf("}") + 2);}
                catch { txt = ""; };
            }
           foreach(var i in peoples)
                i.age = i.GetAge();            
            var youngboyage = (from i in peoples where i.sex == "male" select i.age).Min();
            var younggirlage= (from i in peoples where i.sex == "female" select i.age).Min();
            var youngboy = from i in peoples where i.age==youngboyage  select i;
            var younggirl = from i in peoples where i.age==younggirlage select i;
            foreach (var i in youngboy) Console.WriteLine("Самый молодой парень: "+i.name + "  " + i.age);
            foreach (var i in younggirl) Console.WriteLine("Самая молодая девушка: " +i.name + "   " + i.age);
            Console.ReadKey();
        }
        public static string DownloadWebString(string uri)
        {
            WebClient webclient = new WebClient();
            return webclient.DownloadString(uri);
        }
    }
   class Human
    {
        public string id;
        public string name;
        public string sex;
        public int age;
        public int GetAge()
        {
            string uri = @"http://testlodtask20172.azurewebsites.net/task/" + id;
            Human humanHelper = JsonConvert.DeserializeObject<Human>(Program.DownloadWebString(uri));
            return humanHelper.age;
        }
    }
}