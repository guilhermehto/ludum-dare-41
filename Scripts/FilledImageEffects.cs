using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledImageEffects : MonoBehaviour {



	private Image _image;
	private readonly float _fillFactor = 0.5f;
	private readonly float _emptyFactor = 1.25f;
	private bool _isFilling;
	private bool _isEmptying;
	private bool _nextIsZero;
	private float _currentMaxValue = 0;

	void Start() {
		_image = GetComponent<Image>();
	}

	void Update() {
		if (!_isFilling && !_isEmptying) {
			return;
		}

		if (_isFilling) {
			Fill();
		} else if (_isEmptying) {
			Empty();
		}
	}

	public void FillTo(float value) {
		if (value == 0) {
			if (_image.fillAmount != _currentMaxValue ) {
				_nextIsZero = true;
			} else {
				_currentMaxValue = 0;	
			}
		} else {
			_currentMaxValue = value;
			_isFilling = true;
			_isEmptying = false;
		}
	}

	private void Empty() {
		_image.fillAmount -= _emptyFactor * Time.deltaTime;
		
		if (_image.fillAmount <= 0) {
			_image.fillAmount = 0;
			_isEmptying = false;
		}
	}

	private void Fill() {
		_image.fillAmount += _fillFactor * Time.deltaTime;
		
		if (_image.fillAmount >= _currentMaxValue) {
			_image.fillAmount = _currentMaxValue;
			_isFilling = false;
			if (_nextIsZero) {
				_nextIsZero = false;
				_isEmptying = true;
			}
		}
	}
 
}
