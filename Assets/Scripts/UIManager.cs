using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [field: SerializeField] public PlayerCharacter Player { get; set; }
    [field: SerializeField] public TMP_Text CharacterStateStatus { get; set; }
    [field: SerializeField] public GameManager Game { get; set; }
    [field: SerializeField] public TMP_Text GameStateStatus { get; set; }

    private StateMachine PlayerState => Player.CharacterState;
    private StateMachine GameState => Game.GameState;

    private void OnEnable() {
        PlayerState.OnStateChange += UpdateCharacterStateStatus;
        GameState.OnStateChange += UpdateGameStateStatus;
    }

    private void OnDisable() {
        PlayerState.OnStateChange -= UpdateCharacterStateStatus;
        GameState.OnStateChange -= UpdateGameStateStatus;
    }

    private void UpdateCharacterStateStatus(StateTransision transision) {
        CharacterStateStatus.text = transision.To.Name;
    }
    private void UpdateGameStateStatus(StateTransision transision) {
        GameStateStatus.text = transision.To.Name;
    }
}
