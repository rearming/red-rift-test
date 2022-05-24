using UnityEngine;
using Utils;

namespace Core
{
	public class Table : Singleton<Table>
	{
		[SerializeField] private BoxCollider activeZone;

		private Plane _plane;
		private Camera _camera;

		protected override void Awake()
		{
			base.Awake();
			
			_plane = new Plane(transform.up, transform.position);
			_camera = Camera.main;
		}

		public bool GetCardTablePosition(out Vector3? position, out Vector3 normal)
		{
			position = null;
			normal = _plane.normal;
			
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			
			if (!_plane.Raycast(ray, out float enter))
				return false;
			
			position = ray.GetPoint(enter);
			if (activeZone.bounds.Contains(position.Value))
				return true;
				
			position = Input.mousePosition;
			return false;
		}
	}
}