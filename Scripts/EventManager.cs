namespace CardNSlash {
    public static class EventManager {

        public delegate void PlaceCardAction();
	    public static event PlaceCardAction OnCardPlaced;

        public static void CardPlaced() {
            if (OnCardPlaced != null) {
                OnCardPlaced();
            }
        }

        public delegate void ActionQueued(ICardAction action);
	    public static event ActionQueued OnActionQueued;

        public static void QueueAction(ICardAction action) {
            if (OnActionQueued != null) {
                OnActionQueued(action);
            }
        }

        public delegate void ShakeCameraAction(CameraShake.ShakeStrength force);
	    public static event ShakeCameraAction OnCameraShake;

        public static void ShakeCamera(CameraShake.ShakeStrength force) {
            if (OnCameraShake != null) {
                OnCameraShake(force);
            }
        }
        

        public delegate void EnemyKilledAction();
	    public static event EnemyKilledAction OnEnemyKilled;

        public static void EnemyKilled() {
            if (OnEnemyKilled != null) {
                OnEnemyKilled();
            }
        }

        public delegate void GameEndAction();
	    public static event GameEndAction OnGameEnded;

        public static void GameEnded() {
            if (OnGameEnded != null) {
                OnGameEnded();
            }
        }

    }
}