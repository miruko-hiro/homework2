using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Base.Search;
using UnityEngine;
using UtilityAI.Actions;

namespace UtilityAI
{
    public class ZombieAI: AIntelligence
    {
        [SerializeField] private float updateTime = 0.1f;
        private List<BaseAction> _actions;

        private bool _isEnable = true;
        public override bool IsEnable
        {
            get => _isEnable;
            set
            {
                if(_isEnable && value) return;
                _isEnable = value;
                if(_isEnable) StartCoroutine(SelectActionAI());
            }
        }

        private void Awake()
        {
            _actions = GetComponentsInChildren<BaseAction>().ToList();
        }

        private void Start()
        {
            StartCoroutine(SelectActionAI());
        }

        private IEnumerator SelectActionAI()
        {
            bool someoneIsActive;
            // BaseAction actionInProgress = _actions.OrderBy(p => p.GetScores()).Last();
            while (_isEnable)
            {
                yield return new WaitForSeconds(updateTime);
                someoneIsActive = false;
                // var biggestAction = _actions.OrderBy(p => p.GetScores()).Last();
                // if (!biggestAction.Equals(actionInProgress))
                // {
                //     actionInProgress.Stop();
                // }
                // biggestAction.Play();
                // actionInProgress = biggestAction;
                
                foreach (BaseAction action in _actions)
                {
                    if (action.IsEnabled)
                    {
                        someoneIsActive = true;
                        break;
                    }
                }
                
                if(!someoneIsActive) _actions.OrderBy(p => p.GetScores()).Last().Play();
            }
            
            foreach (var action in _actions)
            {
                action.Stop();
            }
        }
    }
}