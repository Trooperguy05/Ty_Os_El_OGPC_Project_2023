using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNode : MonoBehaviour
{
    public Vector2 coordinates;
    public List<GameObject> adjacentNodes = new List<GameObject>();

    // method that sets the coordinates for the node
    public void setCoords(float x, float y) {
        coordinates = new Vector2(x, y);
    }
}
