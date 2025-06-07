using UnityEngine;
using System.IO;
using System.Collections;

public class TilemapExporter : MonoBehaviour
{
    #region Some file saving stuff
    private const string DIRECTORY = "MyWorks";
    private const string FILENAME = "image.png";

    private string _directoryPath =>
#if UNITY_EDITOR
        Path.Combine(Application.dataPath, DIRECTORY);
#else
            Path.Combine(Directory.GetParent(Application.dataPath).ToString(), DIRECTORY);
#endif

    private string _filePath => Path.Combine(_directoryPath, FILENAME);
    #endregion

    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private Camera renderCamera;
    [SerializeField] private int width = 512;
    [SerializeField] private int height = 512;
    private Coroutine _messageRoutine;

    private void Awake()
    {
        messageGroup.alpha = 0f;
    }

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

        Debug.Log($"Picture saved to {_filePath}");

        if (_messageRoutine != null)
        {
            StopCoroutine(_messageRoutine);
        }
        _messageRoutine = StartCoroutine(ShowSavedMessage());
    }

    private IEnumerator ShowSavedMessage()
    {
        messageGroup.alpha = 1.0f;

        yield return new WaitForSeconds(1f);

        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            messageGroup.alpha = Mathf.Lerp(1f, 0f, timer / 0.5f);
            yield return null;
        }
        messageGroup.alpha = 0f;

        yield return null;
    }
}
