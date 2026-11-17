
using UnityEngine;
using TMPro;

public class NoteInteraction : MonoBehaviour
{
    [TextArea(2, 5)]
    public string NoteText = "Hai trovato qualcosa!";

    public GameObject DialoguePanel;
    public TextMeshProUGUI DialogueText;

    private Camera MainCamera;
    private bool Collected = false;

    void Start()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {
        if (Collected)
            return;

        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            CheckTouch(Input.GetTouch(0).position);
        }

#if UNITY_EDITOR //execute if I'm in the editor
        if (Input.GetMouseButtonDown(0))
        {
            CheckTouch(Input.mousePosition);
        }
#endif
    }

    void CheckTouch(Vector2 touchPosition)
    {
        Ray ray = MainCamera.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                ShowNote();
            }
        }
    }

    void ShowNote()
    {
        Collected = true;

        DialoguePanel.SetActive(true);
        DialogueText.text = NoteText;

        gameObject.SetActive(false);
    }
}
