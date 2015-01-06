using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Planets.Model.GameObjects;

namespace Planets.Model
{
    public class BinaryObjectTree
    {
        // An empty list that is returned when no objects of a given type are found
        private static readonly List<GameObject> _emptyList = new List<GameObject>();
        
        // The amount of objects in this BOT
        public int Count
        {
            get { return _objects.Count + (_t1 == null ? 0 : _t1.Count) + (_t2 == null ? 0 : _t2.Count); }
        }

        public IEnumerable<GameObject> All
        {
            get { return _allGameObjects; }
        }

        private readonly BinaryObjectTree _t1;
        private readonly BinaryObjectTree _t2;
        private readonly BinaryObjectTree _parent;
        private readonly Rectangle _boundingBox;
        private readonly List<GameObject> _objects = new List<GameObject>();
        private int _colCount;

        private readonly HashSet<GameObject> _allGameObjects;
        private Dictionary<Type, List<GameObject>> _typeDictionary;

        private BinaryObjectTree(BinaryObjectTree parent, Rectangle r, int level, int maxlevel, int splitDirection)
        {
            // Save variables, such as boundingbox etc
            _boundingBox = r;
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
        
        public BinaryObjectTree(Rectangle r) 
            : this(null, r, 0, 8, 0)
        {
            // Create Type dictionary
            _typeDictionary = new Dictionary<Type, List<GameObject>>();
            
            // Create list of all GameObjects
            _allGameObjects = new HashSet<GameObject>();
        }
        
        public void Add(GameObject go)
        {
            // Add GameObject to linear list of all gameobjects
            if(_allGameObjects != null)
                _allGameObjects.Add(go);
            
            // Add GameObject to type-specific storage
            if(_typeDictionary != null)
            {
                List<GameObject> result;
                _typeDictionary.TryGetValue(go.GetType(), out result);
                if(result == null) {
                    result = new List<GameObject>();
                    _typeDictionary.Add(go.GetType(), result);
                }
                result.Add(go);
            }
            
            // If object is not in this boudingbox, Add to parent
            if (_parent != null)
            {
                if (!IsIn(go.BoundingBox, _boundingBox))
                {
                    _parent.Add(go);
                    return;
                }
            }
            
            // if object is in boundingbox of T1, add to T1
            if (_t1 != null && IsIn(go.BoundingBox, _t1._boundingBox))
            {
                _t1.Add(go);
                return;
            }
            
            // if object is in boundingbox of T2, add to T2
            if (_t2 != null && IsIn(go.BoundingBox, _t2._boundingBox))
            {
                _t2.Add(go);
                return;
            }
            
            // Else, add object to this list
            _objects.Add(go);
            
            // Register update listener
            go.Moved += Update;
        }
        
        private void Update(GameObject go)
        {
            go.Moved -= Update;
            _objects.Remove(go);
            Add(go);
        }
        public void Remove(GameObject go)
        {
            // Remove from list of all GameObjects
            if(_allGameObjects != null)
                _allGameObjects.Remove(go);
                
            // Remove from T1 if it exists
            if (_t1 != null)
                _t1.Remove(go);
                
            // Remove from T2 if it exists
            if (_t2 != null)
                _t2.Remove(go);
            
            // Remove from current list of GameObjects
            _objects.Remove(go);
        }
        public void Clear()
        {
            // Clear all GameObjects if it exists
            if(_allGameObjects != null)
                _allGameObjects.Clear();
            
            // Clear type dictionary if it exists
            if(_typeDictionary != null)
                _typeDictionary.Clear();
            
            // Clear current objects
            _objects.Clear();
            
            // Clear T1 if it exists
            if (_t1 != null)
                _t1.Clear();
                
            // Clear T2 if it exists
            if (_t2 != null)
                _t2.Clear();
        }
        
        public void DoCollisions(Action<GameObject, GameObject, double> a, double ms)
        {
            _colCount = 0;
            List<GameObject> temp = _objects.ToList();
            for (int i = temp.Count - 1; i >= 0; i--)
            {
                GameObject go1 = temp[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    a(go1, temp[j], ms);
                    _colCount++;
                }
                if (_t1 != null)
                {
                    _colCount += _t1.DoCollisions(a, go1, ms);
                }
                if (_t2 != null)
                {
                    _colCount += _t2.DoCollisions(a, go1, ms);
                }
            }
            if (_t1 != null)
            {
                _t1.DoCollisions(a, ms);
                _colCount += _t1._colCount;
            }
            if (_t2 != null)
            {
                _t2.DoCollisions(a, ms);
                _colCount += _t2._colCount;
            }
        }

        private int DoCollisions(Action<GameObject, GameObject, double> a, GameObject go, double ms)
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
                for (var i = 0; i < _objects.Count; i++)
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
            if (_t1 != null)
                _t1.Iterate(a);
            if (_t2 != null)
                _t2.Iterate(a);
        }
        
        public IEnumerable<GameObject> this[Type t] {
            get {
                List<GameObject> result;
                _typeDictionary.TryGetValue(t, out result);
                return result ?? _emptyList;
            }
        }
        
        // Helper method for checking whether a rectangle is entirely inside another one
        private static bool IsIn(Rectangle s, Rectangle r)
        {
            return s.X >= r.X && s.Y >= r.Y && (s.X + s.Width) <= (r.X + r.Width) && (s.Y + s.Height) <= (r.Y + r.Height);
        }
    }
}
