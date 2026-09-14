using UnityEngine;

public class Respawn : MonoBehaviour
{
    public void VoltarCheckpoint()
    {
        float x = PlayerPrefs.GetFloat("CheckpointX");
        float y = PlayerPrefs.GetFloat("CheckpointY");

        transform.position = new Vector2(x, y);
    }
}