using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenManager : MonoBehaviour
{
    [SerializeField] WinningImage[] winningImages;

    [SerializeField] private float enlargeScale = 1.5f;
    [SerializeField] private float shrinkScale = 1f;
    [SerializeField] private float enlargeDuration = .25f;
    [SerializeField] private float shrinkDuration = .25f;

    public int numberOfImagesToShow;
    public InputPlayerController inputPlayer;

    private PointsBehavior pointsManager;
    public int maxPointsShow1;
    public int maxPointsShow2;
    public int maxPointsShow3;
    
    // Start is called before the first frame update
    void Start()
    {
        pointsManager = Singleton.Instance.PointsManager;
    }

    void ShowImages(int numberOfImages)
    {
        StartCoroutine(ShowImagesRoutine(numberOfImages));
    }

    private IEnumerator ShowImagesRoutine(int numberOfImages)
    {
        foreach (WinningImage x in winningImages)
        {
            x.winImage.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < numberOfImages; i++)
        {
            yield return StartCoroutine(EnlargeAndShrinkImage(winningImages[i]));
        }
    }

    private IEnumerator EnlargeAndShrinkImage(WinningImage winningImage)
    {
        yield return StartCoroutine(ChangeImageScale(winningImage, enlargeScale, enlargeDuration));
        yield return StartCoroutine(ChangeImageScale(winningImage, shrinkScale, shrinkDuration));
    }

    private IEnumerator ChangeImageScale(WinningImage img, float targeScale, float duration)
    {
        Vector3 initialScale = img.winImage.transform.localScale;
        Vector3 finalScale = new Vector3(targeScale, targeScale, targeScale);

        float elapsedTime = 0;

        while ( elapsedTime < duration)
        {
            img.winImage.transform.localScale = Vector3.Lerp(initialScale, finalScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        img.winImage.transform.localScale = finalScale;
    }

    public void CalculateStars()
    {
        if (pointsManager.Points < maxPointsShow1) ShowImages(1);
        else if(pointsManager.Points < maxPointsShow2) ShowImages(2);
        else if(pointsManager.Points < maxPointsShow3) ShowImages(3);
        else ShowImages(0);

        inputPlayer.enabled = false;
        StartCoroutine(OnPause());
    }

    IEnumerator OnPause()
    {
        yield return new WaitForSeconds(2f);
        Singleton.Instance.GameManager.OnPause();
    }
}
