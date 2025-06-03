using UnityEngine.UI;
using System.Linq;
using UnityEngine;
using TMPro;

public class BrushController : MonoBehaviour
{
    [SerializeField] private Camera paintCamera;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject errorMessage;

    [Header("Model")]
    [SerializeField] private ColorPaletteSO palette;

    [Header("View")]
    [SerializeField] private Transform paletteParent;
    [SerializeField] private Button paleteButtonPrefab;

    [SerializeField] private Sprite brushSprite;
    [SerializeField] private BrushView brushView;
    
    [SerializeField] private Vector2Int fieldSize = new Vector2Int(15, 15);
    private BrushModel _brushModel;

    public void SetBrushAsEraser()
    {
        SetBrushColor(palette.EraserColor);
    }

    private void Awake()
    {
        InitializeBrush();
        FillPlayableArea();

        inputField.onSubmit.AddListener(ParseNumberOfColors);
        submitButton.onClick.AddListener(() => ParseNumberOfColors(inputField.text));
    }

    public void InitializePaletteUI(int numberOfColors)
    {
        ClearOldButtons();

        var availableColor = palette.Colors.ToList();

        if (numberOfColors < palette.Colors.Length)
        {
            availableColor.Shuffle();
        }

        for (int i = 0; i < numberOfColors; i++)
        {
            var button = Instantiate(paleteButtonPrefab, paletteParent);
            button.image.color = availableColor[i];

            // For some unknown reason 'i' for every button = 14 if set directly
            int index = i;

            button.onClick.AddListener(() => SetBrushColor(availableColor[index]));
        }
    }

    private void ParseNumberOfColors(string input)
    {
        if (int.TryParse(input, out var numberOfColors))
        {
            numberOfColors = Mathf.Clamp(numberOfColors, 1, palette.Colors.Length);
            InitializePaletteUI(numberOfColors);
            window.SetActive(false);
        }
        else
        {
            Debug.Log($"Incorrect value");
            errorMessage.SetActive(true);
        }
    }

    private void ClearOldButtons()
    {
        foreach (Transform child in paletteParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetBrushColor(Color color)
    {
        _brushModel.SetBrushColor(color);
    }

    private void InitializeBrush()
    {
        _brushModel = new BrushModel(brushSprite, palette.EraserColor);
    }

    private void FillPlayableArea()
    {
        for (int x = 0; x < fieldSize.x; x++)
        {
            for (int y = 0; y < fieldSize.y; y++)
            {
                brushView.PlaceTile(new Vector3Int(x, y, 0), _brushModel.CurrentBrush);
            }
        }
    }

    private void Update()
    {
        var cursorPosition = paintCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = brushView.paintTilemap.WorldToCell(cursorPosition);
        cellPosition.z = 0;

        if (Input.GetMouseButtonDown(0) && IsInBounds(cellPosition))
        {
            brushView.PaintCell(cellPosition, _brushModel.CurrentBrush);
        }
    }

    private bool IsInBounds(Vector3Int position) => position.x >= 0 && position.x < fieldSize.x && position.y >= 0 && position.y < fieldSize.y;
}
