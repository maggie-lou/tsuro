using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace tsuro
{
	// Converts XML to objects 
    public static class XMLDecoder
    {
		public static Tile xmlToTile(XElement tileXml)
        {
            List<Path> paths = new List<Path>();
            List<XElement> tileXMLTree = tileXml.Descendants().ToList();
            bool tileCheck = checkOrderOfTagsFromXML(new List<string> { "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n",
                "connect", "n", "n"}, tileXMLTree);
            if (!tileCheck)
            {
                throw new Exception("Invalid Tile XML Received from Network Player.");
            }
            IEnumerable<XElement> elements =
            from el in tileXml.Elements("connect")
            select el;
            foreach (XElement i in elements)
            {
                int start = (int)i.Elements("n").ElementAt(0);
                int end = (int)i.Elements("n").ElementAt(1);
                paths.Add(new Path(start, end));
            }
            Tile returnTile = new Tile(paths);
            return returnTile;

        }


        public static bool checkOrderOfTagsFromXML(List<string> expected, List<XElement> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            for (int i = 0; i < expected.Count; i++)
            {
                if (expected[i] != actual[i].Name)
                {
                    return false;
                }
            }
            return true;
        }
        public static List<Posn> xmlToPosn(XElement posnXML)
        {
            List<XElement> posnXMLTree = posnXML.Descendants().ToList();
            bool hCheck = checkOrderOfTagsFromXML(new List<string> { "h", "n", "n" }, posnXMLTree);
            bool vCheck = checkOrderOfTagsFromXML(new List<string> { "v", "n", "n" }, posnXMLTree);
            if (!(hCheck || vCheck))
            {
                throw new Exception("Invalid Posn XML Received from Network Player.");
            }

            int row1;
            int row2;
            int col1;
            int col2;
            int tilePos;
            int tilePos2;

            XElement edgeXML = posnXML.Elements("n").ElementAt(0);
            int edge;
            int.TryParse(edgeXML.Value, out edge);

            XElement locOnEdgeXML = posnXML.Elements("n").ElementAt(1);
            int locOnEdge;
            int.TryParse(locOnEdgeXML.Value, out locOnEdge);

            if (hCheck)
            {
                row1 = edge;
                row2 = edge - 1;

                col1 = locOnEdge / 2;
                col2 = col1;

                tilePos = locOnEdge % 2;
                if (tilePos == 0)
                {
                    tilePos2 = 5;
                }
                else
                {
                    tilePos2 = 4;
                }
            }
            else
            {
                col1 = edge - 1;
                col2 = edge;

                row1 = locOnEdge / 2;
                row2 = row1;

                tilePos = locOnEdge % 2 + 2;
                if (tilePos == 2)
                {
                    tilePos2 = 7;
                }
                else
                {
                    tilePos2 = 6;
                }
            }

            Posn p1 = new Posn(row1, col1, tilePos);
            Posn p2 = new Posn(row2, col2, tilePos2);
            List<Posn> posnList = new List<Posn> { p1, p2 };

            return posnList;
        }
    }
}
