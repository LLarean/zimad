using UnityEngine;

namespace ZIMAD
{
    public class IdleAttack : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Animator _animator;

        private float _nextIdleAttackTime;

        private void Start()
        {
            ScheduleNextIdleAttack();
        }

        private void Update()
        {
            if (Time.time >= _nextIdleAttackTime && IsIdle())
            {
                _animator.SetTrigger("IdleAttack");
                ScheduleNextIdleAttack();
            }
        }

        private void OnAttackHit()
        {
            _playerController.CreateFX("AttackEffect");
        }

        private void ScheduleNextIdleAttack()
        {
            _nextIdleAttackTime = Time.time + Random.Range(2f, 5f);
        }

        private bool IsIdle()
        {
            return _animator.GetFloat("Speed") < 0.1f;
        }
    }
}