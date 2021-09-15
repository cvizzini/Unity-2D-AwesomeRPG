using System.Collections;
using UnityEngine;


public class Arc : MonoBehaviour
{
    /// <summary>
    /// This formula results in speeding up the velocity of ammunition when
    /// the destination is further away
    /// </summary>
    /// <param name="destination">The destination.</param>
    /// <param name="duration">The duration.</param>
    /// <returns></returns>
    public IEnumerator TravelArc(Vector3 destination, float duration)
    {
        // 3
        var startPosition = transform.position;
        // 4
        var percentComplete = 0.0f;
        // 5
        while (percentComplete < 1.0f)
        {
            //Time.deltaTime is the amount of time elapsed since the last frame was drawn
            percentComplete += Time.deltaTime / duration;

            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);

            // To achieve the effect where the AmmoObject appears to move
            //smoothly between two points at a constant speed, we use a widely
            //used technique in game programming called Linear Interpolation.
            transform.position = Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up * currentHeight;

           

            // Pause execution of the Coroutine until the next frame
            yield return null;
        }
        // 9
        gameObject.SetActive(false);
    }
}
