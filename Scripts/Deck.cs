using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;


public class Deck : MonoBehaviour {

	[SerializeField] private List<GameObject> _normalCards;
	[SerializeField] private List<GameObject> _rareCards;

	private readonly int _rareCardChance = 15;
	private readonly int _initialCardsCount = 5;
	private readonly float _spawnInterval = 3f;
	
	private List<SpawnPosition> _spawnPositions = new List<SpawnPosition>();

	void Start() {
		SpawnFirstCards();
		EventManager.OnCardPlaced += SpawnNewCard;
	}

	void OnDestroy() {
		EventManager.OnCardPlaced -= SpawnNewCard;
	}

	private void SpawnFirstCards() {
		for (int c = 1; c <= _initialCardsCount; c++) {
			var spawnPos = new Vector3(transform.position.x + _spawnInterval * c, transform.position.y, -6f);
			var randomCard = Instantiate(_normalCards[Random.Range(0, _normalCards.Count)], spawnPos, Quaternion.identity);
			var spawnPosition = new SpawnPosition(spawnPos);
			randomCard.GetComponent<CardBehaviour>().spawnPosition = spawnPosition;
			_spawnPositions.Add(spawnPosition);
		}
	}

	private void SpawnNewCard() {
		foreach (var spawnPosition in _spawnPositions) {
			if (spawnPosition.isEmpty) {
				var randomCard = new GameObject();
				if (Random.Range(0, 100) > 100 - _rareCardChance) {
					Debug.Log("Rare card");
					randomCard = Instantiate(_rareCards[Random.Range(0, _rareCards.Count)], spawnPosition.spawnPosition, Quaternion.identity);
				} else {
					randomCard = Instantiate(_normalCards[Random.Range(0, _normalCards.Count)], spawnPosition.spawnPosition, Quaternion.identity);
				}
				randomCard.GetComponent<CardBehaviour>().spawnPosition = spawnPosition;
				//TODO: Verify if there are more than 3 of the same card
				spawnPosition.isEmpty = false;
				break;
			}
		}
	}
}


public class SpawnPosition {
	public Vector3 spawnPosition;
	public bool isEmpty = true;

	public SpawnPosition() { }
	public SpawnPosition(Vector3 position) {
		spawnPosition = position;
		isEmpty = false;
	 }
}