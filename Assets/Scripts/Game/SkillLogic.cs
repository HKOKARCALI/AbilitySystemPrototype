using AbilitySystem.Core;
using AbilitySystem.Manager;
using UnityEngine;

namespace AbilitySystem.Game
{

    public interface ISkillStrategy
    {

        void Perform();
    }

    public class Skill
    {
        public ISkillStrategy Strategy { get; private set; }

        public Skill(ISkillStrategy strategy)
        {
            Strategy = strategy;
        }

        public void Perform()
        {
            Strategy.Perform();
        }
    }

    public class UpgradeToTripleShot : ISkillStrategy
    {
        public void Perform()
        {
            AbilityManager.Instance.RangedAttack = new TripleShotAttack(AbilityManager.Instance.RangedAttack);
        }
    }

    public class UpgradeToDoubleShot : ISkillStrategy
    {
        public void Perform()
        {
            AbilityManager.Instance.RangedAttack = new DoubleShotAttack(AbilityManager.Instance.RangedAttack);
        }
    }

    public class UpgradeAttackSpeed : ISkillStrategy
    {
        readonly Modifier _modifier;

        public UpgradeAttackSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform()
        {
            AbilityManager.Instance.RangedAttack.AttackSpeed.AddModifier(_modifier);
        }
    }

    public class UpgradeProjectileSpeed : ISkillStrategy
    {
        readonly Modifier _modifier;

        public UpgradeProjectileSpeed(Modifier modifier)
        {
            _modifier = modifier;
        }

        public void Perform()
        {
            AbilityManager.Instance.RangedAttack.ProjectileSpeed.AddModifier(_modifier);
        }
    }

    public class CloneTheCharacterRandomly : ISkillStrategy
    {
        const float _Z_ = -.5f;

        /* Scene Extents... */
        readonly Vector3 _MIN_ = new Vector3(-0.1f, -4f,-0.5f);
        readonly Vector3 _MAX_ = new Vector3(0.1f, 2f, 0.5f);

        public void Perform()
        {
            GameObject objectToBeCloned = AbilityManager.Instance.gameObject;

            Vector3 rndpos = new Vector3(Random.Range(_MIN_.x, _MAX_.x), Random.Range(_MIN_.y, _MAX_.y), Random.Range(_MIN_.z, _MAX_.z));

            GameObject clone = PoolingManager.Instance.Get(objectToBeCloned, rndpos, objectToBeCloned.transform.rotation);
            
            Object.Destroy(clone.GetComponent<AbilityManager>());
        }
    }
}