using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class State : MonoBehaviour {
  [HideInInspector] public StateMachine Machine;
  public virtual bool CanEnter => true;
  public virtual bool CanExit => true;
  public virtual float ExecutionTime => 0f; 
  public virtual void Enter() { }
  public virtual IEnumerator Execute() { yield return null; }
  public virtual void Exit() { }
}