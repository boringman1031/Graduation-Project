using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameData
{
    int Health { get; set; }
    int Defence { get; set; }
    float Speed { get; set; }
    int Power { get; set; }
    int Attack { get; set; }
}
