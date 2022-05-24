using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Networking;
using Utils;
using View;

namespace Core
{
	public class CardFactory : Singleton<CardFactory>
	{
		[SerializeField] private GameObject tableViewPrefab;
		[SerializeField] private GameObject uiViewPrefab;
		[SerializeField] private Transform worldViewParent;
		[SerializeField] private RectTransform uiViewParent;
		
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
			var uiView = Instantiate(uiViewPrefab, uiViewParent).GetComponentInChildren<ICardView>();
			var tableView = Instantiate(tableViewPrefab, worldViewParent).GetComponentInChildren<ICardView>();
			
			var card = new Card(uiView, tableView, uiView.Controller);

			StartCoroutine(LoadSprite(new List<ICardView> { uiView, tableView }));

			if (idx == null)
				return card;
			
			tableView.GameObject.name += $"_{idx.Value}";
			uiView.GameObject.name += $"_{idx.Value}";
			return card;
		}

		private IEnumerator LoadSprite(IEnumerable<ICardView> views)	
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
			views.ForEach(v => v.Art.sprite = sprite);
		}
	}
}