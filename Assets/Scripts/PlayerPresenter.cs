using System;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Diagnostics;

public class PlayerPresenter : MonoBehaviour
{
    public float speed;
    public Vector3 scare;
    [SerializeField] private Animator _animation;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _idleSprite;

    // Start is called before the first frame update
    void Start()
    {
        var positionStream = Observable.EveryUpdate().Select(_ => gameObject.transform.position);
        var directionStream = positionStream.Pairwise()
            .Select(foo => foo.Current - foo.Previous);
        var velocityStream = positionStream.Pairwise()
            .Select(foo => (foo.Current - foo.Previous).magnitude / Time.deltaTime);

        positionStream.Subscribe(position =>
            GameStore.Store.Dispatch(PlayerAction.ActionCreator.UpdatePosition(gameObject.transform.position))).AddTo(this);

        directionStream.Subscribe(direction =>
            {
                scare = direction * 100;
                if (direction.x != 0)
                {
                    var targetAngles = 180f * Vector3.up;
                    if (direction.x < 0)
                    {
                        targetAngles = Vector3.zero;
                    }
                    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngles, 5f * Time.deltaTime);
                }
                
            })
            .AddTo(this);
        
        velocityStream.Subscribe(velocity =>
            {
                
                if (velocity > 0)
                {
                    _animation.enabled = true;
                    _animation.speed = velocity * .1f;
                }
                else
                {
                    _animation.enabled = false;
                    _spriteRenderer.sprite = _idleSprite;
                }
            })
            .AddTo(this);

        //Follow Mouse down
        Observable.EveryUpdate()
            .Where(_ => GameStore.Store.State.gameOver == false)
            .Where(_ => Input.GetMouseButton(0))
            .Select(click =>
            {
                if (Camera.main != null)
                {
                    return Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }

                throw new InvalidOperationException();
            })
            .Subscribe(clickPos =>
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(clickPos.x, clickPos.y),
                    speed * Time.deltaTime);
                if (transform.position.y < -1)
                {
                    transform.position = new Vector3(0, 5, 0);
                }
            })
            .AddTo(this);

        //Double Click detection
        var clickStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0));

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(xs =>
            {
                GetComponent<AudioSource>().Play();

            }) //Bark
            .AddTo(this);
    }
}