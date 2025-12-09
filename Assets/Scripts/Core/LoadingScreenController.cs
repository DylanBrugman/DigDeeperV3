// using UnityEngine;
//
// namespace Core {
//     public class LoadingScreenController : MonoBehaviour
//     {
//         [Header("UI References")]
//         [SerializeField] private UnityEngine.UI.Slider progressBar;
//         [SerializeField] private TMPro.TextMeshProUGUI loadingText;
//         [SerializeField] private TMPro.TextMeshProUGUI tipText;
//         
//         [Header("Loading Tips")]
//         [SerializeField] private string[] loadingTips = {
//             "Tip: Colonists with higher mining skills dig faster!",
//             "Tip: Watch the structural integrity - deep excavations can collapse!",
//             "Tip: Keep an eye on your colonists' needs to maintain productivity.",
//             "Tip: Different resources are found at different depths.",
//             "Tip: Build support structures to prevent cave-ins."
//         };
//         
//         private void Start()
//         {
//             // Subscribe to loading events
//             SceneController.OnSceneLoadProgress += UpdateProgress;
//             SceneController.OnSceneLoadStarted += OnLoadingStarted;
//             SceneController.OnSceneLoadCompleted += OnLoadingCompleted;
//             
//             // Show random tip
//             if (tipText != null && loadingTips.Length > 0)
//             {
//                 tipText.text = loadingTips[UnityEngine.Random.Range(0, loadingTips.Length)];
//             }
//         }
//         
//         private void OnLoadingStarted(string sceneName)
//         {
//             if (loadingText != null)
//             {
//                 loadingText.text = $"Loading {sceneName}...";
//             }
//         }
//         
//         private void UpdateProgress(float progress)
//         {
//             if (progressBar != null)
//             {
//                 progressBar.value = progress;
//             }
//         }
//         
//         private void OnLoadingCompleted(string sceneName)
//         {
//             if (loadingText != null)
//             {
//                 loadingText.text = "Complete!";
//             }
//         }
//         
//         private void OnDestroy()
//         {
//             // Unsubscribe from events
//             SceneController.OnSceneLoadProgress -= UpdateProgress;
//             SceneController.OnSceneLoadStarted -= OnLoadingStarted;
//             SceneController.OnSceneLoadCompleted -= OnLoadingCompleted;
//         }
//     }
// }