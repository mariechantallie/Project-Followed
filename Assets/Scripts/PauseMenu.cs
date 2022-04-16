using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [SerializeField] GameStateChangeEvent gameChangeState;
    [SerializeField] GameObject pauseMenuUI;

    void OnEnable() {
        gameChangeState.OnGameStateChange += ChangePauseMenu;
    }

    void OnDisable() {
        gameChangeState.OnGameStateChange += ChangePauseMenu;
    }

    void ChangePauseMenu(StateTransision transision) {
        if(pauseMenuUI.activeSelf) {
            pauseMenuUI.SetActive(false);
        } else {
            pauseMenuUI.SetActive(true);
        }
    }
}
