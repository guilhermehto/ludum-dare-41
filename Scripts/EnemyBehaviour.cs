using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;

public class EnemyBehaviour : MonoBehaviour {


	[Header("Audio")]
	[SerializeField] private AudioClip _deadAudio;
	[SerializeField] private AudioClip _hitAudio;

	[Header("Control")]
	[SerializeField] private float _damage = 5f;
	[SerializeField] private float _knockBackForce = 10f;
	[SerializeField] private float _attackInterval = 1.5f;
	[SerializeField] private float _maxHealth = 50f;
	[SerializeField] private Sprite _deadSprite;

	private readonly float _step = 0.25f;
	private readonly float _minDistanceFromPlayer = 1f;
	private readonly float _speed = 1f;

	

	private List<ShortestPathModel> _openList = new List<ShortestPathModel>();
	private List<ShortestPathModel> _closedList = new List<ShortestPathModel>();


	private Transform _player;
	private Vector2 _lastPlayerTransform;
	private Rigidbody2D _rb;
	private bool _canMove;
	private bool _canAttack = true;
	private float _currentHealth;
	private AudioSource _audio;

	void Start() {
		_audio = GetComponent<AudioSource>();
		_currentHealth = _maxHealth;
		_player = FindObjectOfType<PlayerController>().gameObject.transform;
		_rb = GetComponent<Rigidbody2D>();
		EventManager.OnGameEnded += GameEnded;
	}

	void OnDestroy() {
		EventManager.OnGameEnded -= GameEnded;
	}
	

	void Update() {
		FindShortestPath();
		Attack();
		if(_canMove && Vector2.Distance(transform.position, _closedList[0].position) <= 0.1f) {
			_closedList.RemoveAt(0);
			if (_closedList.Count == 0) {
				_canMove = false;
				_rb.velocity = Vector2.zero;
			}
		}
	}

	void FixedUpdate() {
		if (_canMove) {
			Move();
		}
	}

	private void Move() {
		_rb.velocity = (_closedList[0].position - (Vector2) transform.position ).normalized * _speed;
	}

	private void Attack() {
		if (_canAttack && Vector2.Distance(transform.position, _player.position) <= _minDistanceFromPlayer + _minDistanceFromPlayer / 3) {
			_player.gameObject.GetComponent<PlayerController>().Damage(_damage);
			_canAttack = false;
			StartCoroutine(ResetCanAttack());
		}
	}

	IEnumerator ResetCanAttack() {
		yield return new WaitForSeconds(_attackInterval);
		_canAttack = true;
	}

	private void GameEnded() {
		_rb.velocity = Vector2.zero;
		Destroy(this);
	}

	private void FindShortestPath() {
		if (_lastPlayerTransform == (Vector2) _player.position) {
			return;
		}
		//Reset lists
		_closedList = new List<ShortestPathModel>();
		_openList = new List<ShortestPathModel>();
		//Add initial position to closed list
		_closedList.Add(new ShortestPathModel {
			gCost = _step,
			hCost = Vector2.Distance(transform.position, _player.position),
			fCost = _step + Vector2.Distance(transform.position, _player.position),
			position = transform.position
		});

		//		Debug.Log("[G Cost]: " + _closedList[0].gCost + " - [H Cost]:" + _closedList[0].hCost + " - [F Cost]:" + _closedList[0].fCost + " - [Position]:" + _closedList[0].position);

		//Add 4 adjacents (UP DOWN LEFT RIGHT)
		//Up
		var newPosition = new Vector2(transform.position.x, transform.position.y + _step);
		var newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
		var lowestFValueIndex = _openList.Count - 1;
		if (!_closedList.Contains(newShortestPathModel)) {
			_openList.Add(newShortestPathModel);
			lowestFValueIndex = _openList.Count - 1;
		}

		//Down
		newPosition = new Vector2(transform.position.x, transform.position.y - _step);
		newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
		if (!_closedList.Contains(newShortestPathModel)) {
			_openList.Add(newShortestPathModel);
			lowestFValueIndex = _openList[_openList.Count - 1].fCost < _openList[lowestFValueIndex].fCost 
				? _openList.Count - 1 
				: lowestFValueIndex;
		}
		
		//Left
		newPosition = new Vector2(transform.position.x - _step, transform.position.y);
		newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
		if (!_closedList.Contains(newShortestPathModel)) {
			_openList.Add(newShortestPathModel);
			lowestFValueIndex = _openList[_openList.Count - 1].fCost < _openList[lowestFValueIndex].fCost 
				? _openList.Count - 1 
				: lowestFValueIndex;
		}
		
		//Right
		newPosition = new Vector2(transform.position.x + _step, transform.position.y);
		newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
		if (!_closedList.Contains(newShortestPathModel)) {
			_openList.Add(newShortestPathModel);
			lowestFValueIndex = _openList[_openList.Count - 1].fCost < _openList[lowestFValueIndex].fCost 
				? _openList.Count - 1 
				: lowestFValueIndex;
		}
		
		_closedList.Add(_openList[lowestFValueIndex]);
		_openList.Remove(_openList[lowestFValueIndex]);
		var c = 0;
		
		
		while (Vector2.Distance(_closedList[_closedList.Count - 1].position, _player.position) > _minDistanceFromPlayer) {
			c++;
			if (c > 100) {
				break;
			}
			//UP
			newPosition = new Vector2(_closedList[_closedList.Count - 1].position.x, 
				_closedList[_closedList.Count - 1].position.y + _step);
			newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
			lowestFValueIndex = _closedList.Count - 1;
			if (!_closedList.Contains(newShortestPathModel)) {
				_openList.Add(newShortestPathModel);
				lowestFValueIndex = _openList.Count - 1;
			}

			//Down
			newPosition = new Vector2(_closedList[_closedList.Count - 1].position.x, 
				_closedList[_closedList.Count - 1].position.y - _step);
			newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
			if (!_closedList.Contains(newShortestPathModel)) {
				_openList.Add(newShortestPathModel);
				lowestFValueIndex = _openList[_openList.Count - 1].hCost< _openList[lowestFValueIndex].hCost 
					? _openList.Count - 1 
					: lowestFValueIndex;
			}
			
			//Left
			newPosition = new Vector2(_closedList[_closedList.Count - 1].position.x - _step, 
				_closedList[_closedList.Count - 1].position.y);
			newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
			if (!_closedList.Contains(newShortestPathModel)) {
				_openList.Add(newShortestPathModel);
				lowestFValueIndex = _openList[_openList.Count - 1].hCost < _openList[lowestFValueIndex].hCost 
					? _openList.Count - 1 
					: lowestFValueIndex;
			}
			
			//Right
			newPosition = new Vector2(_closedList[_closedList.Count - 1].position.x + _step, 
				_closedList[_closedList.Count - 1].position.y);
			newShortestPathModel = new ShortestPathModel(newPosition, _closedList[0].position, _player.position);
			if (!_closedList.Contains(newShortestPathModel)) {
				_openList.Add(newShortestPathModel);
				lowestFValueIndex = _openList[_openList.Count - 1].hCost < _openList[lowestFValueIndex].hCost 
					? _openList.Count - 1 
					: lowestFValueIndex;
			}

			_closedList.Add(_openList[lowestFValueIndex]);
			_openList.Remove(_openList[lowestFValueIndex]);
		}

		//		Debug.Log("Found: " + _closedList.Count + " Position: " + _closedList[_closedList.Count - 1].position);
		_lastPlayerTransform = _player.position;
		_canMove = true;
	}

	private void PlayClip(AudioClip clip) {
		_audio.clip = clip;
		_audio.Play();
	}

	public void Damage(float value) {
		_currentHealth -= value;
		if(_currentHealth <= 0) {
			_currentHealth = 0;
			transform.localScale = new Vector3(transform.localScale.x / 2, transform.localScale.x / 2, transform.localScale.x / 2);
			_rb.velocity = Vector2.zero;
			EventManager.EnemyKilled();
			Destroy(GetComponent<Animator>());
			GetComponent<SpriteRenderer>().sprite = _deadSprite;
			PlayClip(_deadAudio);
			Destroy(this);
		} else {
			PlayClip(_hitAudio);
		}
	}
}

public class ShortestPathModel {
	public float gCost;
	public float hCost;
	public float fCost;
	public Vector2 position;

	public ShortestPathModel() { }
	public ShortestPathModel(Vector2 newPosition, Vector2 initialPosition, Vector2 destination) {
		gCost = Vector2.Distance(initialPosition, newPosition);
		hCost = Vector2.Distance(newPosition, destination);
		fCost = gCost + hCost;
		position = newPosition;
//		Debug.Log("[G Cost]: " + gCost + " - [H Cost]:" + hCost + " - [F Cost]:" + fCost + " - [Position]:" + position);
	}
}
