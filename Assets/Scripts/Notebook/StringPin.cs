using UnityEngine;
using UnityEngine.EventSystems;

public class StringPin : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{

    [SerializeField] private RectTransform line;
    private StringPin connectedPin;

    private void Awake()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.gameObject.SetActive(true);
        UpdateLine(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateLine(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.gameObject.SetActive(false);
    }

    void UpdateLine(Vector2 end)
    {
        Vector2 start = ((RectTransform)transform).position;

        Vector2 dir = end - start;
        float distance = dir.magnitude;

        line.position = start;
        line.sizeDelta = new Vector2(distance, line.sizeDelta.y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.rotation = Quaternion.Euler(0, 0, angle);
    }

    // If it has a connected pin, returns that pin; else, returns self
    public StringPin GetConnected()
    {
        return connectedPin == null ? this : connectedPin;
    }
}