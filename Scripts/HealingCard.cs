using UnityEngine;
using CardNSlash;

public class HealingCard : CardBehaviour {
    void Start() {
        _action = new CardHealAction();
    }
    
}