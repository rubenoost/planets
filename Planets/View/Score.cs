using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Planets.View
{
    public class Score : GameView
    {
        public void ModifyScore(int Score)
            {
                int CurrentScore = Convert.ToInt32(ScoreLabel.Text);
                ScoreLabel.Text = Convert.ToString(CurrentScore + Score);
            }

    }
}
