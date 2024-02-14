﻿using DG.Tweening;
using Skibidi.Components.Events;
using FluffyUnderware.Curvy.Controllers;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.UI;

namespace Skibidi.Views
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private SplineController _splineController;
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider _collider;
        [SerializeField] private Transform _modelTransform;
        [SerializeField] private Slider _healthSlider;

        [SerializeField] private float _speed = 6;
        [SerializeField] private float _punchInterval = 2f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private int _health = 3;
        [SerializeField] private float _punchDuration = .4f;
        [SerializeField] private float _lookRotation = .2f;
        
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Defend1 = Animator.StringToHash("Defend");

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; set; }
        public EcsWorld EcsEventWorld { get; set; }

        public int Damage => _damage;
        public int Health => _health;
        public float PunchInterval => _punchInterval;
        public float Speed => _speed;
        public float PunchDuration => _punchDuration;

        public void SetSpeed(float speed)
        {
            _splineController.Speed = speed;
        }

        public void SetHealthValue(int value)
        {
            _healthSlider.maxValue = value;
            _healthSlider.value = value;
        }

        public void ChangeHealthValue(int newValue)
        {
            _healthSlider.value = newValue;
        }

        public void UpdateSliderBillboard(Vector3 camPosition)
        {
            _healthSlider.transform.LookAt(_healthSlider.transform.position + camPosition);
        }

        public void ControlPointReached(CurvySplineMoveEventArgs args)
        {
            var entity = EcsEventWorld.NewEntity();
            ref var eventComponent = ref EcsEventWorld.GetPool<MovementEvent>().Add(entity);
            eventComponent.View = this;
            eventComponent.Status = MoveStatus.End;
        }

        public void Disable()
        {
            _collider.enabled = false;
        }

        public void AttackAnimation()
        {
            _animator.SetTrigger(Attack);
        }

        public void DieAnimation()
        {
            _animator.SetTrigger(Die);
        }

        public void ToggleWalkState(bool state)
        {
            _animator.SetBool(Walk, state);
        }

        public void HitAnimation()
        {
            _animator.SetTrigger(Hit);
        }

        public void LookAtTarget(Transform target)
        {
            _modelTransform.DOLookAt(target.position, _lookRotation);
        }

        public void ResetLook()
        {
            _modelTransform.DOLocalRotate(Vector3.zero, _lookRotation);
        }

        public void Defend(bool active)
        {
            _animator.SetBool(Defend1, active);
        }

        public void Restore()
        {
            _splineController.Position = 0f;
            _splineController.Speed = 0;
        }
    }
}