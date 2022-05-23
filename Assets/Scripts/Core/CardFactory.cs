using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using View;

namespace Core
{
	public class CardFactory : Singleton<CardFactory>
	{
		[SerializeField] private CardView cardPrefab;
		[SerializeField] private Transform parent;
		[SerializeField] private string cardImageURL = "https://picsum.photos/250/200";
		[SerializeField] private int minCardCount = 4;
		[SerializeField] private int maxCardCount = 6;

		public List<Card> CreateAll()
		{
			int cardsNum = Random.Range(minCardCount, maxCardCount + 1);
			
			var cards = new List<Card>(cardsNum);
			for (var i = 0; i < cardsNum; i++)
				cards.Add(Create(i));

			return cards;
		}

		public Card Create(int? idx = null)
		{
			var card = new Card(Instantiate(cardPrefab, parent));
			if (idx != null)
				card.View.gameObject.name += $"_{idx.Value}";
			return card;
		}

		public IEnumerator GetArt(CardView view)
		{
			using UnityWebRequest request = UnityWebRequestTexture.GetTexture(cardImageURL);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError($"Error while loading texture from [{cardImageURL}] -> [{request.error}]");
				yield break;
			}
				
			Texture2D texture = DownloadHandlerTexture.GetContent(request);
			var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), Vector2.one * 0.5f, 100f);
			view.Art.sprite = sprite;
		}
	}
}