using TMPro;
using UniRx;
using UnityEngine;

public class HudPresenter : MonoBehaviour
{
    //TODO: Flashing Time Text for 30 and 10 seconds left
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text sheepText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalSheepText;
    [SerializeField] private TMP_Text finalScoreText;

    // Start is called before the first frame update
    void Start()
    {
        //Update in game hud
        GameStore.Subject.Select(state => state.timeLeft).DistinctUntilChanged()
            .Subscribe(timeLeft => timeText.text = "Time Left: " + timeLeft.ToString())
            .AddTo(this);
        GameStore.Subject.Select(state => state.score).DistinctUntilChanged()
            .Select(score => score.ToString("#,#"))
            .Select(score => "Score: " + score)
            .Subscribe(timeLeft => highScoreText.text = timeLeft.ToString()).AddTo(this);
        GameStore.Subject.Select(state => state.sheepCount).DistinctUntilChanged()
            .Select(sheepCount => "Sheeps Left: " + sheepCount.ToString() + "!")
            .Subscribe(timeLeft => sheepText.text = timeLeft.ToString()).AddTo(this);

        GameStore.Subject.Select(state => state.sheepCount).DistinctUntilChanged().Skip(1)
            .Subscribe(_ => GetComponent<AudioSource>().Play()).AddTo(this);
        
        //Show Game over panel 
        GameStore.Subject.Where(state => state.gameOver).Subscribe(state =>
        {
            highScoreText.enabled = false;
            timeText.enabled = false;
            sheepText.enabled = false;
            finalSheepText.text = "You Collected\n" + (GameStore.sheepCount - GameStore.State.sheepCount).ToString() + " Sheep!";
            finalScoreText.text = "Your Score:\n" + GameStore.State.score.ToString("#,#");
            gameOverPanel.SetActive(true);
        }).AddTo(this);
    }
}