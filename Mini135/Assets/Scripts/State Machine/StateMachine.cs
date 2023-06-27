using System.Collections;
using UnityEngine;

public class StateMachine : MonoBehaviour {
  [HideInInspector] public State State;

  public Coroutine ForceChangeState(State newState) {
    StopAllCoroutines();
    return StartCoroutine(ChangeStateRoutine(newState));
  }

  public Coroutine ChangeState(State newState) {
    if (State != null && !State.CanExit) return null;
    if (newState == null || !newState.CanEnter) return null;
    StopAllCoroutines();
    return StartCoroutine(ChangeStateRoutine(newState));
  }

  public IEnumerator ChangeStateRoutine(State newState) {
    yield return null;
    if (State != null) State.Exit();
    State = newState;
    State.Machine = this;
    State.Enter();
    yield return StartCoroutine(State.Execute());
  }
}