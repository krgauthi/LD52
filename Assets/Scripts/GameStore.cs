using System;
using Unidux;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class GameStore : SingletonMonoBehaviour<GameStore>, IStoreAccessor
{
    //TODO: Leaderboard
    //TODO: Add obstacles
    //TODO: Add Enemies / Health
    public static int sheepCount = 1000;
    private static int startingTime = 60;
    [SerializeField] private float cameraSpeed = 5f;
    [SerializeField] private float cameraHeight = 15f;
    [SerializeField] private GameObject sheepPrefab;
    [SerializeField] private Transform sheepContainer;
    [SerializeField] private int maxSheepGroup = 15;

    private Store<State> _store;

    public IStoreObject StoreObject
    {
        get { return Store; }
    }

    public static State State
    {
        get { return Store.State; }
    }

    public static Subject<State> Subject
    {
        get { return Store.Subject; }
    }

    private static State InitialState => new State(sheepCount, startingTime);

    public static Store<State> Store
    {
        get
        {
            return Instance._store = Instance._store ??
                                     new Store<State>(InitialState, new PlayerAction.Reducer(),
                                         new SheepAction.Reducer(),
                                         new GameAction.Reducer());
        }
    }

    public static object Dispatch<TAction>(TAction action)
    {
        return Store.Dispatch(action);
    }

    private void spawn(GameObject go, Transform container, int maxAmount, int maxGroup)
    {
        for (int i = 0; i < maxAmount;)
        {
            float x = Random.Range(-45, 45);
            float y = Random.Range(10, 90);
            var group = Random.Range(1, maxGroup) + i;
            for (; i < maxAmount && i <= group; i++)
            {
                float xMod = Random.Range(-4, 4);
                float yMod = Random.Range(-4, 4);
                GameObject instantiatedGameObject =
                    Instantiate(go, new Vector3(x + xMod, y + yMod, 0), Quaternion.identity);
                instantiatedGameObject.name = "Sheep " + i;
                instantiatedGameObject.transform.SetParent(container.transform);
            }
        }
    }

    private void Start()
    {
        //Spawn in Entities
        spawn(sheepPrefab, sheepContainer, sheepCount, maxSheepGroup);

        //SETUP Game Over Condiditons
        Store.Subject.Select(state => state.timeLeft).Where(timeLeft => timeLeft <= 0)
            .Subscribe(_ => GameStore.Store.Dispatch(GameAction.ActionCreator.GameOver())).AddTo(this);
        
        Store.Subject.Select(state => state.timeLeft).Where(timeLeft => timeLeft <= 0)
            .Subscribe(_ => GameStore.Store.Dispatch(GameAction.ActionCreator.GameOver())).AddTo(this);
        
        //Camera Movement Logic
        Store.Subject
            .Select(state => state.playerPosition)
            .Subscribe(pos =>
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(pos.x, pos.y, cameraHeight * -1),
                    cameraSpeed * Time.deltaTime);
            })
            .AddTo(this);

        //Camera Zoom Logic
        Observable.EveryUpdate()
            .Select(_ => Input.mouseScrollDelta.y)
            .Subscribe(scroll =>
            {
                GetComponent<Camera>().orthographicSize -= scroll;
                var size = GetComponent<Camera>().orthographicSize;
                if (size < 5)
                {
                    GetComponent<Camera>().orthographicSize = 5f;
                }
                else if (size > 30)
                {
                    GetComponent<Camera>().orthographicSize = 30f;
                }
            })
            .AddTo(this);
        
        //Start Timer
        Observable.Timer(TimeSpan.FromSeconds(1)).RepeatUntilDestroy(this)
            .Where(_ => GameStore.Store.State.gameOver == false)
            .Subscribe(_ => GameStore.Dispatch(GameAction.ActionCreator.DecrementTime())).AddTo(this);
    }

    void Update()
    {
        Store.Update();
    }
}