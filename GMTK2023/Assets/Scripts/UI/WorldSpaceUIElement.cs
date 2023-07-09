using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class WorldSpaceUIElement : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = new Vector2();
    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        
        Vector2 screenPos = Camera.main.WorldToScreenPoint(target.position);
        transform.position = screenPos + offset;
    }

    public void SetText(string str)
    {
        if (text != null)
        {
            text.text = str;
        }
    }
}
