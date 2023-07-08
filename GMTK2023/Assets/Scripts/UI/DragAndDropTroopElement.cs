using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDropTroopElement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public UnityEvent grabEvent;
    public UnityEvent<bool> dropEvent;

    private RectTransform rectTransform;
    private LayoutElement layoutElement;
    public Text countText, costText;

    private TroopPurchaseData troopData;
    private int troopCount;

    private bool canDrag;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(TroopPurchaseData data, int count)
    {
        troopData = data;
        troopCount = count;
        countText.text = troopCount.ToString();
        costText.text = PurchaseCost().ToString();
        layoutElement = GetComponent<LayoutElement>();
    }

    private int PurchaseCost()
    {
        return troopData.costPerUnit * troopCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!EconomyManager.Instance.CanAfford(PurchaseCost()))
        {
            //TODO: Tell the user they are broke
            Debug.Log("You do not have: " + PurchaseCost());
            canDrag = false;
            return;
        }
        

        int siblingIndex = transform.GetSiblingIndex();

        // replace the dragged element because it will be destroyed on drop
        DragAndDropTroopElement copiedElement = Instantiate(gameObject, transform.parent).GetComponent<DragAndDropTroopElement>();
        copiedElement.Initialize(troopData, troopCount);
        copiedElement.transform.SetSiblingIndex(siblingIndex);

        rectTransform.SetParent(GetComponentInParent<Canvas>().transform);
        layoutElement.ignoreLayout = true;

        EconomyManager.Instance.DecreaseMoney(PurchaseCost());

        canDrag = true;

        if (grabEvent != null)
            grabEvent.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!canDrag)
        {
            return;
        }
        
        rectTransform.anchoredPosition += eventData.delta * UIManager.Instance.CanvasScale;    // without canvas scale, item will not follow mouse properly
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!canDrag)
        {
            return;
        }

        layoutElement.ignoreLayout = true;

        bool success = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            DeploymentZone dz = hit.collider.GetComponent<DeploymentZone>();
            if (dz)
            {
                dz.AddToDeploymentQueue(troopData, troopCount);
                success = true;
            }
        }

        if (dropEvent != null)
            dropEvent.Invoke(success);

        // refund money since troops not deployed on valid area
        if (!success)
        {
            EconomyManager.Instance.IncreaseMoney(PurchaseCost());
        }

        

        Destroy(gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
