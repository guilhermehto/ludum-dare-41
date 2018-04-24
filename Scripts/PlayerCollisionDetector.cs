using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour {

	enum CollisionPosition { Up, Bottom, Left, Right }
	
	[SerializeField] private CollisionPosition _position;


	private PlayerBoundaryCollisions _player;

	void Start() {
		_player = transform.parent.gameObject.GetComponent<PlayerController>().collisions;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.gameObject.CompareTag("Boundary")) {
			return;
		}

		switch (_position) {
			case CollisionPosition.Up:
				_player.topColliding = true;
				break;
			case CollisionPosition.Bottom:
				_player.bottomColliding = true;
				break;
			case CollisionPosition.Left:
				_player.leftColliding = true;
				break;
			case CollisionPosition.Right:
				_player.rightColliding = true;
				break;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (!other.gameObject.CompareTag("Boundary")) {
			return;
		}
		
		switch (_position) {
			case CollisionPosition.Up:
				_player.topColliding = false;
				break;
			case CollisionPosition.Bottom:
				_player.bottomColliding = false;
				break;
			case CollisionPosition.Left:
				_player.leftColliding = false;
				break;
			case CollisionPosition.Right:
				_player.rightColliding = false;
				break;
		}
	}
}
