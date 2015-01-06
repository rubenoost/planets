using System;
using System.Collections.Generic;
using System.IO;

namespace Planets.Model
{
	public class ScoreBoard
	{
        // Total Score
		public int Total { get; private set; }

        // All Scores
        public List<Score> Scores { get; private set; }

        // Scores path
        private static readonly String Filepath = @"Data\scores.txt";

        // Constructor
		public ScoreBoard()
		{
			Scores = new List<Score>();
		}

        // Add a score, like (+50)
		public void AddScore(Score score)
		{
			Scores.Add(score);
			Total += score.Value;
		}

        // Check TimeStamps for the Fading and moving up of the scores
		public void CheckStamps()
		{
			for (int i = Scores.Count - 1; i >= 0; i--) {
				TimeSpan span = DateTime.Now - Scores[i].Stamp;
				if (span.TotalMilliseconds >= 2000)
					Scores.Remove(Scores[i]);
			}
		}
        
        // Return Highscore from file
        public static int GetHighScore()
        {
            int score;

            if(File.Exists(Filepath))
            {
                string stringscore = File.ReadAllText(Filepath);
                score = Convert.ToInt16(stringscore);
            }
            else
            {
                File.Create(Filepath);
                File.WriteAllText(Filepath, "0");
                score = 0;
            }

            return score;
        }

        // Write new score to file if Score is higher than the registered Highscore
        public static void WriteScore(int score)
        {
            if(score > GetHighScore())
            {
                File.WriteAllText(Filepath, score.ToString());
            }
        }

	}
}
