using System;

[Serializable]
public class Player {
  public string Name;
  public int Points;
  public bool IsAlive;
  public bool IsGuessing;

  public void GetShot() => IsAlive = false;
}