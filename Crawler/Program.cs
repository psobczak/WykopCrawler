﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Crawler
{
    class Program
    {
        private static string databaseName = "Wykop";
        private static string firstPage = "https://www.wykop.pl/";
        private static string createMainTable = "CREATE TABLE IF NOT EXISTS Main (" +
            "id TEXT," +
            "title TEXT," +
            "diggs NUMBER," +
            "username TEXT," +
            "source TEXT," +
            "comments NUMBER," +
            "description TEXT," +
            "date DATE);";
        public static string containerXpath = ".//ul[@id='itemsStream']/li[@class='link iC ']";

        static void Main(string[] args)
        {
            // Define database controls
            DatabaseControls databaseControls = new DatabaseControls();
            databaseControls.CreateDatabase(databaseName);
            databaseControls.ManageConnection("open");
            databaseControls.CreateTable(createMainTable);

            // Define nodes, their xpaths
            NodeManager nodeManager = new NodeManager();
            HtmlDocument htmlDocument = nodeManager.LoadPage(firstPage);
            HtmlNodeCollection nodeCollection = nodeManager.GetNodeCollection(htmlDocument, containerXpath);

            int index = 1;
            foreach (HtmlNode node in nodeCollection)
            { 
                string id = Guid.NewGuid().ToString();
                string title = nodeManager.GetGenericValue(node, ".//div[@class='lcontrast m-reset-margin']/h2/a").Trim();
                int diggs = nodeManager.GetIntValue(node, ".//div[@class='diggbox ']//a//span");
                string username = nodeManager.GetGenericValue(node, ".//div[@class='fix-tagline']/a");


                Console.WriteLine($"{index}. {username}");
                index++;
            }

            databaseControls.ManageConnection("close");
        }
    }
}
