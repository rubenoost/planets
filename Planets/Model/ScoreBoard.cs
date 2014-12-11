using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    public class ScoreBoard
    {

        public List<Score> Scores { get; private set; }

        public ScoreBoard()
        {
            this.Scores = new List<Score>();
        }

        public void AddScore(Score score)
        {
            this.Scores.Add(score);
        }

        public void CheckStamps()
        {
            for(int i = Scores.Count - 1; i >= 0; i--)
            {
                TimeSpan span = DateTime.Now - Scores[i].Stamp;
                if (span.TotalMilliseconds >= 2000)
                    Scores.Remove(Scores[i]);
            }
        }

    }
}
