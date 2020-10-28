using UnityEngine;

namespace AbilitySystem.Data
{
    [CreateAssetMenu(fileName = "CharacterAbility", menuName = "Character Ability", order = 1)]


    public class CharacterAbility : ScriptableObject
    {
        [SerializeField]
        private AbilityNames _names;

        [SerializeField]
        private Sprite _image;

        public AbilityNames Code { get { return _names; } }
  
        public Sprite Image { get { return _image; } }

    }

    public enum AbilityNames
    {
        TripleShot, DoubleShot, MoreBullet, FastBullet, Clone,
    }
}
