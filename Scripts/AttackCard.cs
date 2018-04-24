using UnityEngine;
using CardNSlash;

public class AttackCard : CardBehaviour {

    [SerializeField] private CardAttackAction.AttackType _attackType;
    
    void Start() {
        _action = new CardAttackAction(_attackType);
    }


}