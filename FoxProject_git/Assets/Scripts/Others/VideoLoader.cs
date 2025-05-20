using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoPath;
    //트레일러 : "StreamingAssets\\Trailer1.mp4
    //타이틀 : StreamingAssets\\Title1.mp4
    void OnEnable()
    {
        string path = Path.Combine(Application.dataPath, videoPath);//Application.dataPath, "../StreamingAssets/cutscene.mp4"
        videoPlayer.url = path;
        Debug.Log(path);
        videoPlayer.Play();
    }
}
