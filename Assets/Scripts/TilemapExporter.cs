using UnityEngine;
using System.IO;

public class TilemapExporter : MonoBehaviour
{
    private const string DIRECTORY = "MyWorks";
    private const string FILENAME = "image.png";

    private string _directoryPath =>
#if UNITY_EDITOR
        Path.Combine(Application.dataPath, DIRECTORY);
#else
            Path.Combine(Directory.GetParent(Application.dataPath).ToString(), DIRECTORY);
#endif

    private string _filePath => Path.Combine(_directoryPath, FILENAME);

    [SerializeField] private Camera renderCamera;
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;

    [ContextMenu("Save")]
    public void Save()
    {
        if (!Directory.Exists(_directoryPath))
            Directory.CreateDirectory(_directoryPath);

        RenderTexture rt = new RenderTexture(width, height, 24);
        renderCamera.targetTexture = rt;
        renderCamera.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        renderCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        File.WriteAllBytes(_filePath, bytes);
    }
}
