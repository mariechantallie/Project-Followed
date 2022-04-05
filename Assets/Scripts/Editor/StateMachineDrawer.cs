using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StateMachine))]
public class StateMachineDrawer : PropertyDrawer {
    private StateMachine target = null;
    private int currentSelection;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if(property == null) { return; }

        target ??= fieldInfo.GetValue(property.serializedObject.targetObject) as StateMachine;

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(position, label);

        UpdatePosition(ref position);
        currentSelection = EditorGUI.Popup(position, CurrentActiveStateIndex, GetCurrentStates());

        if(GUI.changed) {
            target.ChangeState(target.States[currentSelection]);
        }

        EditorGUI.EndProperty();
        GUI.enabled = HasStates;
    }

    private static void UpdatePosition(ref Rect position) {
        position.x += EditorGUIUtility.labelWidth;
        position.width -= EditorGUIUtility.labelWidth;
    }

    private bool HasStates => (target.States.Count > 0);

    private GUIContent[] GetCurrentStates() {
        GUIContent[] result = new GUIContent[target.States.Count];
        for(int i = 0; i < result.Length; i++) {
            result[i] = new GUIContent(target.States[i].Name);
        }
        return result;
    }

    private int CurrentActiveStateIndex => target.States.IndexOf(target.CurrentState);
}
