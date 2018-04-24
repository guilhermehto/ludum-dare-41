using UnityEngine;

namespace CardNSlash {

    public class CardMoveAction : ICardAction {
        
        public enum MovementDirection {
            Up,
            Down,
            Left,
            Right
        }

        private readonly float _moveSpeed = 2f;
        private readonly float _moveDistance = 3f;
        
        private MovementDirection _direction;

        private Rigidbody2D _playerRb = null;
        private PlayerController _playerController = null;
        private Vector2 _playerInitialPosition;

        public CardMoveAction(MovementDirection direction) {
            _direction = direction;
        }


        private void AdjustMeleeTriggers(Vector3 rotation) {
            foreach (var trigger in _playerController.meleeTriggers) {
                trigger.transform.rotation = Quaternion.Euler(rotation);
            }
        }

        public bool DoAction(GameObject player) {
            if (_playerRb == null || _playerController == null) {
                _playerRb = player.GetComponent<Rigidbody2D>();
                _playerInitialPosition = player.transform.position;
                _playerController = player.GetComponent<PlayerController>();
            }
            switch(_direction) {
                case MovementDirection.Up:
                    _playerRb.velocity = new Vector2(0, _moveSpeed);
                    AdjustMeleeTriggers(new Vector3(0, 0, 0));
                    _playerController.front = Vector2.up;
                    if (player.transform.position.y >= _playerInitialPosition.y + _moveDistance) {
                        player.transform.position = new Vector2(player.transform.position.x, _playerInitialPosition.y + _moveDistance);
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    } else if (_playerController.collisions.topColliding) {
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    }
                    return false;
                case MovementDirection.Down:
                    _playerController.front = -Vector2.up;
                    AdjustMeleeTriggers(new Vector3(0, 0, 180));
                    _playerRb.velocity = new Vector2(0, -_moveSpeed);
                    if (player.transform.position.y <= _playerInitialPosition.y - _moveDistance) {
                        player.transform.position = new Vector2(player.transform.position.x, _playerInitialPosition.y - _moveDistance);
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    } else if (_playerController.collisions.bottomColliding) {
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    }
                    return false;
                case MovementDirection.Left:
                    _playerController.front = -Vector2.right;
                    _playerRb.velocity = new Vector2(-_moveSpeed, 0);
                    AdjustMeleeTriggers(new Vector3(0, 0, 90));
                    if (player.transform.position.x <= _playerInitialPosition.x - _moveDistance) {
                        player.transform.position = new Vector2(_playerInitialPosition.x - _moveDistance, player.transform.position.y);
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    } else if (_playerController.collisions.leftColliding) {
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    }
                    return false;
                    
                case MovementDirection.Right:
                    _playerController.front = Vector2.right;
                    AdjustMeleeTriggers(new Vector3(0, 0, 270));
                    _playerRb.velocity = new Vector2(_moveSpeed, 0);
                    if (player.transform.position.x >= _playerInitialPosition.x + _moveDistance) {
                        player.transform.position = new Vector2(_playerInitialPosition.x + _moveDistance, player.transform.position.y);
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    } else if (_playerController.collisions.rightColliding) {
                        _playerRb.velocity = Vector2.zero;
                        return true;
                    }
                    return false;
            }
            return false;
        }
    }
}