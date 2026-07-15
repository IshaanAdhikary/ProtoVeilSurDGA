using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform bounds;
    private RectTransform rect;
    private Vector2 mousePosition;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        // Empty for now
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