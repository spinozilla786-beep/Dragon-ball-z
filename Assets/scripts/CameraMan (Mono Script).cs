using UnityEngine;

public class CameraMan : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;
    private bool hasOffset = false;

    void Start()
    {
        InitializeCamera();
    }

    void LateUpdate()
    {
        // Try finding player again if it spawned late
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            if (!hasOffset)
            {
                offset = transform.position - player.transform.position;
                hasOffset = true;
            }

            transform.position = player.transform.position + offset;
        }
    }

    private void InitializeCamera()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            offset = transform.position - player.transform.position;
            hasOffset = true;
        }
    }
}