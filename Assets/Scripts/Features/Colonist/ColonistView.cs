// using System;
// using UnityEngine;
//
// namespace Systems.ColonistSystem {
//     public class ColonistView : MonoBehaviour {
//         private Animator _animator;
//         private ColonistController _colonistController;
//         private SpriteRenderer _spriteRenderer;
//         private Colonist _colonist;
//         
//         private void Awake() {
//             _spriteRenderer = GetComponent<SpriteRenderer>();
//             _colonistController = GetComponent<ColonistController>();
//             _animator = GetComponent<Animator>();
//             _colonist = GetComponent<Colonist>();
//         }
//
//         private void Start() {
//             UpdateVisuals();
//         }
//
//         public void UpdateVisuals() {
//             if (_colonist == null) return;
//
//             // Update sprite based on current state
//             _spriteRenderer.sprite = _colonist.Portrait;
//
//             // Update animator parameters
//             _animator.SetFloat("Health", _colonist.ColonistStats.CurrentHealth);
//             _animator.SetFloat("Stamina", _colonist.ColonistStats.CurrentStamina);
//         }
//     }
// }