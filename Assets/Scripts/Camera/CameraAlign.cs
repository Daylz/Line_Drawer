using UnityEngine;
using System.Collections;

public class CameraAlign : MonoBehaviour
{
    public static CameraAlign Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void AlignToBoard()
    {
        Camera camera = Camera.main;

        float x = (camera.orthographicSize / 2) - (BoardCreator.Instance.TileSize * (BoardData.Instance.MaxWidth - BoardData.Instance.Width)) / 2;
        float y = (-camera.orthographicSize / 2) + (BoardCreator.Instance.TileSize * (BoardData.Instance.MaxHeight - BoardData.Instance.Height)) / 2;
        this.transform.position = new Vector3(x, y, this.transform.position.z);
    }
}
