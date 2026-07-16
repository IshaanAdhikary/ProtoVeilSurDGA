using UnityEngine;

/// <summary>
/// Notebook item representing a photo.
/// </summary>
public class PhotoNote : NotebookItem
{
    [SerializeField] private string subjectId;

    public string SubjectId() {
        return subjectId;
    }

    // TODO: add photo-specific code like displaying an image
    // and validation
}
