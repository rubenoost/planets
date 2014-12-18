using System;
using System.Collections.Generic;
using System.IO;

namespace Planets.Model
{
	public class ScoreBoard
	{
		public int Total { get; private set; }

        public List<Score> Scores { get; private set; }

        private static String filepath = @"Data\scores.txt";

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

        public static int GetHighScore()
        {
            int score;

            if(File.Exists(filepath))
            {
                string Stringscore = File.ReadAllText(filepath);
                score = Convert.ToInt16(Stringscore);
            }
            else
            {
                File.Create(filepath);
                score = 0;
            }

            return score;
        }

        public static void WriteScore(int score)
        {
            if(score > GetHighScore())
            {
                File.WriteAllText(filepath, score.ToString());
            }
        }

	}
}
