using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveSpawner waveSpawner;  // ������ �� ������ ������ ����
    [SerializeField] private Image timerCircle;// UI ������� ������� (����)
    [SerializeField] private Image Timer; 
    [SerializeField] private Button startWaveButton;   // ������ ��� ����������� ������ �����
    [SerializeField] private TMP_Text timerText;       // ����� ��� ����������� ����������� �������

    private float _timeBetweenWaves;
    private float _currentTime;

    private void Start()
    {
        // �������� ����� ����� ������� �� ������� WaveSpawner
        _timeBetweenWaves = waveSpawner.GetTimeBetweenWaves();
        // ��������� ������ ��� ����������� ������� �����
        startWaveButton.onClick.AddListener(StartNextWaveNow);
        // �������� ������ � ������
        HideTimer();
    }

    private void Update()
    {
        // ��������� ������� �����, ���� ������ �������
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimerUI();

            // ���� ����� �����������, �������� ������
            if (_currentTime <= 0)
            {
                HideTimer();
            }
        }
    }

    // ��������� ������ ����� �����
    public void StartNewWaveTimer()
    {
        _currentTime = _timeBetweenWaves;  // ������������� ��������� ����� �������
        ShowTimer();                       // ���������� UI �������
        UpdateTimerUI();                   // ��������� UI
    }

    // ��������� UI ������� (���� � �����)
    private void UpdateTimerUI()
    {
        float fillAmount = _currentTime / _timeBetweenWaves;  // ������������ ���������� �����
        timerCircle.fillAmount = fillAmount;                  // ��������� ����
        timerText.text = Mathf.Ceil(_currentTime).ToString(); // ��������� ����� �������
    }

    // �������������� ����� ��������� �����
    private void StartNextWaveNow()
    {
        if (_currentTime > 0)
        {
            _currentTime = 0;                  // ���������� ������
            waveSpawner.ForceStartNextWave();  // ������������� ��������� �����
            HideTimer();                       // �������� ������ ����� ������ �����
        }
    }

    // ���������� ������
    private void ShowTimer()
    {
        Timer.gameObject.SetActive(true);
        /*timerCircle.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        startWaveButton.gameObject.SetActive(true);*/
    }

    // �������� ������
    private void HideTimer()
    {
        Timer.gameObject.SetActive(false);
        /*timerCircle.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        startWaveButton.gameObject.SetActive(false);*/
    }
}
