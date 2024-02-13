using System;
using System.Collections.Generic;
using UnityEngine;
using Skibidi.Views;

namespace Skibidi.Services
{
    public class SceneService : MonoBehaviour
    {
        [SerializeField] private UnitView _player;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private EnemyByRow[] _enemyByRows;
        
        public UnitView Player => _player;
        public EnemyByRow[] EnemyByRows => _enemyByRows;

        public Camera MainCamera => _mainCamera;
    }

    [Serializable]
    public struct EnemyByRow
    {
        [SerializeField] private List<UnitView> _units;

        public List<UnitView> Units => _units;
    }
}