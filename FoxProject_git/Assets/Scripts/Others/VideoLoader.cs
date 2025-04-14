using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        string path = Path.Combine(Application.dataPath, "StreamingAssets\\Trailer1.mp4");//Application.dataPath, "../StreamingAssets/cutscene.mp4"
        videoPlayer.url = path;
        Debug.Log(path);
        videoPlayer.Play();
    }
}
