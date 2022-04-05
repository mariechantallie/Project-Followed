//using UnityEditor;
//using UnityEngine;

//namespace UnityStateMachines {
//    [CustomPropertyDrawer(typeof(MonoState))]
//    public class MonoStateDrawer : PropertyDrawer {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
//            string name = property.FindPropertyRelative("Name").stringValue;
//            Debug.Log(name);
//            EditorGUI.BeginProperty(position, label, property);
//            EditorGUI.PrefixLabel(position, new GUIContent(name));
//            EditorGUI.EndProperty();
//        }
//    }
//}
