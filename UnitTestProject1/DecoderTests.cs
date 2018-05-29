using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;
using System.Xml.Linq;
using System.Linq;

namespace TsuroTests
{
	[TestClass]
    public class DecoderTests
    {
        public DecoderTests()
        {
        }

		[TestMethod]
        public void XMLToTile()
        {
            XElement t1XML = new XElement("tile",
                                                 new XElement("connect",
                                                             new XElement("n", 0),
                                                              new XElement("n", 1)),
                                                  new XElement("connect",
                                                             new XElement("n", 2),
                                                              new XElement("n", 3)),

                                                  new XElement("connect",
                                                             new XElement("n", 4),
                                                              new XElement("n", 5)),

                                                  new XElement("connect",
                                                             new XElement("n", 6),
                                                               new XElement("n", 7)));
            TestScenerios test = new TestScenerios();
            Tile t1Expected = test.makeTile(0, 1, 2, 3, 4, 5, 6, 7);
            Tile t1Actual = XMLDecoder.xmlToTile(t1XML);

            Assert.IsTrue(t1Expected.isEqual(t1Actual));

        }
    }
}
