using System.Collections.Generic;
using UnityEngine;

namespace AbilitySystem.Data
{

    [CreateAssetMenu(fileName = "NewCharacterSkillSet", menuName = "Character Skill Set", order = 2)]
    public class CharacterAbilitySet : ScriptableObject
    {
        [SerializeField] List<CharacterAbility> _abilities;


        public List<CharacterAbility> Abilities { get { return _abilities; } }


        public void AddAbility(CharacterAbility skill, int index)
        {
            Abilities.Insert(index, skill);
        }

        public void RemoveAbility(int index)
        {
            Abilities.RemoveAt(index);
        }
    }
}
