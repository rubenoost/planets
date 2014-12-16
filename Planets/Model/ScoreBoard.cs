using System;
using System.Collections.Generic;

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
            for (int i = Scores.Count - 1; i >= 0; i--)
            {
                TimeSpan span = DateTime.Now - Scores[i].Stamp;
                if (span.TotalMilliseconds >= 2000)
                    Scores.Remove(Scores[i]);
            }
        }
    }
}
