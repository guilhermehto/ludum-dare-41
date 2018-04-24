using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CardNSlash;

public class TornadoCard : CardBehaviour {

    void Start() {
        _action = new CardTornadoAction();    
    }
	
	
}
