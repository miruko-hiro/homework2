using Base.Input;
using Base.Search;
using UnityEngine;

namespace Base.Game
{
    public class ZombieComponent : MonoBehaviour
    {

        [Space(10)] 
        
        [SerializeField] private AIntelligence ai; 
        
        [SerializeField] private GameObject _aliveView;

        [SerializeField] private GameObject _diedView;

        // [SerializeField] private float _speed = 5f;
        //
        // [SerializeField] private Rigidbody _rigidbody;
        //
        // [SerializeField] private Vector3[] _deltaPath;

        // private int _currentPoint = 0;
        // private Vector3 _initPosition;
        
        public bool IsInCover { get; private set; }

        private void Awake()
        {
            IsInCover = false;
            // _initPosition = transform.position;
        }

        private void OnEnable()
        {
            SetState(true);
        }

        // private void FixedUpdate()
        // {
        //         if (_deltaPath == null || _deltaPath.Length < 2)
        //             return;
        //
        //         var direction = _initPosition + _deltaPath[_currentPoint] - transform.position;
        //         _rigidbody.velocity = IsAlive ? direction.normalized * _speed : Vector3.zero;
        //
        //         if (direction.magnitude <= 0.1f)
        //         {
        //             _currentPoint = (_currentPoint + 1) % _deltaPath.Length;
        //         }
        // }

        public void SetState(bool alive)
        {
            _aliveView.SetActive(alive);
            _diedView.SetActive(!alive);
            ai.IsEnable = alive;
        }

        public bool IsAlive => _aliveView.activeInHierarchy;
    }
}