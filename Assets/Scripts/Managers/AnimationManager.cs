using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour, IProvidable
{
    private const float FallDuration = 0.5f;

    private void Awake()
    {
        ServiceProvider.Register(this);
    }
    public void StartAnimateItems(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToAnimate)
    {
        StartCoroutine(AnimateItems(itemsToAnimate));
    }

    public void StartShuffleItems(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToAnimate)
    {
        StartCoroutine(StartShuffle(itemsToAnimate));
    }

    public IEnumerator StartShuffle(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToAnimate)
    {
        yield return new WaitForSeconds(1f);
        for(int i = 0 ; i< itemsToAnimate.Count ;i++ )
        {
            StartCoroutine(SmoothMove(itemsToAnimate[i].item, itemsToAnimate[i].startPos, itemsToAnimate[i].endPos));
        }
        yield return null;
        ServiceProvider.MoveManager.OpenMove();
        ServiceProvider.ShuffleManager.Reset();
    }

    private IEnumerator SmoothMove(ItemBase itemObj, Vector3 startPosition, Vector3 targetPosition)
    {
        float duration = 0.8f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            itemObj.transform.position = Vector3.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

    }

    private IEnumerator AnimateItems(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToAnimate)
    {
        List<Coroutine> itemCoroutines = new List<Coroutine>();

        foreach (var (item, startPos, endPos) in itemsToAnimate)
        {
            item.transform.position = startPos;
            itemCoroutines.Add(StartCoroutine(AnimateItem(item, startPos, endPos)));
        }

        foreach (var coroutine in itemCoroutines)
        {
            yield return coroutine;
        }
            ServiceProvider.ShuffleManager.TryShuffle();

    }

    private IEnumerator AnimateItem(ItemBase item, Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < FallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / FallDuration);

            // Ease.InOutQuad
            t = t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
           // t *= t;

            item.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        item.transform.position = endPos;
    }
}