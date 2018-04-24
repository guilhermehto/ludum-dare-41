using UnityEngine;

namespace CardNSlash {
    public class CardTornadoAction : ICardAction {


        private readonly float _spinTime = 0.75f;
        private readonly int _spins = 3;
        private readonly int _spinSpeed = 900;

        private bool _triggerActivated = false;
        private PlayerController _pc = null;

        private float _timer = 0;
        private Vector3 _originalEuler;
        private int _completeRotations = 0;

        public bool DoAction(GameObject player) {
            if (_pc == null) {
                _pc = player.GetComponent<PlayerController>();
                _pc.ActivateMeleeTrigger(CardAttackAction.AttackType.LightAttack, true);
                _originalEuler = player.transform.rotation.eulerAngles;

            }

            var currentRotation = player.transform.rotation.eulerAngles;
            currentRotation.z += Time.deltaTime * _spinSpeed;
            if (currentRotation.z >= 360) {
                _completeRotations++;
                if (_completeRotations == _spins) {
                    player.transform.rotation = Quaternion.Euler(_originalEuler);
                    _pc.ActivateMeleeTrigger(CardAttackAction.AttackType.LightAttack, false);
                    return true;
                }
            }

            player.transform.rotation = Quaternion.Euler(currentRotation);


            // _timer += Time.deltaTime;
            // if (_timer >= _spinTime) {
            //     _pc.ActivateMeleeTrigger(CardAttackAction.AttackType.LightAttack, false);
            //     player.GetComponent<Rigidbody2D>().angularVelocity = 0;
            //     player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //     return true;
            // }

            return false;
        }
    }
}