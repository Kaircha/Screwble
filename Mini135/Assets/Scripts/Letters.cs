using UnityEngine;
using System.Collections.Generic;

public static class Letters {
  public static List<LetterPointsPair> All => new() { A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z };

  public static LetterPointsPair A => new('a', 3, 0.08167f);
  public static LetterPointsPair B => new('b', 6, 0.01492f);
  public static LetterPointsPair C => new('c', 4, 0.02782f);
  public static LetterPointsPair D => new('d', 5, 0.04253f);
  public static LetterPointsPair E => new('e', 1, 0.12702f);
  public static LetterPointsPair F => new('f', 5, 0.02228f);
  public static LetterPointsPair G => new('g', 5, 0.02015f);
  public static LetterPointsPair H => new('h', 4, 0.06094f);
  public static LetterPointsPair I => new('i', 4, 0.06966f);
  public static LetterPointsPair J => new('j', 5, 0.00153f);
  public static LetterPointsPair K => new('k', 6, 0.00772f);
  public static LetterPointsPair L => new('l', 6, 0.04025f);
  public static LetterPointsPair M => new('m', 6, 0.02406f);
  public static LetterPointsPair N => new('n', 4, 0.06749f);
  public static LetterPointsPair O => new('o', 3, 0.07507f);
  public static LetterPointsPair P => new('p', 6, 0.01929f);
  public static LetterPointsPair Q => new('q', 7, 0.00095f);
  public static LetterPointsPair R => new('r', 4, 0.05987f);
  public static LetterPointsPair S => new('s', 4, 0.06327f);
  public static LetterPointsPair T => new('t', 3, 0.09056f);
  public static LetterPointsPair U => new('u', 6, 0.02758f);
  public static LetterPointsPair V => new('v', 6, 0.00978f);
  public static LetterPointsPair W => new('w', 5, 0.02360f);
  public static LetterPointsPair X => new('x', 9, 0.00150f);
  public static LetterPointsPair Y => new('y', 7, 0.01974f);
  public static LetterPointsPair Z => new('z', 8, 0.00074f);

  public static LetterPointsPair GetRandom() {
    float choice = Random.value;
    LetterPointsPair current = A;

    foreach (LetterPointsPair lpp in All) {
      current = lpp;
      choice -= lpp.frequency;
      if (choice <= 0) break;
    }

    return current;
  }
}

public struct LetterPointsPair {
  public char value;
  public int points;
  public float frequency;

  public LetterPointsPair(char value, int points, float frequency) {
    this.value = value;
    this.points = points;
    this.frequency = frequency;
  }
}