#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Jestering.Rating
{
    [CustomEditor(typeof(KingRating))]
    public class KingRatingInspector : UnityEditor.Editor
    {
        [Min(1)]
        private int _complexityInput = 1;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var kingRating = target as KingRating;
            if (!kingRating)
                return;
            
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();

            _complexityInput = EditorGUILayout.IntField(_complexityInput);
            
            if (GUILayout.Button("Generate new request"))
            {
                kingRating.NewRequest(_complexityInput);
            }
            
            EditorGUILayout.EndHorizontal();
            
            if (GUILayout.Button("Reset Request"))
            {
                kingRating.ResetRequest();
            }
        }
    }
}
#endif
