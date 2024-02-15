using System;
using System.Collections.Generic;
using UnityEngine;
using Skibidi.Views;

namespace Skibidi.Services
{
    public class SceneService : MonoBehaviour
    {
        [SerializeField] private UnitView _player;
        [SerializeField] private GamePopupView _popupView;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private EnemyByRow[] _enemyByRows;
        [SerializeField] private InputButtonView _attackView;
        [SerializeField] private InputButtonView _defendView;
        
        public UnitView Player => _player;
        public EnemyByRow[] EnemyByRows => _enemyByRows;

        public Camera MainCamera => _mainCamera;
        public GamePopupView PopupView => _popupView;
        public InputButtonView AttackView => _attackView;
        public InputButtonView DefendView => _defendView;
    }

    [Serializable]
    public struct EnemyByRow
    {
        [SerializeField] private List<UnitView> _units;

        public List<UnitView> Units => _units;
    }
}