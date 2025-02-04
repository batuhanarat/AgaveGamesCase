using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, IProvidable> _registerDictionary = new();

    public static AssetLib AssetLib => GetManager<AssetLib>();
    public static Grid GameGrid => GetManager<Grid>();
    public static AnimationManager AnimationManager => GetManager<AnimationManager>();
    public static UIManager UIManager => GetManager<UIManager>();
    public static ILevelManager LevelManager => GetManager<LevelManager>();
    public static IScoreManager ScoreManager => GetManager<ScoreManager>();
    public static IMoveManager MoveManager => GetManager<MoveManager>();
    public static IFallManager FallManager => GetManager<FallManager>();
    public static IItemFactory ItemFactory => GetManager<ItemFactory>();
    public static IShuffleManager ShuffleManager => GetManager<ShuffleManager>();
    public static IMatchManager MatchManager => GetManager<MatchManager>();
    public static GameConfig GameConfig;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {
        /*
        SceneManager.sceneLoaded += (_, _) =>
        {
            if(GameConfig == null) {
                GameConfig = Resources.Load<GameConfig>("ScriptableObjects/GameConfigSO");
            }
            //Self registered.
            _ = new UIManager();
            _ = new LevelManager();
            _ = new MoveManager();
            _ = new ScoreManager();
        };
        */


        if(GameConfig == null) {
            GameConfig = Resources.Load<GameConfig>("ScriptableObjects/GameConfigSO");
        }
        //Self registered.
            _ = new Grid();
            _ = new UIManager();
            _ = new ScoreManager();
            _ = new LevelManager();
            _ = new MoveManager();
            _ = new ItemFactory();
            _ = new FallManager();
            _ = new ShuffleManager();
            _ = new MatchManager();
            SceneManager.sceneLoaded += (_, _) =>
            {
                ScoreManager.Reset();
                MoveManager.Reset();
            };

    }

    private static T GetManager<T>() where T : class, IProvidable
    {
        if (_registerDictionary.ContainsKey(typeof(T)))
        {
            return (T)_registerDictionary[typeof(T)];
        }

        return null;
    }

    public static T Register<T>(T target) where T: class, IProvidable
    {
        _registerDictionary[typeof(T)] = target;
        return target;
    }
}