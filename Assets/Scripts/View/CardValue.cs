using TMPro;
using UnityEngine;

namespace View
{
	public class CardValue : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI valueText;
		[SerializeField] private TextMeshProUGUI counterText;

		private int _changedCount;
		
		public void Set(string newValue)
		{
			_changedCount++;
			valueText.text = newValue;
			counterText.text = _changedCount.ToString();
		}
	}
}