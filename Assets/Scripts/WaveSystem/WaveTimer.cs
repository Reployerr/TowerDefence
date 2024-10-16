using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveTimer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WaveSpawner waveSpawner;  // Ссылка на скрипт спавна волн
    [SerializeField] private Image timerCircle;// UI элемент таймера (круг)
    [SerializeField] private Image Timer; 
    [SerializeField] private Button startWaveButton;   // Кнопка для мгновенного старта волны
    [SerializeField] private TMP_Text timerText;       // Текст для отображения оставшегося времени

    [SerializeField] public bool isFirstWave;
    private float _timeBetweenWaves;
    private float _currentTime;

    private void Start()
    {
        if(waveSpawner._currentWave == 1)
		{
            isFirstWave = true;
        }
		else
		{
            isFirstWave = false;
        }
        // Получаем время между волнами из скрипта WaveSpawner
        _timeBetweenWaves = waveSpawner.GetTimeBetweenWaves();
        // Назначаем кнопку для мгновенного запуска волны
        startWaveButton.onClick.AddListener(StartNextWaveNow);
        // Скрываем таймер в начале
        HideTimer();
    }

    private void Update()
    {
        //Debug.Log("isFirstWave of Wave Timer = " + isFirstWave);
        // Уменьшаем текущее время, если таймер активен
        if (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            UpdateTimerUI();

            // Если время закончилось, скрываем таймер
            if (_currentTime <= 0)
            {
                HideTimer();
            }
        }
    }

    // Запускаем таймер новой волны
    public void StartNewWaveTimer()
    {
        _currentTime = _timeBetweenWaves;  // Устанавливаем начальное время таймера
        ShowTimer();                       // Показываем UI таймера
        UpdateTimerUI();                   // Обновляем UI
    }

    // Обновляем UI таймера (круг и текст)
    private void UpdateTimerUI()
    {
        float fillAmount = _currentTime / _timeBetweenWaves;  // Рассчитываем заполнение круга
        timerCircle.fillAmount = fillAmount;                  // Обновляем круг
        timerText.text = Mathf.Ceil(_currentTime).ToString(); // Обновляем текст времени
    }

    // Принудительный старт следующей волны
    private void StartNextWaveNow()
    {
		if (isFirstWave == true)
		{
            isFirstWave = false;
            waveSpawner.ForceStartNextWave(true);
            HideTimer();
        }
        else if (_currentTime > 0)
        {
            _currentTime = 0;                  // Сбрасываем таймер
            waveSpawner.ForceStartNextWave();  // Принудительно запускаем волну
            HideTimer();                       // Скрываем таймер после старта волны
        }
    }

    // Показываем таймер
    public void ShowTimer()
    {
        Timer.gameObject.SetActive(true);
        /*timerCircle.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        startWaveButton.gameObject.SetActive(true);*/
    }

    // Скрываем таймер
    private void HideTimer()
    {
        Timer.gameObject.SetActive(false);
        /*timerCircle.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        startWaveButton.gameObject.SetActive(false);*/
    }
}
