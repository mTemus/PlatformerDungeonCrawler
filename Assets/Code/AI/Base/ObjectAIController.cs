﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ObjectAIController : NestedComponent
{
    protected List<AIState> m_aiStates;
    protected AIState m_currentAIState;

    protected abstract void CreateObjectAI();

    protected void HandleState()
    {
        m_currentAIState.Update(this);
    }

    public void SetNewAIState(AIStateType stateType)
    {
        var state = m_aiStates.FirstOrDefault(state => state.AIStateType == stateType);

        if (state == null)
        {
            Debug.LogError($"Can't find AI State {stateType} in {transform.parent.name}!");
            return;
        }

        var eventsContainer = GetComponentFromRoot<ObjectEventsContainer>();
        
        eventsContainer.CallEvent(AIEvents.BeforeStateChanged, m_currentAIState);
        m_currentAIState.OnStateChanged();
        m_currentAIState = state;
        m_currentAIState.OnStateSet();
        eventsContainer.CallEvent(AIEvents.AfterStateChanged, state);
    }
}