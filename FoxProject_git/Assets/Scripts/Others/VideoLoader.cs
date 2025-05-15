using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoPath;
    //Ʈ���Ϸ� : "StreamingAssets\\Trailer1.mp4
    //Ÿ��Ʋ : StreamingAssets\\Title1.mp4
    void OnEnable()
    {
        string path = Path.Combine(Application.dataPath, videoPath);//Application.dataPath, "../StreamingAssets/cutscene.mp4"
        videoPlayer.url = path;
        Debug.Log(path);
        videoPlayer.Play();
    }
}
