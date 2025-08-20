using System;
using System.Collections;
using BreakInfinity;
using TMPro;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance;
    [SerializeField] private GeneratorDataSO _ratGeneratorData; // For ID
    [SerializeField] private TextMeshProUGUI _ratsOnStrikeUGUI; // For ID

    private Generator _ratGeneratorRef;
    public enum EventStatus
    {
        Waiting,
        InProgress,
        Completed
    }
    private EventStatus _ratRevolutionStatus;
    private BigDouble _ratsOnStrike;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ResetEventData();
    }

    private void Update()
    {
        if (_ratGeneratorData == null) return;

        // Handle event checks
        if (_ratRevolutionStatus == EventStatus.Waiting)
        {
            if (_ratGeneratorRef.GetCount() > 2500)
            {
                StartRatRevolutionEvent();
            }
        }
    }

    public void ResetEventData()
    {
        _ratGeneratorRef = ResourceManager.Instance.GetGenerator(_ratGeneratorData.ID);
        _ratRevolutionStatus = EventStatus.Waiting;
        _ratsOnStrikeUGUI.gameObject.SetActive(false);
        Debug.Log("Event data reset.");
    }


    public void StartRatRevolutionEvent()
    {
        _ratRevolutionStatus = EventStatus.InProgress;
        _ratsOnStrikeUGUI.gameObject.SetActive(true);
        StartCoroutine(RatRevolutionEvent());
        Debug.Log("Event started: rat revolution");
    }

    public IEnumerator RatRevolutionEvent()
    {
        float quitRate = 0.02f;
        WaitForSeconds wait = new WaitForSeconds(1);

        // Keep updating the revolution event while it's in progress
        while (_ratRevolutionStatus == EventStatus.InProgress)
        {
            yield return wait;
            BigDouble ratsEnteringStrike = (_ratGeneratorRef.GetCount() * quitRate).Round();
            _ratsOnStrike += ratsEnteringStrike;
            _ratsOnStrikeUGUI.text = "Rats on strike: " + Utils.FormatNum(_ratsOnStrike);
            _ratGeneratorRef.Remove(ratsEnteringStrike);
        }
    }
}
