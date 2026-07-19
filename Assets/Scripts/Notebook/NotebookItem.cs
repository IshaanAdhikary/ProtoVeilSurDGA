using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Base class for draggable notebook items. Handles drag input and clamps the
/// item's position within assigned bounds. Concrete note types (e.g.
/// TextNote, QuestionNote) inherit from this to add their own content.
/// </summary>
public abstract class NotebookItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform bounds;
    [SerializeField] private StringPin pin;
    private RectTransform rect;
    private Vector2 mousePosition;

    public StringPin Pin => pin;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        mousePosition = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData e)
    {
        mousePosition += e.delta;
        rect.anchoredPosition = ClampToBounds(mousePosition);
    }

    public void OnEndDrag(PointerEventData e)
    {
        // Empty for now
    }

    private Vector2 ClampToBounds(Vector2 toClamp)
    {
        // Half-sizes, since anchoredPosition is measured from the pivot
        Vector2 halfSize = rect.rect.size * 0.5f;
        Vector2 boundHalfSize = bounds.rect.size * 0.5f;

        float minX = -boundHalfSize.x + halfSize.x;
        float maxX = boundHalfSize.x - halfSize.x;
        float minY = -boundHalfSize.y + halfSize.y;
        float maxY = boundHalfSize.y - halfSize.y;

        toClamp.x = Mathf.Clamp(toClamp.x, minX, maxX);
        toClamp.y = Mathf.Clamp(toClamp.y, minY, maxY);

        return toClamp;
    }
}