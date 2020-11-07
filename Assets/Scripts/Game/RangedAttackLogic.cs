using DG.Tweening;
using AbilitySystem.Core;
using AbilitySystem.Manager;
using UnityEngine;

namespace AbilitySystem.Game
{

    public interface IRangedAttack
    {
       
        Attribute AttackSpeed { get; }
        Attribute ProjectileSpeed { get; }
        bool UpdateTimer(float deltaTime);
        void Shoot(Vector3 position, float direction);
       
    }

    public class DefaultRangedAttack : IRangedAttack
    {
        const float _PROJECTILE_RANGE_ = 15f;

        float _timer;
        
        readonly GameObject _projectile;

        public Attribute AttackSpeed { get; }
        public Attribute ProjectileSpeed { get; }

        public DefaultRangedAttack(GameObject projectile, float attackSpeed, float projectileSpeed)
        {
            _projectile = projectile;

            AttackSpeed = new Attribute(attackSpeed);
            ProjectileSpeed = new Attribute(projectileSpeed);
        }

        public bool UpdateTimer(float deltaTime)
        {
            if ((_timer += deltaTime) > AttackSpeed.Value)
            {
                _timer = 0f;
                return true;
            }
            return false;
        }

        public void Shoot(Vector3 position, float direction)
        {
            GameObject projectile = PoolingManager.Instance.Get(_projectile, position, Quaternion.Euler(0f, 0f, direction - 90f));

            Vector3 target = projectile.transform.position;

            target.x += _PROJECTILE_RANGE_ * Mathf.Cos(direction * Mathf.Deg2Rad);
            target.y += _PROJECTILE_RANGE_ * Mathf.Sin(direction * Mathf.Deg2Rad);

            projectile.transform.DOMove(target, _PROJECTILE_RANGE_ / ProjectileSpeed.Value).OnComplete(() => {
                PoolingManager.Instance.Add(projectile);
                PoolingManager.OnArrow.Invoke();
            });
        }
    }

    public abstract class RangedAttackDecorator : IRangedAttack
    {
        protected IRangedAttack _rangedAttack;

        public Attribute AttackSpeed { get { return _rangedAttack.AttackSpeed; } }
        public Attribute ProjectileSpeed { get { return _rangedAttack.ProjectileSpeed; } }

        public RangedAttackDecorator(IRangedAttack rangedAttack)
        {
            _rangedAttack = rangedAttack;
        }

        public virtual bool UpdateTimer(float deltaTime)
        {
            return _rangedAttack.UpdateTimer(deltaTime);
        }

        public virtual void Shoot(Vector3 position, float direction)
        {
            _rangedAttack.Shoot(position, direction);
        }
    }

    public class TripleShotAttack : RangedAttackDecorator
    {
        public TripleShotAttack(IRangedAttack rangedAttack) : base(rangedAttack) { }

       
        public override void Shoot(Vector3 position, float direction)
        {
            base.Shoot(position, direction - 45f);
            base.Shoot(position, direction);
            base.Shoot(position, direction + 45f);
        }
    }

    public class DoubleShotAttack : RangedAttackDecorator
    {
        const float _DURATION_BETWEEN_ATTACKS_ = 0.15f;

        float _secondAttackTimer;

        public DoubleShotAttack(IRangedAttack rangedAttack) : base(rangedAttack)
        {
            _secondAttackTimer = 0f;
        }

        public override bool UpdateTimer(float deltaTime)
        {
            if (_secondAttackTimer == 0f)
            {
                if (base.UpdateTimer(deltaTime))
                {
                    _secondAttackTimer = _DURATION_BETWEEN_ATTACKS_;
                    return true;
                }
                return false;
            }
            else
            {
                _secondAttackTimer -= deltaTime;
                if (_secondAttackTimer <= 0f)
                {
                    _secondAttackTimer = 0f;
                    return true;
                }
                return false;
            }
        }

        public override void Shoot(Vector3 position, float direction)
        {
            base.Shoot(position, direction);
        }
    }
}