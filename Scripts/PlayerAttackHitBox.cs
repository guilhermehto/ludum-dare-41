using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;

public class PlayerAttackHitBox : MonoBehaviour {

	[SerializeField] private float _damage = 25f;

	void OnTriggerEnter2D(Collider2D other) {
		var eb = other.GetComponent<EnemyBehaviour>();
		if (eb != null) {
			eb.Damage(_damage);
			EventManager.ShakeCamera(CameraShake.ShakeStrength.Light);
		}
		
	}
}
