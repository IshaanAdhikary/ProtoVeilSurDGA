using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Notebook item representing a photo.
/// </summary>
public class PhotoNote : NotebookItem
{
    [SerializeField] private List<string> subjectIds = new();

    public IReadOnlyList<string> SubjectIds() {
        return subjectIds;
    }

    public bool HasSubject(string subjectId) {
        return subjectIds.Contains(subjectId);
    }

    // TODO: add photo-specific code like displaying an image
    // and validation
}
