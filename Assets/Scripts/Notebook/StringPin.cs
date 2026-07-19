using UnityEngine;
using UnityEngine.EventSystems;

public class StringPin : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private RectTransform canvas;
    [SerializeField] private RectTransform temporaryLine;
    [SerializeField] private RectTransform connectionsParent;
    [SerializeField] private GameObject linePrefab;

    private RectTransform rect;
    private Vector2 startPoint;

    public static StringPin HoveredPin;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (temporaryLine != null)
            temporaryLine.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoveredPin = this;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoveredPin == this)
            HoveredPin = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPoint = rect.anchoredPosition;

        temporaryLine.gameObject.SetActive(true);

        UpdateLine(temporaryLine, startPoint, GetMousePosition(eventData));
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateLine(temporaryLine, startPoint, GetMousePosition(eventData));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        temporaryLine.gameObject.SetActive(false);

        if (HoveredPin == null)
            return;

        if (HoveredPin == this)
            return;

        CreatePermanentConnection(HoveredPin);
    }

    private Vector2 GetMousePosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 point);

        return point;
    }

    private void CreatePermanentConnection(StringPin other)
    {
        RectTransform line = Instantiate(linePrefab, connectionsParent)
            .GetComponent<RectTransform>();

        UpdateLine(line, rect.anchoredPosition, other.rect.anchoredPosition);
    }

    private void UpdateLine(RectTransform line, Vector2 start, Vector2 end)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;

        line.gameObject.SetActive(true);

        // Position at the start point
        line.anchoredPosition = start;

        // Stretch to the correct length
        line.sizeDelta = new Vector2(distance, line.sizeDelta.y);

        // Rotate toward the end point
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        line.localRotation = Quaternion.Euler(0, 0, angle);

        // Make sure the pivot is on the left (0, 0.5)
        // Otherwise the line won't start exactly at the pin.
    }
}