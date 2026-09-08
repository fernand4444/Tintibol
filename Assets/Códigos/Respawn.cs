using UnityEngine;

public class Respawn : MonoBehaviour
{
    public void VoltarCheckpoint()
    {
        if (PlayerPrefs.GetInt("TemCheckpoint", 0) == 1)
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}