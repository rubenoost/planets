using System;
using System.Collections.Generic;
using System.Xml;

namespace Planets.Model
{
	public class ScoreBoard
	{
		public int Total { get; private set; }

		public List<Score> Scores { get; private set; }

		public ScoreBoard()
		{
			Scores = new List<Score>();
		}

		public void AddScore(Score score)
		{
			Scores.Add(score);
			Total += score.Value;
		}

		public void CheckStamps()
		{
			for (int i = Scores.Count - 1; i >= 0; i--) {
				TimeSpan span = DateTime.Now - Scores[i].Stamp;
				if (span.TotalMilliseconds >= 2000)
					Scores.Remove(Scores[i]);
			}
		}

		public static int[] getHighScore()
		{
            String filepath = "scores.xml";

			XmlDocument xd = new XmlDocument();
			xd.Load(filepath);

			XmlNodeList nodelist = xd.SelectNodes("/scores");

			List<int> scores = new List<int>();
			for (int i = 0; i < nodelist.Count; i++) {
				scores.Add(Convert.ToInt16(nodelist[i].SelectSingleNode("score").ToString()));
			}

			return scores.ToArray();
		}
	}
}
