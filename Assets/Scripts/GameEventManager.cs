using System;
using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum EventStatus
{
    Waiting,
    InProgress,
    Completed
}

/// <summary>
/// Manages the rat revolution event.
/// </summary>
public class GameEventManager : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _ratGeneratorData; // For ID
    [SerializeField] private TextMeshProUGUI _ratsOnStrikeUGUI; // For ID
    [SerializeField] private GameObject _gameEndCanvas;
    [SerializeField] private Button _becomeRatKingButton;
    [SerializeField] private TextMeshProUGUI _becomeRatKingCostUGUI;
    [SerializeField] private TextMeshProUGUI _eventInfoUGUI;

    [Header("Revolution settings")]
    [SerializeField] private BigDouble _minProfitToRevolution = 10000;
    [SerializeField] private BigDouble _minRatsToProgressRevolution = 100;

    public static GameEventManager Instance;

    private Generator _ratsRef;
    public EventStatus RatRevolutionStatus { get; private set; }
    public int RatRevolutionPhase { get; private set; }
    private string _ratsOnStrikeText;

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
        _becomeRatKingButton.onClick.AddListener(EndGame);
        ResourceManager.Instance.OnGameSoftReset += ResetEventData;
    }

    private void Update()
    {
        if (_ratGeneratorData == null) return;

        // Handle event checks
        if (RatRevolutionStatus == EventStatus.Waiting)
        {
            if (_ratsRef.GetCount() > 100 && ResourceManager.Instance.Profit > _minProfitToRevolution)
            {
                StartRatRevolutionEvent();
            }
        }
        else if (RatRevolutionStatus == EventStatus.InProgress)
        {
            _ratsOnStrikeUGUI.text = _ratsOnStrikeText + ResourceManager.Instance.GetRatsOnStrike();

            // Check if revolution phase will be changed
            if (_ratsRef.GetCount() > _minRatsToProgressRevolution && ResourceManager.Instance.Profit > _minProfitToRevolution * BigDouble.Pow(10, RatRevolutionPhase * 2))
            {
                switch (RatRevolutionPhase)
                {
                    case 1:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.04f);
                        _eventInfoUGUI.text = "More rats have joined the union.";
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 2:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.06f);
                        _eventInfoUGUI.text = "Hire cats and dogs to make rats work again.";
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 3:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.08f);
                        _eventInfoUGUI.text = "Rat union is gainining popularity.";
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 4:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.1f);
                        _ratsOnStrikeText = "Defective rats: ";
                        _eventInfoUGUI.text = "Rat usion is stealing your profits.";
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 5:
                        ResourceManager.Instance.SetQuitRate(0.12f);
                        _becomeRatKingButton.gameObject.SetActive(true);
                        _becomeRatKingCostUGUI.text = Utils.FormatNum(_minProfitToRevolution * BigDouble.Pow(10, RatRevolutionPhase));
                        break;
                }
            }
            if (RatRevolutionPhase == 5)
            {
                _becomeRatKingButton.interactable = ResourceManager.Instance.Profit >= _minProfitToRevolution * BigDouble.Pow(10, RatRevolutionPhase);
            }

        }
    }

    public void ResetEventData()
    {
        _gameEndCanvas.SetActive(false);
        _ratsRef = ResourceManager.Instance.GetGenerator(_ratGeneratorData.ID);
        RatRevolutionStatus = EventStatus.Waiting;
        _ratsOnStrikeUGUI.gameObject.SetActive(false);
        _becomeRatKingButton.gameObject.SetActive(false);
        ResourceManager.Instance.SetQuitRate(0f);
        _eventInfoUGUI.text = "You are rat.";
        Debug.Log("Event data reset.");
    }

    public void StartRatRevolutionEvent()
    {
        RatRevolutionStatus = EventStatus.InProgress;
        _ratsOnStrikeUGUI.gameObject.SetActive(true);
        RatRevolutionPhase = 1;
        _ratsOnStrikeText = "Rats on strike: ";
        _eventInfoUGUI.text = "Some rats have formed an union.";
        ResourceManager.Instance.SetQuitRate(0.005f);
        Debug.Log("Event rat revolution started");
    }

    public void EndRatRevolutionEvent()
    {
        RatRevolutionStatus = EventStatus.Completed;
        _ratsOnStrikeUGUI.gameObject.SetActive(false);
        ResourceManager.Instance.SetQuitRate(0f);
        Debug.Log("Event rat revolution completed");
        EndGame();
    }

    public void EndGame()
    {
        ScreenFade.Instance.StartScreenFaidOut(2, () =>
        {
            _gameEndCanvas.SetActive(true);
        });
    }

    public void SetEventData(EventStatus revolutionStatus, int revolutionPhase)
    {
        RatRevolutionStatus = revolutionStatus;

        switch (revolutionStatus)
        {
            case EventStatus.Waiting:
                break;
            case EventStatus.InProgress:
                _ratsOnStrikeUGUI.gameObject.SetActive(true);
                RatRevolutionPhase = revolutionPhase;

                switch (RatRevolutionPhase)
                {
                    case 1:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.04f);
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 2:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.06f);
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 3:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.08f);
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 4:
                        RatRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.1f);
                        _ratsOnStrikeText = "Defective rats: ";
                        Debug.Log("Entering revolution phase: " + RatRevolutionPhase);
                        break;
                    case 5:
                        ResourceManager.Instance.SetQuitRate(0.12f);
                        _becomeRatKingButton.gameObject.SetActive(true);
                        _becomeRatKingCostUGUI.text = Utils.FormatNum(_minProfitToRevolution * BigDouble.Pow(10, RatRevolutionPhase));
                        break;
                }
                break;
            case EventStatus.Completed:
                EndRatRevolutionEvent();
                break;
        }
    }
}
