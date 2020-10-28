using AbilitySystem.Data;
using AbilitySystem.Core;
using AbilitySystem.Game;
using UnityEngine;

namespace AbilitySystem.Manager
{

    public class AbilityManager : MakeSingleton<AbilityManager>
    {
        [SerializeField] float _startingAttackSpeed;
        [SerializeField] float _startingArrowSpeed;

        [SerializeField] GameObject _projectile;

        int _skillsCastedSoFar;

        public IRangedAttack RangedAttack { get; set; }

        public delegate void OnAttackEvent();
        public static OnAttackEvent OnAttack;

        public delegate void OnSkillCastEvent(int skillOrder, int skillsCastedSoFar);
        public static OnSkillCastEvent OnSkillCast;

        void OnEnable()
        {
            GameManager.OnGameStateChange += OnGameStateChange;
        }

        void OnDisable()
        {
            GameManager.OnGameStateChange -= OnGameStateChange;
        }

        void Update()
        {
            if (GameManager.Instance.State == GameManager.GameState.PLAY)
            {
                if (RangedAttack.UpdateTimer(Time.deltaTime))
                {
                    OnAttack?.Invoke();
                    
                }
            }
        }

        public void CastSkill(int order, CharacterAbility characterSkill)
        {
            ISkillStrategy skillStrategy = null;
            switch (characterSkill.Code)
            {
                case AbilityNames.TripleShot:
                    skillStrategy = new UpgradeToTripleShot();
                    break;
                case AbilityNames.DoubleShot:
                    skillStrategy = new UpgradeToDoubleShot();
                    break;
                case AbilityNames.MoreBullet:
                    skillStrategy = new UpgradeAttackSpeed(new AdditionModifier(-1f, characterSkill.name));
                    break;
                case AbilityNames.FastBullet:
                    skillStrategy = new UpgradeProjectileSpeed(new MultiplicationModifier(0.5f, characterSkill.name));
                    break;
                case AbilityNames.Clone:
                    skillStrategy = new CloneTheCharacterRandomly();
                    break;
            }
            Skill skill = new Skill(skillStrategy);
            skill.Perform();
            OnSkillCast?.Invoke(order, ++_skillsCastedSoFar);
        }

        void OnGameStateChange(GameManager.GameState oldState, GameManager.GameState newState)
        {
            if (oldState == GameManager.GameState.MENU && newState == GameManager.GameState.TRANSITION)
            {
                RangedAttack = new DefaultRangedAttack(_projectile, _startingAttackSpeed, _startingArrowSpeed);
            }
            if (oldState == GameManager.GameState.TRANSITION && newState == GameManager.GameState.MENU)
            {
                Clean();
            }
        }

        void Clean()
        {
            Character[] characters = FindObjectsOfType<Character>();
            if (characters.Length > 1)
            {
                foreach (Character character in characters)
                {
                    if (character.transform != transform)
                    {
                        PoolingManager.Instance.Add(character.gameObject);
                    }
                }
            }
            _skillsCastedSoFar = 0;
        }
    }
}
