using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MakeItButton : MonoBehaviour
{
    private GameObject button;
    public GameObject[] walls;
    public GameObject[] indicatorArrows;
    private int m_currentPair;
    // Start is called before the first frame update
    void Start()
    {
        button = this.gameObject;

        // there must be the same number of obstacle walls as there are indicator arrows
        Debug.Assert(walls.Length == indicatorArrows.Length);

        // defualt to the first pair
        m_currentPair = 0;
        updatePairs();
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(ray, out hit) && (hit.collider.gameObject == gameObject))
            {
                m_currentPair = (m_currentPair + 1) % walls.Length;
                updatePairs();
            }
        }
    }

    private void updatePairs()
    {
        if(m_currentPair < walls.Length && m_currentPair >= 0)
        {
            for (int i = 0; i < walls.Length; i++)
            {
                if(i == m_currentPair)
                {
                    // activate the selected path
                    walls[i].SetActive(false);
                    indicatorArrows[i].SetActive(true);
                }
                else
                {
                    // and then disable the rest
                    walls[i].SetActive(true);
                    indicatorArrows[i].SetActive(false);
                }
                
            }
        }
    }
}
