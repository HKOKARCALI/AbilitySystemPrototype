using DG.Tweening;
using AbilitySystem.Manager;
using UnityEngine;

namespace AbilitySystem.Game
{
 
    public class Character : MonoBehaviour
    {
        const float _LINEAR_SPEED_ = 3f;
        const float _ANGULAR_SPEED_ = 180f;

        [SerializeField] Vector3 _inScreenPosition;
        [SerializeField] Vector3 _outScreenPosition;
        public Animator animator;
        void OnEnable()
        {
            GameManager.OnGameStateChange += OnGameStateChange;
            AbilityManager.OnAttack += OnAttack;
            PoolingManager.OnArrow += OnArrow;
        }

        void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
            AbilityManager.OnAttack -= OnAttack;
            PoolingManager.OnArrow -= OnArrow;
        }

        void OnGameStateChange(GameManager.GameState oldState, GameManager.GameState newState)
        {
            if (oldState == GameManager.GameState.MENU && newState == GameManager.GameState.TRANSITION)
            {
                EnterScreen();
                animator.SetBool("run", true);
            }

            if (oldState == GameManager.GameState.PLAY && newState == GameManager.GameState.TRANSITION)
            {
                LeaveScreen();
                animator.SetBool("run", true);
            }
        }
        void OnArrow()
        {

            animator.SetBool("attack", true);

        }

        void OnAttack()
        {
            
            AbilityManager.Instance.RangedAttack.Shoot(transform.position, transform.eulerAngles.x);

            animator.SetBool("attack", true);

        }  


        void FaceTo(Vector3 worldPosition)
        {
            Vector3 diff = worldPosition - transform.position;
            float degree = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

           // transform.DORotateQuaternion(Quaternion.Euler(degree, -90f, 90f), Mathf.Abs(degree) / _ANGULAR_SPEED_);
        }

        float GetTraverseDuration(Vector3 worldPosition)
        {
            return Vector3.Distance(transform.position, worldPosition) / _LINEAR_SPEED_;
        }

 
        void EnterScreen()
        {
            FaceTo(_inScreenPosition);
            transform.DOMove(_inScreenPosition, GetTraverseDuration(_inScreenPosition)).OnComplete(() => {
                GameManager.Instance.SetState(GameManager.GameState.PLAY);
                animator.SetBool("run", false);
            });
        }

        void LeaveScreen()
        {
            FaceTo(_outScreenPosition);
            transform.DOMove(_outScreenPosition, GetTraverseDuration(_outScreenPosition)).OnComplete(() => {
                GameManager.Instance.SetState(GameManager.GameState.MENU);
            });
        }
    }
}
