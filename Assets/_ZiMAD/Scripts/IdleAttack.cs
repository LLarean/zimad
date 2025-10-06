using UnityEngine;

public class IdleAttack : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    
    private Animator _animator;
    private float _nextIdleAttackTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
        _nextIdleAttackTime = Time.time + Random.Range(3f, 7f);
    }

    private bool IsIdle()
    {
        return _animator.GetFloat("Speed") < 0.1f;
    }
}