using System;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
using Range = UnityEngine.SocialPlatforms.Range;

public class SheepPresenter : MonoBehaviour
{
    [SerializeField] private float safeZone;
    [SerializeField] private Animator _animation;
    [SerializeField] private bool isMenuSheep = false;
    [SerializeField] private int randomMovementAmount;

    // Start is called before the first frame update
    void Start()
    {
        _animation.speed = Random.value;

        var positionStream = Observable.EveryUpdate().TakeUntilDestroy(this).Select(_ => gameObject.transform.position);
        var directionStream = positionStream.TakeUntilDestroy(this).Pairwise()
            .Select(foo => foo.Current - foo.Previous);
        var velocityStream = positionStream.TakeUntilDestroy(this).Pairwise()
            .Select(foo => (foo.Current - foo.Previous).magnitude / Time.deltaTime);
        var harvestStream = positionStream.Where(position => position.y < 0);


        // Handle Sheep that reach the goal
        harvestStream.Where(_ => GameStore.State.gameOver == false).Take(1).Subscribe(_ =>
        {
            //Score Condition
            Destroy(gameObject, 10f);
            if (!isMenuSheep)
            {
                GameStore.Store.Dispatch(SheepAction.ActionCreator.HarvestSheep());
            }
        }).AddTo(this);

        harvestStream.Subscribe(_ =>
        {
            //What to do after harvest
            var direction = new Vector2(Random.Range(-.25f, .25f), -.25f) * Random.value;
            gameObject.GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
        }).AddTo(this);


        // Rotate Sheep in direction they are moving //TODO: limit 90 degree rotation
        directionStream.Subscribe(direction =>
            {
                if (direction.x != 0)
                {
                    var targetAngles = 180f * Vector3.up;
                    if (direction.x < 0)
                    {
                        targetAngles = Vector3.zero;
                    }

                    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 10f * Time.deltaTime);
                }
            })
            .AddTo(this);

        //Animate Sheep faster while moving
        velocityStream.Where(velocity => velocity > 1).Subscribe(velocity =>
            {
                if (velocity > 0)
                {
                    _animation.speed = velocity * .1f;
                }
                else
                {
                    _animation.speed = Random.value;
                }
            })
            .AddTo(this);

        //Random movement
        Observable.Timer(TimeSpan.FromSeconds(Random.Range(1, 30))).RepeatUntilDestroy(this)
            .Subscribe(_ =>
            {
                //Move Random
                var body = gameObject.GetComponent<Rigidbody2D>();
                var direction = new Vector2(Random.Range(randomMovementAmount * -1, randomMovementAmount),
                    Random.Range(randomMovementAmount * -1, randomMovementAmount));
                body.AddForce(direction, ForceMode2D.Impulse);
                if (Random.Range(1, 10) > 9)
                {
                    GetComponent<AudioSource>().Play();
                }
            }).AddTo(this);

        if (!isMenuSheep)
        {
            //Evade the Player
            GameStore.Subject
                .TakeUntilDisable(this)
                .StartWith(GameStore.State)
                .Select(state => state.playerPosition)
                .Where(playerPosition => (playerPosition - gameObject.transform.position).magnitude <= safeZone)
                .Subscribe(playerPosition =>
                {
                    var body = gameObject.GetComponent<Rigidbody2D>();
                    var direction = transform.position - playerPosition; //Maybe add som random value?
                    body.AddForce(direction, ForceMode2D.Force);
                    // if (Random.Range(1, 10) > 8.99)
                    // {
                    //     GetComponent<AudioSource>().Play();
                    // }
                })
                .AddTo(this);
        }
    }
}