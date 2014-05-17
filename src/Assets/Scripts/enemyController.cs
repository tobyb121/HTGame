using UnityEngine;
using System.Collections;
using BloodyMuns;

public class enemyController {

    public Character character;
    public GameObject enemy;

    public enemyController(Character c)
    {
        character = c;
    }

    public void updateCharacter(Character c){
        character.Position = c.Position;
        character.Rotation = c.Rotation;
        character.Velocity = c.Velocity;
    }

}
