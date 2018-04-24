using UnityEngine;

namespace CardNSlash {

    public class CardAttackAction : ICardAction {

        
        private readonly float _attackSpeed = 10f;
        private readonly float _dashDistance = 1.25f;

        private AttackType _attackType;
        private bool _triggerActivated;
        private Vector2 _initialPosition;
        private PlayerController _pc;

        public enum AttackType {
            LightAttack,
            HeavyAttack,
        }

        public CardAttackAction(AttackType type) {
            _attackType = type;
        }

        public bool DoAction(GameObject player) {
            if (!_triggerActivated) {
                _pc = player.GetComponent<PlayerController>();
                _pc.ActivateMeleeTrigger(_attackType, true);
                _triggerActivated = true;
                _initialPosition = player.transform.position;
                player.GetComponent<Rigidbody2D>().velocity = -_pc.front * _attackSpeed;
            }

            if (Vector2.Distance(player.transform.position, _initialPosition) >= _dashDistance) {
                return FinishAttack(player);
            } else if (_pc.collisions.topColliding && _pc.front == -Vector2.up) {
                return FinishAttack(player);
            } else if (_pc.collisions.rightColliding && _pc.front == -Vector2.right) {
                return FinishAttack(player);
            } else if (_pc.collisions.leftColliding && _pc.front == Vector2.right) {
                return FinishAttack(player);
            } else if (_pc.collisions.bottomColliding && _pc.front == Vector2.up) {
                return FinishAttack(player);
            }

            return false;
        }

        private bool FinishAttack(GameObject player) {
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<PlayerController>().ActivateMeleeTrigger(_attackType, false);
            return true;
        }
    }



}