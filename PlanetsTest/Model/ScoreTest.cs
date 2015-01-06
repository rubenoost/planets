using Microsoft.VisualStudio.TestTools.UnitTesting;
using Planets.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetsTest.Model
{
	[TestClass]
	class ScoreTest
	{
		[TestMethod]
		public void testWriteScoreShouldWriteScoreIfNoFileExists()
		{
			ScoreBoard.WriteScore(10);

			Assert.IsTrue(File.Exists(ScoreBoard.Filepath));
		}

		[TestMethod]
		public void testReadMethodReadsScoreFromFileSystem()
		{
			ScoreBoard.WriteScore(10);
			Assert.AreEqual(10, ScoreBoard.GetHighScore());
		}
	}
}
