using BreakInfinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEventManager : MonoBehaviour
{
    [SerializeField] private GeneratorDataSO _ratGeneratorData; // For ID
    [SerializeField] private TextMeshProUGUI _ratsOnStrikeUGUI; // For ID
    [SerializeField] private GameObject _gameEndCanvas;
    [SerializeField] private Button _becomeRatKingButton;
    [SerializeField] private TextMeshProUGUI _becomeRatKingCostUGUI;

    [Header("Revolution settings")]
    [SerializeField] private BigDouble _minProfitToRevolution = 10000;
    [SerializeField] private BigDouble _minRatsToProgressRevolution = 100;

    public static GameEventManager Instance;

    private Generator _ratsRef;
    public enum EventStatus
    {
        Waiting,
        InProgress,
        Completed
    }
    private EventStatus _ratRevolutionStatus;
    private int _ratRevolutionPhase;
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
    }

    private void Update()
    {
        if (_ratGeneratorData == null) return;

        // Handle event checks
        if (_ratRevolutionStatus == EventStatus.Waiting)
        {
            if (_ratsRef.GetCount() > 100 && ResourceManager.Instance.Profit > _minProfitToRevolution)
            {
                StartRatRevolutionEvent();
            }
        }
        else if (_ratRevolutionStatus == EventStatus.InProgress)
        {
            _ratsOnStrikeUGUI.text = _ratsOnStrikeText + ResourceManager.Instance.GetRatsOnStrike();

            // Check if revolution phase will be changed
            if (_ratsRef.GetCount() > _minRatsToProgressRevolution && ResourceManager.Instance.Profit > _minProfitToRevolution * BigDouble.Pow(10, _ratRevolutionPhase * 2))
            {
                switch (_ratRevolutionPhase)
                {
                    case 1:
                        _ratRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.04f);
                        Debug.Log("Entering revolution phase: " + _ratRevolutionPhase);
                        break;
                    case 2:
                        _ratRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.06f);
                        Debug.Log("Entering revolution phase: " + _ratRevolutionPhase);
                        break;
                    case 3:
                        _ratRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.08f);
                        Debug.Log("Entering revolution phase: " + _ratRevolutionPhase);
                        break;
                    case 4:
                        _ratRevolutionPhase++;
                        ResourceManager.Instance.SetQuitRate(0.1f);
                        _ratsOnStrikeText = "Defective rats: ";
                        Debug.Log("Entering revolution phase: " + _ratRevolutionPhase);
                        break;
                    case 5:
                        ResourceManager.Instance.SetQuitRate(0.12f);
                        _becomeRatKingButton.gameObject.SetActive(true);
                        _becomeRatKingCostUGUI.text = Utils.FormatNum(_minProfitToRevolution * BigDouble.Pow(10, _ratRevolutionPhase));
                        break;
                }
            }
            if (_ratRevolutionPhase == 5)
            {
                _becomeRatKingButton.interactable = ResourceManager.Instance.Profit >= _minProfitToRevolution * BigDouble.Pow(10, _ratRevolutionPhase);
            }

        }
    }

    public void ResetEventData()
    {
        _gameEndCanvas.SetActive(false);
        _ratsRef = ResourceManager.Instance.GetGenerator(_ratGeneratorData.ID);
        _ratRevolutionStatus = EventStatus.Waiting;
        _ratsOnStrikeUGUI.gameObject.SetActive(false);
        _becomeRatKingButton.gameObject.SetActive(false);
        ResourceManager.Instance.SetQuitRate(0f);
        Debug.Log("Event data reset.");
    }

    public void StartRatRevolutionEvent()
    {
        _ratRevolutionStatus = EventStatus.InProgress;
        _ratsOnStrikeUGUI.gameObject.SetActive(true);
        _ratRevolutionPhase = 1;
        _ratsOnStrikeText = "Rats on strike: ";
        ResourceManager.Instance.SetQuitRate(0.005f);
        Debug.Log("Event rat revolution started");
    }

    public void EndRatRevolutionEvent()
    {
        _ratRevolutionStatus = EventStatus.Completed;
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
}
