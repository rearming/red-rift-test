using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core
{
	[RequireComponent(typeof(Button))]
	public class StrangeButton : MonoBehaviour
	{
		[SerializeField] private int minChange = -2;
		[SerializeField] private int maxChange = 9;

		private int _currentCardIdx;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() =>
			{
				Hand.Instance[_currentCardIdx].ChangeRandomField((int)Random.Range(minChange, (float)maxChange));
				_currentCardIdx++;
			});
		}
	}
}