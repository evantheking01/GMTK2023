using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDropTroopElement : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public UnityEvent<int> moneyChangeEvent;

    private RectTransform rectTransform;
    private LayoutElement layoutElement;
    public Text countText, costText;

    private float canvasScale = 1;

    private TroopPurchaseData troopData;
    private int troopCount;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();
        // Assuming match will be either 0 or 1. Otherwise this is some more math
        if (canvasScaler.matchWidthOrHeight > 0.5f)
            canvasScale = canvasScaler.referenceResolution.y / Screen.height;
        else
            canvasScale = canvasScaler.referenceResolution.x / Screen.width;

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
        int siblingIndex = transform.GetSiblingIndex();

        // replace the dragged element because it will be destroyed on drop
        DragAndDropTroopElement copiedElement = Instantiate(gameObject, transform.parent).GetComponent<DragAndDropTroopElement>();
        copiedElement.Initialize(troopData, troopCount);
        copiedElement.transform.SetSiblingIndex(siblingIndex);

        rectTransform.parent = GetComponentInParent<Canvas>().transform;
        layoutElement.ignoreLayout = true;

        // TODO: change this to informing the economy manager instance because tracking this event may be tedious
        if (moneyChangeEvent != null)
            moneyChangeEvent.Invoke(PurchaseCost());
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta * canvasScale;    // without canvas scale, item will not follow mouse properly
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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

        // refund money since troops not deployed on valid area
        if (!success)
        {
            if (moneyChangeEvent != null)
                moneyChangeEvent.Invoke(-PurchaseCost());
        }

        Destroy(gameObject);

    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
