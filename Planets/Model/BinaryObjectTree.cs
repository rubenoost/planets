using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Planets.Model
{
    public class BinaryObjectTree
    {
        public int Count
        {
            get { return _objects.Count + (t1 == null ? 0 : t1.Count) + (t2 == null ? 0 : t2.Count); }
        }

        private BinaryObjectTree t1;
        private BinaryObjectTree t2;

        private Rectangle r1;
        private Rectangle r2;

        private int level;
        private BinaryObjectTree _parent;

        private Rectangle boundigBox;

        private List<GameObject> _objects = new List<GameObject>();

        public int colCount;

        public BinaryObjectTree(BinaryObjectTree parent, Rectangle r, int l, int maxlevel, int splitDirection)
        {
            boundigBox = r;
            level = l;
            _parent = parent;

            if (level != maxlevel)
            {
                if (splitDirection == 0)
                {
                    t1 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y, r.Width / 2, r.Height), level + 1, maxlevel, 1);
                    t2 = new BinaryObjectTree(this, new Rectangle(r.X + r.Width / 2, r.Y, r.Width / 2, r.Height), level + 1, maxlevel, 1);
                }
                else
                {
                    t1 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y, r.Width, r.Height / 2), level + 1, maxlevel, 0);
                    t2 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y + r.Height / 2, r.Width, r.Height / 2), level + 1, maxlevel, 0);
                }
            }
        }

        public void Add(GameObject go)
        {
            var r = new Rectangle((int)(go.Location.X - go.Radius), (int)(go.Location.Y - go.Radius), (int)go.Radius * 2, (int)go.Radius * 2);
            Add(go, r);
        }

        private void Add(GameObject go, Rectangle r)
        {
            if (_parent != null)
            {
                if (!IsIn(r, boundigBox))
                {
                    _parent.Add(go, r);
                    return;
                }
            }

            if (t1 != null && IsIn(r, t1.boundigBox))
            {
                t1.Add(go, r);
                return;
            }

            if (t2 != null && IsIn(r, t2.boundigBox))
            {
                t2.Add(go, r);
                return;
            }


            _objects.Add(go);
            go.Moved += Update;
        }

        private static bool IsIn(Rectangle s, Rectangle r)
        {
            return s.X >= r.X && s.Y >= r.Y && (s.X + s.Width) <= (r.X + r.Width) && (s.Y + s.Height) <= (r.Y + r.Height);
        }

        public void Update(GameObject go)
        {
            go.Moved -= Update;
            _objects.Remove(go);
            Add(go);
        }

        public void Remove(GameObject go)
        {
            if (t1 != null)
                t1.Remove(go);
            if (t2 != null)
                t2.Remove(go);
            _objects.Remove(go);
        }

        public void Clear()
        {
            _objects.Clear();
            if (t1 != null)
                t1.Clear();
            if (t2 != null)
                t2.Clear();
        }

        public void DoCollisions(Action<GameObject, GameObject, double> a, double ms)
        {
            colCount = 0;
            for(int i = _objects.Count - 1; i >= 0; i = Math.Min(i - 1, _objects.Count - 1))
            {
                GameObject go1 = _objects[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    colCount++;
                    a(go1, _objects[j], ms);
                }

                if (t1 != null)
                {
                    colCount += t1.Count;
                    t1.Iterate(p => a(go1, p, ms));
                }

                if (t2 != null)
                {
                    colCount += t2.Count;
                    t2.Iterate(p => a(go1, p, ms));
                }
            }

            if (t1 != null)
            {
                t1.DoCollisions(a, ms);
                colCount += t1.colCount;
            }

            if (t2 != null)
            {
                t2.DoCollisions(a, ms);
                colCount += t2.colCount;
            }
        }

        public void Iterate(Action<GameObject> a)
        {
            for (int i = _objects.Count - 1; i >= 0; i--)
            {
                a(_objects[i]);
            }
            if(t1 != null)
                t1.Iterate(a);
            if(t2 != null)
                t2.Iterate(a);
        }
    }
}
