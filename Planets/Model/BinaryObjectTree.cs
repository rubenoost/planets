using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            if (_parent != null)
            {
                if (!IsIn(go.BoundingBox, boundigBox))
                {
                    _parent.Add(go);
                    return;
                }
            }

            if (t1 != null && IsIn(go.BoundingBox, t1.boundigBox))
            {
                t1.Add(go);
                return;
            }

            if (t2 != null && IsIn(go.BoundingBox, t2.boundigBox))
            {
                t2.Add(go);
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
            List<GameObject> temp = _objects.ToList();
            for(int i = temp.Count - 1; i >= 0; i--)
            {
                GameObject go1 = temp[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    a(go1, temp[j], ms);
                    colCount++;
                }

                if (t1 != null)
                {
                    colCount += t1.DoCollisions(a, go1, ms);
                }

                if (t2 != null)
                {
                    colCount += t2.DoCollisions(a, go1, ms);
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

        protected int DoCollisions(Action<GameObject, GameObject, double> a, GameObject go, double ms)
        {
            int colCount = 0;
            if (t1 != null)
            {
                if (go.BoundingBox.IntersectsWith(t1.boundigBox))
                {
                    colCount += t1.DoCollisions(a, go, ms);
                }
            }

            if (t2 != null)
            {
                if (go.BoundingBox.IntersectsWith(t2.boundigBox))
                {
                    colCount += t2.DoCollisions(a, go, ms);
                }
            }

            if (go.BoundingBox.IntersectsWith(boundigBox))
            {
                for (int i = 0; i < _objects.Count; i++)
                {
                    a(_objects[i], go, ms);
                    colCount++;
                }
            }
            return colCount;
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
