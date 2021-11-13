using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base.Game;
using UnityEngine;
using Utility_AI.Scorers;

namespace Utility_AI
{
    public class ZombieAI: MonoBehaviour
    {
        [SerializeField] private Transform _transformEnemy;
        [SerializeField] private LevelMap _levelMap;
        
        private ActionAI _runningAwayFromEnemy;
        private ActionAI _hidingFromEnemy;
        private ActionAI _dodgingEnemy;

        private List<ActionAI> _actions;

        private void Awake()
        {
            _runningAwayFromEnemy = new ActionAI(new DistanceToEnemy(transform, _transformEnemy), new EnemyNotSee());
            _hidingFromEnemy = new ActionAI(new CloseToCover(_levelMap, transform), new IsInCover(GetComponent<ZombieComponent>()));
            _dodgingEnemy = new ActionAI(new EnemyNotSee());

            _actions = new List<ActionAI>
            {
                _runningAwayFromEnemy, 
                _hidingFromEnemy, 
                _dodgingEnemy
            };
        }

        private void Start()
        {
            StartCoroutine(SelectActionAI());
        }

        private IEnumerator SelectActionAI()
        {
            while (true)
            {
                ActionAI actionAI = _actions.OrderBy(p => p.Score).First();

                if (actionAI == _runningAwayFromEnemy) Runnung();
                else if (actionAI == _hidingFromEnemy) Hidding();
                else Dodging();
                
                yield return new WaitForSeconds(1f);
            }
        }

        private void Runnung()
        {
            Debug.Log("Running");
        }

        private void Hidding()
        {
            Debug.Log("Hidding");
        }

        private void Dodging()
        {
            Debug.Log("Dodging");
        }
    }
}