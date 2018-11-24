using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using XmlProject.Models;

namespace XmlProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home



        public ActionResult Index()
        {



            string path = (@"C:\ParkNet\deneme.xml");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);        
            XmlNode rootNode = xmlDoc.SelectSingleNode("words");
            XmlNodeList childNode = rootNode.SelectNodes("word");
            
            List<Words> w = new List<Words>();


            foreach (XmlNode node in childNode)
            {
                w.Add(new Words
                {
                    Count = Convert.ToInt16(node.Attributes.GetNamedItem("Count").Value),
                    text = node.Attributes.GetNamedItem("text").Value
                });




            }

            var model =(w.OrderByDescending(a => a.Count).Take(10));
            return View(model);


      























        }
    }
}