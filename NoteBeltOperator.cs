using UnityEngine;

public class NoteBeltOperator : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] notes;

    private int BPM;

    [SerializeField]
    private float distance;
    [SerializeField]
    private float startPosition;
    [SerializeField]
    private float endPosition;
    
    void Update()
    {
        float delta = Time.deltaTime * distance * BPM;

        for(int i = 0; i < notes.Length; i++)
        {
            Vector3 oldPosition = notes[i].anchoredPosition;
            if(oldPosition.x > endPosition)
            {
                notes[i].anchoredPosition = new Vector3(oldPosition.x - delta, oldPosition.y, 0f);
            }
            else
            {
                ResetEvent(i);
            }
        }
    }

    private void ResetEvent(int i)
    {
        notes[i].anchoredPosition = new Vector3(startPosition, 0f, 0f);
    }

    public void SetBPM(int newValue)
    {
        BPM = newValue;
    }
}
