﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets.Model
{
    abstract class GameObject
    {

        protected Vector location;

        protected Vector DV;

        protected double mass;

        public GameObject(double x, double y, double[] DV, double mass)
        {

        }

        public abstract void Draw(Graphics g);

    }
}
