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
            get { return _objects.Count + (_t1 == null ? 0 : _t1.Count) + (_t2 == null ? 0 : _t2.Count); }
        }

        private readonly BinaryObjectTree _t1;
        private readonly BinaryObjectTree _t2;

        private readonly BinaryObjectTree _parent;

        private readonly Rectangle _boundingBox;

        private readonly List<GameObject> _objects = new List<GameObject>();

        public int ColCount;

        public BinaryObjectTree(BinaryObjectTree parent, Rectangle r, int l, int maxlevel, int splitDirection)
        {
            _boundingBox = r;
            var level = l;
            _parent = parent;

            if (level != maxlevel)
            {
                if (splitDirection == 0)
                {
                    _t1 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y, r.Width / 2, r.Height), level + 1, maxlevel, 1);
                    _t2 = new BinaryObjectTree(this, new Rectangle(r.X + r.Width / 2, r.Y, r.Width / 2, r.Height), level + 1, maxlevel, 1);
                }
                else
                {
                    _t1 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y, r.Width, r.Height / 2), level + 1, maxlevel, 0);
                    _t2 = new BinaryObjectTree(this, new Rectangle(r.X, r.Y + r.Height / 2, r.Width, r.Height / 2), level + 1, maxlevel, 0);
                }
            }
        }

        public void Add(GameObject go)
        {
            if (_parent != null)
            {
                if (!IsIn(go.BoundingBox, _boundingBox))
                {
                    _parent.Add(go);
                    return;
                }
            }

            if (_t1 != null && IsIn(go.BoundingBox, _t1._boundingBox))
            {
                _t1.Add(go);
                return;
            }

            if (_t2 != null && IsIn(go.BoundingBox, _t2._boundingBox))
            {
                _t2.Add(go);
                return;
            }


            _objects.Add(go);
            go.Moved += Update;
            go.Resized += Update;
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
            if (_t1 != null)
                _t1.Remove(go);
            if (_t2 != null)
                _t2.Remove(go);
            _objects.Remove(go);
        }

        public void Clear()
        {
            _objects.Clear();
            if (_t1 != null)
                _t1.Clear();
            if (_t2 != null)
                _t2.Clear();
        }

        public void DoCollisions(Action<GameObject, GameObject, double> a, double ms)
        {
            ColCount = 0;
            List<GameObject> temp = _objects.ToList();
            for(int i = temp.Count - 1; i >= 0; i--)
            {
                GameObject go1 = temp[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    a(go1, temp[j], ms);
                    ColCount++;
                }

                if (_t1 != null)
                {
                    ColCount += _t1.DoCollisions(a, go1, ms);
                }

                if (_t2 != null)
                {
                    ColCount += _t2.DoCollisions(a, go1, ms);
                }
            }

            if (_t1 != null)
            {
                _t1.DoCollisions(a, ms);
                ColCount += _t1.ColCount;
            }

            if (_t2 != null)
            {
                _t2.DoCollisions(a, ms);
                ColCount += _t2.ColCount;
            }
        }

        protected int DoCollisions(Action<GameObject, GameObject, double> a, GameObject go, double ms)
        {
            int colCount = 0;
            if (_t1 != null)
            {
                if (go.BoundingBox.IntersectsWith(_t1._boundingBox))
                {
                    colCount += _t1.DoCollisions(a, go, ms);
                }
            }

            if (_t2 != null)
            {
                if (go.BoundingBox.IntersectsWith(_t2._boundingBox))
                {
                    colCount += _t2.DoCollisions(a, go, ms);
                }
            }

            if (go.BoundingBox.IntersectsWith(_boundingBox))
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
            if(_t1 != null)
                _t1.Iterate(a);
            if(_t2 != null)
                _t2.Iterate(a);
        }
    }
}
