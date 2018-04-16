using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using tsuro;

namespace TsuroTests
{
    [TestClass]
    public class BoardTests
    {
        [TestMethod]
        public void PlayerGetsEliminated()
        {
            SPlayer p1 = new SPlayer("blue", null);
            SPlayer p2 = new SPlayer("red", null);

            Board b = new Board();
            b.registerPlayer(p1);
            b.registerPlayer(p2);

            b.eliminatePlayer(p1);
            Assert.IsFalse(b.returnOnBoard().Contains(p1));
            Assert.IsTrue(b.returnEliminated().Contains(p1));
        }
        

    }
}
