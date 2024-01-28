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

        private JesterObject _jesterObjectToRate;
        
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
            
            if(kingRating.CurrentRequest.LoveRequest.category == JesterObject.ItemCategory.None)
                return;
            
            GUILayout.Space(10);
            
            EditorGUILayout.BeginHorizontal();

            _jesterObjectToRate = EditorGUILayout.ObjectField(_jesterObjectToRate, typeof(JesterObject)) as JesterObject;
            
            if (GUILayout.Button("Rate Jester Object"))
            {
                if(!_jesterObjectToRate)
                    return;
                
                kingRating.RateObject(_jesterObjectToRate, out _);
            }
            
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
