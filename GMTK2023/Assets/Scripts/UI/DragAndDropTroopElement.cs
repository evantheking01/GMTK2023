using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragAndDropTroopElement : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent grabEvent;
    public UnityEvent<bool> dropEvent;

    private RectTransform rectTransform;
    private LayoutElement layoutElement;
    public Text countText, costText, nameText, descriptionText;
    public Image thumbnailImage;
    public GameObject hoverObject;
    private Animator animator;

    private TroopPurchaseData troopData;
    private int troopCount;

    private bool canDrag;

    private bool canUse=true;

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

        thumbnailImage.sprite = data.thumbnail;
        countText.text = "x" + troopCount.ToString();
        costText.text = PurchaseCost().ToString("C");
        nameText.text = data.groupName;
        descriptionText.text = data.description;
        hoverObject.SetActive(false);

        layoutElement = GetComponent<LayoutElement>();
        animator = GetComponent<Animator>();
        if (count < 0)
        {
            // use this to buy all you can
        }
    }

    private int PurchaseCost()
    {
        return troopData.costPerUnit * troopCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!EconomyManager.Instance.CanAfford(PurchaseCost()))
        {
            animator.SetTrigger("Shake");

            Debug.Log("You do not have: " + PurchaseCost());
            canDrag = false;
            return;
        }

        if (!canUse) return;
        hoverObject.SetActive(false);
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
        if(!canDrag || !canUse)
        {
            return;
        }
        
        rectTransform.anchoredPosition += eventData.delta * UIManager.Instance.CanvasScale;    // without canvas scale, item will not follow mouse properly
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(!canDrag || !canUse)
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
        else
        {
            GameManager.Instance.UpdateLifetimeEconomy(PurchaseCost());
        }

        Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverObject.SetActive(true && !canDrag);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverObject.SetActive(false);
    }

    public void SetCanUse(bool val)
    {
        canUse = val;
        GetComponentInChildren<Button>().interactable = val;
    }
}
