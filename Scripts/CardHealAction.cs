using UnityEngine;
using CardNSlash;
namespace CardNSlash {

    public class CardHealAction : ICardAction {
        private readonly float _healAmount = 25f;

        public bool DoAction(GameObject player) {
            player.GetComponent<PlayerController>().Heal(_healAmount);
            return true;
        }
    }


}