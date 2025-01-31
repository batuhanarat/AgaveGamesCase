using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ServiceProvider
{
    private static readonly Dictionary<Type, IProvidable> _registerDictionary = new();

    public static UIManager UIManager => GetManager<UIManager>();
    public static LevelManager LevelManager => GetManager<LevelManager>();
    public static AssetLib AssetLib => GetManager<AssetLib>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void InitializeServiceProvider()
    {
        SceneManager.sceneLoaded += (_, _) =>
        {
            //Self registered.
            _ = new UIManager();
            _ = new LevelManager();

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