using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;

namespace Utils
{
	public static class Extensions
	{
		public static void ClearFixed<T>(this ObservableList<T> observableList)
		{
			List<T> removals = observableList.ToList();
			for (var i = 0; i < observableList.Count; i++)
				observableList.Remove(removals[i]);
		}
	}
}