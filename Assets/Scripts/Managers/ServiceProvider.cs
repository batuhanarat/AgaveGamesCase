using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, IProvidable> _registerDictionary = new();

    public static Grid GameGrid => GetManager<Grid>();
    public static UIManager UIManager => GetManager<UIManager>();
    public static LevelManager LevelManager => GetManager<LevelManager>();
    public static ScoreManager ScoreManager => GetManager<ScoreManager>();
    public static MoveManager MoveManager => GetManager<MoveManager>();
    public static FallManager FallManager => GetManager<FallManager>();
    public static AnimationManager AnimationManager => GetManager<AnimationManager>();
    public static AssetLib AssetLib => GetManager<AssetLib>();
    public static ItemFactory ItemFactory => GetManager<ItemFactory>();
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
        _ = new UIManager();
        _ = new LevelManager();
        _ = new MoveManager();
        _ = new ScoreManager();
        _ = new ItemFactory();
        _ = new FallManager();
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