using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

		public static Vector3 WithX(this in Vector3 color, float x) => new Vector3(x, color.y, color.z);
		public static Vector3 WithY(this in Vector3 color, float y) => new Vector3(color.x, y, color.z);
		public static Vector3 WithZ(this in Vector3 color, float z) => new Vector3(color.x, color.y, z);
		
		public static Color WithA(this in Color color, float a) => new Color(color.r, color.g, color.b, a);
	}
}