using DG.Tweening;
using AbilitySystem.Manager;
using UnityEngine;

namespace AbilitySystem.Interface
{

    public class MenuInterface : MonoBehaviour
    {
        RectTransform _transform;

        void OnEnable()
        {
            GameManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
        }

        void Awake()
        {
            _transform = GetComponent<RectTransform>();
        }

        void OnGameStateChange(GameManager.GameState oldState, GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.MENU)
            {
                EnterScreen();
            }
        }

        void EnterScreen()
        {
            _transform.DOAnchorPosX(0f, 1f);
        }

        public void LeaveScreen()
        {
            _transform.DOAnchorPosX(-800f, 1f).OnComplete(() =>
            {
                _transform.anchoredPosition = new Vector2(800f, 0f);
                GameManager.Instance.SetState(GameManager.GameState.TRANSITION);
            });
        }
    }
}
