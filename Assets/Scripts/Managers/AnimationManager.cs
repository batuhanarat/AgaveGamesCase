using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour, IProvidable
{
    private const float FallDuration = 0.3f;

    private void Awake()
    {
        ServiceProvider.Register(this);
    }
    public void StartFallingItemsAnimation(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToFall)
    {
        StartCoroutine(AnimateFallingItems(itemsToFall));
    }

    private IEnumerator AnimateFallingItems(List<(ItemBase item, Vector3 startPos, Vector3 endPos)> itemsToFall)
    {
        List<Coroutine> itemCoroutines = new List<Coroutine>();

        foreach (var (item, startPos, endPos) in itemsToFall)
        {
            item.transform.position = startPos;
            itemCoroutines.Add(StartCoroutine(AnimateItem(item, startPos, endPos)));
        }

        foreach (var coroutine in itemCoroutines)
        {
            yield return coroutine;
        }

    }

    private IEnumerator AnimateItem(ItemBase item, Vector3 startPos, Vector3 endPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < FallDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / FallDuration);

            // Ease.InOutQuad
           // t = t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
            t *= t;

            item.transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        item.transform.position = endPos;
    }
}