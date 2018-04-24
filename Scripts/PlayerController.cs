using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;
public class PlayerController : MonoBehaviour {
	

	[Header("UI")]
	[SerializeField] private FilledImageEffects _healthBar;

	[Header("Audio")]
	[SerializeField] private AudioClip _healClip;

	[Header("Combat")]
	[SerializeField] private float _maxHealth = 100;
	public List<GameObject> meleeTriggers;

	[HideInInspector] public PlayerBoundaryCollisions collisions = new PlayerBoundaryCollisions();
	[HideInInspector] public Vector2 front = new Vector2(0, 1);

	private Queue<ICardAction> _actions = new Queue<ICardAction>();


	private Rigidbody2D _rb;
	private ICardAction _currentAcion = null;
	private float _currentHealth;
	private bool _dead = false;
	private AudioSource _audio;

	void Start() {
		_audio = GetComponent<AudioSource>();
		_rb = GetComponent<Rigidbody2D>();
		_currentHealth = _maxHealth;
		UpdateHealthBar();
		EventManager.OnActionQueued += AddAction;
	}

	void OnDestroy() {
		EventManager.OnActionQueued -= AddAction;	
	}

	void Update() {
		if(_dead) {
			return;
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			_actions.Enqueue(new CardMoveAction(CardMoveAction.MovementDirection.Up));
		} else if (Input.GetKeyDown(KeyCode.S)) {
			_actions.Enqueue(new CardMoveAction(CardMoveAction.MovementDirection.Down));
		} else if (Input.GetKeyDown(KeyCode.A)) {
			_actions.Enqueue(new CardMoveAction(CardMoveAction.MovementDirection.Left));
		} else if (Input.GetKeyDown(KeyCode.D)) {
			_actions.Enqueue(new CardMoveAction(CardMoveAction.MovementDirection.Right));
		}

		if (_currentAcion == null) {
			if (_actions.Count > 0) {
				_currentAcion = _actions.Dequeue();
			} else {
				return;
			}
		}

		if (_currentAcion.DoAction(gameObject)) {
			_currentAcion = null;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("Boundary")) {
			if (other.transform.position.x > transform.position.x) {
				collisions.rightColliding = true;
			}
			
		}
	}

	private void UpdateHealthBar() {
		_healthBar.FillTo(_currentHealth / _maxHealth);
	}

	public void AddAction(ICardAction action) {
		if(_dead) {
			return;
		}
		_actions.Enqueue(action);
	}

	public void Damage(float value) {
		_currentHealth = _currentHealth - value <= 0 ? 0 : _currentHealth - value;
		if (_currentHealth == 0) {
			Debug.Log("Died");
			EventManager.GameEnded();
			_dead = true;
		}
		UpdateHealthBar();
	}

	public void Heal(float value) {
		_currentHealth = _currentHealth + value >= _maxHealth ? _maxHealth : _currentHealth + value;		
		UpdateHealthBar();
		PlayClip(_healClip);
	}

	public void ActivateMeleeTrigger(CardAttackAction.AttackType type, bool active) {
		switch (type) {
			case CardAttackAction.AttackType.LightAttack:
				meleeTriggers[0].SetActive(active);
				break;
			case CardAttackAction.AttackType.HeavyAttack:
				meleeTriggers[1].SetActive(active);
				break;
		}
	}

	private void PlayClip(AudioClip clip) {
		_audio.clip = clip;
		_audio.Play();
	}
}

public class PlayerBoundaryCollisions {
	public bool leftColliding;
	public bool rightColliding;
	public bool topColliding;
	public bool bottomColliding;
}

