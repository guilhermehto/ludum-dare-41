using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;


public class MovementCard : CardBehaviour {

    [SerializeField] private CardMoveAction.MovementDirection _moveDirection;

    void Start() {
        _action = new CardMoveAction(_moveDirection);
    }
		
}
