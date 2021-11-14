using System;
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
        [SerializeField] private List<BaseAction> actions;

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

        private void Start()
        {
            StartCoroutine(SelectActionAI());
        }

        private IEnumerator SelectActionAI()
        {
            BaseAction actionInProgress = actions.OrderBy(p => p.GetScores()).Last();
            while (_isEnable)
            {
                yield return new WaitForSeconds(0.1f);
                var biggestAction = actions.OrderBy(p => p.GetScores()).Last();
                if (!biggestAction.Equals(actionInProgress))
                {
                    actionInProgress.Stop();
                }
                biggestAction.Play();
                actionInProgress = biggestAction;
            }
            
            foreach (var action in actions)
            {
                action.Stop();
            }
        }
    }
}