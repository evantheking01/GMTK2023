using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class WaveEndScreen : MonoBehaviour
{
    [SerializeField] Text waveText;
    [SerializeField] NumberCounter troopsDeployedText;
    [SerializeField] NumberCounter moneySpentText;
    [SerializeField] NumberCounter moneyEarnedText;
    [SerializeField] NumberCounter balanceText;
    [SerializeField] Button nextButton;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        nextButton.onClick.AddListener(Close);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWaveText(int wave, bool levelComplete)
    {
        if (!levelComplete)
        {
            waveText.text = $"Wave {wave} Over";
            nextButton.GetComponentInChildren<Text>().text = "Next Wave";
            nextButton.onClick.RemoveAllListeners();
            LevelManager lm = GameObject.FindObjectOfType<LevelManager>();
            nextButton.onClick.AddListener(Close);
            nextButton.onClick.AddListener(lm.StartWave);

        }
        else
        {
            waveText.text = $"Level complete in {wave} waves";
            nextButton.GetComponentInChildren<Text>().text = "Next Level";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(Close);
            nextButton.onClick.AddListener(GameManager.Instance.LoadNextLevel);
        }
    }

    public void Populate(int totalTroops, int moneySpent, int moneyEarned, int balance)
    {
        troopsDeployedText.QuickText(totalTroops.ToString());
        moneySpentText.QuickText(moneySpent.ToString("C"));
        moneyEarnedText.QuickText(moneyEarned.ToString("C"));
        balanceText.QuickText(balance.ToString("C"));
        Launch();
        StartCoroutine(ShowEndScreen(totalTroops, moneySpent, moneyEarned, balance));
    }

    private IEnumerator ShowEndScreen(int totalTroops, int moneySpent, int moneyEarned, int balance)
    {
        yield return new WaitForSeconds(0.5f);

        troopsDeployedText.SetText(0, totalTroops, true);
        yield return new WaitForSeconds(0.1f);

        moneySpentText.SetText(0, moneySpent, true, 1f, "C"); 
        yield return new WaitForSeconds(0.1f);

        moneyEarnedText.SetText(0, moneyEarned, true, 1f, "C"); 
        yield return new WaitForSeconds(0.1f);

        balanceText.SetText(0, balance, true, 1f, "C"); 
        yield return new WaitForSeconds(0.1f);

    }

    public void Launch()
    {
        Debug.Log("launch");
        animator = GetComponent<Animator>();
        animator.SetTrigger("Launch");
    }

    public void Close()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Close");
    }

}
