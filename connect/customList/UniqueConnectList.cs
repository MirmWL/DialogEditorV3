using System;
using System.Collections.Generic;

using ConnectLib;

namespace UniqueConnectListLib
{
	public class UniqueConnectList : IList<Connect>
	{
		private readonly IList<Connect> _list;

		public UniqueConnectList(IList<Connect> list)
		{
			_list = list;
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public bool IsReadOnly 
		{
			get { return _list.IsReadOnly; }
		}

		public Connect this[int index]
		{
			get { return _list[index]; }
			set { _list[index] = value; }
		}

		public void Add(Connect item)
		{
			foreach(var connect in _list)
			{
				if(item.GetOrigin() == connect.GetOrigin() && item.GetDestination() == connect.GetDestination()) return;
			}
			_list.Add(item);
		}

		public void Clear() { _list.Clear(); }	
		public bool Contains(Connect item) => _list.Contains(item); 
		public void CopyTo(Connect[] array, int arrayIndex) { _list.CopyTo(array, arrayIndex); }
		public IEnumerator<Connect> GetEnumerator() => _list.GetEnumerator();

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int IndexOf(Connect item) => _list.IndexOf(item);
		public void Insert(int index, Connect item) { _list.Insert(index, item); }
		public bool Remove(Connect item) => _list.Remove(item); 
		public void RemoveAt(int index) { _list.RemoveAt(index); }

	}
}
