using UnityEngine;

[System.Serializable]
public class PathAction
{
    public NPCState state;
    public Direction direction;
    public float durationOrDistance;
    public bool useStartPos;
    public Vector2 startPos;
    public Vector2 GetVector()
    {
        var magnitude = 0.3f;
        if (state == NPCState.Walking)
            magnitude = Mathf.Floor(durationOrDistance);
        if (direction == Direction.Right)
            return new Vector2(magnitude, 0);
        else if (direction == Direction.Left)
            return new Vector2(magnitude * -1, 0);
        else if (direction == Direction.Up)
            return new Vector2(0, magnitude);
        else
            return new Vector2(0, magnitude * -1);
    }
}

public enum Direction { Up, Down, Left, Right }