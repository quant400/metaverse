using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector2 index;
    public int internalIndex;

    public void setIndex()
    {
        index = new Vector2(transform.position.x / 100, transform.position.z / 100);
    }
}
