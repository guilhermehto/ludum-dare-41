using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;

public class CameraShake : MonoBehaviour {


	public enum ShakeStrength {
		Light,
		Medium,
		Strong
	}

	private readonly float _lightShakeTime = 0.25f;
	private readonly float _mediumShakeTime = 0.25f;
	private readonly float _strongShakeTime = 0.25f;

	private readonly float _lightShakeForce = 0.15f;
	private readonly float _mediumShakeForce = 0.25f;
	private readonly float _strongShakeForce = 0.5f;


	private bool _isShaking = false;
	private ShakeStrength _currentStrength;
	private Vector3 _originalPosition;
	private float _currentShakeTime;

	void Start() {
		EventManager.OnCameraShake += ShakeCamera;
	}

	void Update() {
		if (!_isShaking) {
			return;
		}

		switch(_currentStrength) {
			case ShakeStrength.Light:
				transform.position = _originalPosition + Random.insideUnitSphere * _lightShakeForce;
				if (_currentShakeTime >= _lightShakeTime) {
					_isShaking = false;
					transform.position = _originalPosition;
				}
				break;
			case ShakeStrength.Medium:
				transform.position = _originalPosition + Random.insideUnitSphere * _mediumShakeForce;
				if (_currentShakeTime >= _mediumShakeTime) {
					_isShaking = false;
					transform.position = _originalPosition;
				}
				break;
			case ShakeStrength.Strong:
				transform.position = _originalPosition + Random.insideUnitSphere * _strongShakeForce;
				if (_currentShakeTime >= _strongShakeTime) {
					_isShaking = false;
					transform.position = _originalPosition;
				}
				break;
		}

		_currentShakeTime += Time.deltaTime;
	}

	void OnDestroy() {
		EventManager.OnCameraShake -= ShakeCamera;
	}
	

	private void ShakeCamera(ShakeStrength force) {
		_currentStrength = force;
		_isShaking = true;
		_originalPosition = transform.position;
		_currentShakeTime = 0;
	}

	


}
