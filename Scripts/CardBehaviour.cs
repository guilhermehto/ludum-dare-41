using UnityEngine;
using CardNSlash;

public class CardBehaviour : MonoBehaviour {
    


    [HideInInspector] public SpawnPosition spawnPosition;

    private Vector3 _movingOffset;
    private bool _isMoving;
    protected Animator _animator;
    protected ICardAction _action;


    void Update() {
        if (_isMoving) {
            if (Input.GetMouseButtonUp(0)) {
                _animator = GetComponent<Animator>();
                _isMoving = false;
                spawnPosition.isEmpty = true;
                _animator.Play("Drop");
                EventManager.QueueAction(_action);
            }
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _movingOffset;
        }
    }

    private void Dropped() {
        EventManager.CardPlaced();
        Destroy(gameObject);
    }

    private void OnMouseOver() {
        if (Input.GetMouseButton(0) && !_isMoving) {
            _isMoving = true;
            _movingOffset = transform.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }


}