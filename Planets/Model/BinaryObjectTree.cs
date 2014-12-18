using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Planets.Model.GameObjects;

namespace Planets.Model
{
    public class BinaryObjectTree
    {
        public int Count
        {
            get { return _objects.Count + (t1 == null ? 0 : t1.Count) + (t2 == null ? 0 : t2.Count); }
        }

        public IEnumerable<GameObject> GameObjectList
        {
            get { return _allGameObjects; }
        }

        private BinaryObjectTree t1;
        private BinaryObjectTree t2;
        private int level;
        private BinaryObjectTree _parent;
        private Rectangle boundingBox;
        private List<GameObject> _objects = new List<GameObject>();
        public int ColCount;

        private HashSet<GameObject> _allGameObjects = new HashSet<GameObject>();

        public BinaryObjectTree(BinaryObjectTree parent, Rectangle r, int l, int maxlevel, int splitDirection)
        {
            boundingBox = r;
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
            _allGameObjects.Add(go);
            if (_parent != null)
            {
                if (!IsIn(go.BoundingBox, boundingBox))
                {
                    _parent.Add(go);
                    return;
                }
            }
            if (t1 != null && IsIn(go.BoundingBox, t1.boundingBox))
            {
                t1.Add(go);
                return;
            }
            if (t2 != null && IsIn(go.BoundingBox, t2.boundingBox))
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
            _allGameObjects.Remove(go);
            if (go is Stasis)
                Console.WriteLine("");
            if (t1 != null)
                t1.Remove(go);
            if (t2 != null)
                t2.Remove(go);
            _objects.Remove(go);
        }
        public void Clear()
        {
            _allGameObjects.Clear();
            _objects.Clear();
            if (t1 != null)
                t1.Clear();
            if (t2 != null)
                t2.Clear();
        }
        public void DoCollisions(Action<GameObject, GameObject, double> a, double ms)
        {
            ColCount = 0;
            List<GameObject> temp = _objects.ToList();
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                GameObject go1 = temp[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    a(go1, temp[j], ms);
                    ColCount++;
                }
                if (t1 != null)
                {
                    ColCount += t1.DoCollisions(a, go1, ms);
                }
                if (t2 != null)
                {
                    ColCount += t2.DoCollisions(a, go1, ms);
                }
            }
            if (t1 != null)
            {
                t1.DoCollisions(a, ms);
                ColCount += t1.ColCount;
            }
            if (t2 != null)
            {
                t2.DoCollisions(a, ms);
                ColCount += t2.ColCount;
            }
        }
        protected int DoCollisions(Action<GameObject, GameObject, double> a, GameObject go, double ms)
        {
            int colCount = 0;
            if (t1 != null)
            {
                if (go.BoundingBox.IntersectsWith(t1.boundingBox))
                {
                    colCount += t1.DoCollisions(a, go, ms);
                }
            }
            if (t2 != null)
            {
                if (go.BoundingBox.IntersectsWith(t2.boundingBox))
                {
                    colCount += t2.DoCollisions(a, go, ms);
                }
            }
            if (go.BoundingBox.IntersectsWith(boundingBox))
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
                a(_objects[i]);
            if (t1 != null)
                t1.Iterate(a);
            if (t2 != null)
                t2.Iterate(a);
        }
    }
}