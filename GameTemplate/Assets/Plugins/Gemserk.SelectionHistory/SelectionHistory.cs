using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk
{
    [Serializable]
    public class SelectionHistory
    {
        [SerializeField]
        List<Object> _history = new List<Object>(100);

        [SerializeField]
        List<Object> _favorites = new List<Object>(100);

        int currentSelectionIndex;

        Object currentSelection;

        int historySize = 10;

        public List<Object> History
        {
            get { return _history; }
        }

        public List<Object> Favorites
        {
            get { return _favorites; }
        }

        public int HistorySize
        {
            get { return historySize; }
            set { historySize = value; }
        }

        public bool IsSelected(int index)
        {
            return index == currentSelectionIndex;
        }

        public bool IsSelected(Object obj)
        {
            return currentSelection == obj;
        }

        public void Clear()
        {
            _history.Clear();
        }

        public int GetHistoryCount()
        {
            return _history.Count;
        }

        public Object GetSelection()
        {
            return currentSelection;
        }

        public void UpdateSelection(Object selection)
        {
            if (selection == null)
                return;

            var lastSelectedObject = _history.Count > 0 ? _history.Last() : null;

            if (lastSelectedObject != selection && currentSelection != selection)
            {
                _history.Add(selection);
                currentSelectionIndex = _history.Count - 1;
            }

            currentSelection = selection;

            if (_history.Count > historySize)
            {
                _history.RemoveRange(0, _history.Count - historySize);
                //			_history.RemoveAt(0);
            }
        }

        public void Previous()
        {
            if (_history.Count == 0)
                return;

            currentSelectionIndex--;
            if (currentSelectionIndex < 0)
                currentSelectionIndex = 0;
            currentSelection = _history[currentSelectionIndex];
        }

        public void Next()
        {
            if (_history.Count == 0)
                return;

            currentSelectionIndex++;
            if (currentSelectionIndex >= _history.Count)
                currentSelectionIndex = _history.Count - 1;
            currentSelection = _history[currentSelectionIndex];
        }

        public void SetSelection(Object obj)
        {
            currentSelectionIndex = _history.IndexOf(obj);
            currentSelection = obj;
        }

        public void ClearDeleted()
        {
            var deletedCount = _history.Count(e => e == null);

            _history.RemoveAll(e => e == null);
            _favorites.RemoveAll(d => d == null);

            currentSelectionIndex -= deletedCount;

            if (currentSelectionIndex < 0)
                currentSelectionIndex = 0;

            if (currentSelection == null)
                currentSelectionIndex = -1;
        }

        public void RemoveDuplicated()
        {
            var tempList = new List<Object>(_history);

            foreach (var item in tempList)
            {
                var itemFirstIndex = _history.IndexOf(item);
                var itemLastIndex = _history.LastIndexOf(item);

                while (itemFirstIndex != itemLastIndex)
                {
                    _history.RemoveAt(itemFirstIndex);

                    itemFirstIndex = _history.IndexOf(item);
                    itemLastIndex = _history.LastIndexOf(item);
                }
            }

            if (currentSelectionIndex >= _history.Count)
                currentSelectionIndex = _history.Count - 1;
        
        }

        public bool IsFavorite(Object obj)
        {
            return _favorites.Contains(obj);
        }

        public void ToggleFavorite(Object obj)
        {
            if (_favorites.Contains(obj))
                _favorites.Remove(obj);
            else
                _favorites.Add(obj);
        }
        

    }
}