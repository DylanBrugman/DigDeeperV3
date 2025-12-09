using System.Linq;
using Core.ECS;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
/// <summary>
/// Custom Inspector that displays the per‑system timings in a compact list.
/// </summary>
[CustomEditor(typeof(ECSSystemRunner))]
public class ECSSystemRunnerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector first (toggles, etc.).
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Per‑System Timings (ms)", EditorStyles.boldLabel);

        ECSSystemRunner runner = (ECSSystemRunner)target;

        if (Application.isPlaying)
        {
            // Order by slowest to fastest for at‑a‑glance hot‑spot spotting.
            foreach (ECSSystemInfo info in runner.SystemsInfo.OrderByDescending(i => i.LastFrameMs))
            {
                EditorGUILayout.LabelField(info.Name, $"{info.LastFrameMs:0.000}");
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Timings appear while the scene is running.", MessageType.Info);
        }
    }
}
#endif