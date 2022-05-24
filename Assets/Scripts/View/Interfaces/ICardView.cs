using Core;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public interface ICardView
	{
		Image Art { get; }
		GameObject GameObject { get; }
		GameObject Root { get; }
		CardController Controller { get; }
		
		void Init(Card model);
	}
}