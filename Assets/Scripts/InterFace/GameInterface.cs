using DG.Tweening;
using AbilitySystem.Manager;
using AbilitySystem.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace AbilitySystem.Interface
{

    public class GameInterface : MonoBehaviour
    {
        [SerializeField] GameObject _buttonPrefab;

        [SerializeField] RectTransform _exitButton;

        [SerializeField] CharacterAbilitySet _skillSet;

        RectTransform _transform;

        readonly List<Button> _skillButtons = new List<Button>();

        void Awake()
        {
            _transform = GetComponent<RectTransform>();

            for (int i = 0; i < _skillSet.Abilities.Count; i++)
            {
                MakeAbilityButton(i, _skillSet.Abilities[i]);
            }
        }

        void OnEnable()
        {
            GameManager.OnGameStateChange += OnGameStateChange;
            AbilityManager.OnSkillCast += OnSkillCast;
        }

        void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
            AbilityManager.OnSkillCast -= OnSkillCast;
        }

        
        void OnGameStateChange(GameManager.GameState oldState, GameManager.GameState newState)
        {
            if (newState == GameManager.GameState.PLAY)
            {
                EnterScreen();
                SetButtonActivities(true);
            }
        }

        void OnSkillCast(int order, int skillsCastedSoFar)
        {
            _skillButtons[order].interactable = false;
            if (skillsCastedSoFar == 3)
            {
                SetButtonActivities(false);
            }
        }

      
        void EnterScreen()
        {
            _transform.DOAnchorPosY(0f, 1f).OnComplete(() =>
            {
                _exitButton.DOAnchorPosX(-110f, 0.5f);
            });
        }

   
        public void LeaveScreen()
        {
            GameManager.Instance.SetState(GameManager.GameState.TRANSITION);
            _exitButton.DOAnchorPosX(145f, 0.5f).OnComplete(() =>
            {
                _transform.DOAnchorPosY(-800f, 3f);
            });
        }

       
        void SetButtonActivities(bool value)
        {
            foreach (Button skillButton in _skillButtons)
            {
                skillButton.interactable = value;
            }
        }

       
        void MakeAbilityButton(int order, CharacterAbility skill)
        {
            GameObject skillButton = Instantiate(_buttonPrefab, _transform);

            skillButton.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(order % 5 * 90f, order / 5 * 90f);

            Button buttonComponent = skillButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(delegate { AbilityManager.Instance.CastSkill(order, skill); });

            skillButton.GetComponent<Image>().sprite = skill.Image;

            _skillButtons.Add(buttonComponent);
        }
    }
}
