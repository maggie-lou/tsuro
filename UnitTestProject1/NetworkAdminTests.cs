﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;
using System.Xml.Linq;
using System.Linq;

namespace TsuroTests
{
	[TestClass]
	public class NetworkAdminTests
	{
		public NetworkAdminTests()
		{
		}
      

		[TestMethod]
		public void InitializeHandler()
		{
			RandomPlayer iplayer = new RandomPlayer();
			NetworkAdmin competitor = new NetworkAdmin(iplayer);
			XElement initXML = new XElement("initialize",
											new XElement("color", "blue"),
											new XElement("list",
														 new XElement("color", "green"),
														 new XElement("color", "hotpink")));

			string response = competitor.initializeHandler(initXML);
			string expectedResponse = (new XElement("void", "")).ToString();

			Assert.AreEqual("blue", iplayer.getColor());
			Assert.AreEqual(2, iplayer.getPlayerOrder().Count);
			Assert.AreEqual("green",iplayer.getPlayerOrder()[0]);
			Assert.AreEqual("hotpink", iplayer.getPlayerOrder()[1]);
			Assert.AreEqual(expectedResponse, response);
            
        }
	}
}
