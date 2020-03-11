using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public MobileTouchHandler androidController;
    public GameObject bubble;
    private int colorIndex;
    private List<FingerTouch> fingerTouches = new List<FingerTouch>();
    public List<Color> colors = new List<Color>();
    public bool DelayPassed { get; set; } = false;

    private void Start()
    {
        if (PlayerPrefs.GetInt("darkMode", 1) == 1)
        {
            colors.Add(Color.white);
        }
        else
        {
            colors.Add(Color.black);
        }
        colorIndex = Random.Range(0, 10);
    }

    public void SaveTouch(Touch touch)
    {
        androidController.PlayTapSound();
        GameObject circle = Instantiate(bubble);
        circle.GetComponent<SpriteRenderer>().color = CrossSceneInfos.mode == Mode.INDIVIDUAL ? colors[colorIndex] : colors[9];
        NextColorIndex();
        Vector3 fingerPos = touch.position;
        fingerPos.z = 5;
        circle.transform.position = Camera.main.ScreenToWorldPoint(fingerPos);
        FingerTouch ft = new FingerTouch
        {
            time = Time.time,
            touch = touch,
            circle = circle
        };
        fingerTouches.Add(ft);
    }

    public void DeleteTouch(Touch touch)
    {
        foreach (FingerTouch ft in fingerTouches.ToList())
        {
            if (ft.touch.fingerId == touch.fingerId)
            {
                Destroy(ft.circle);
                fingerTouches.Remove(ft);
            }
        }
    }

    public void UpdateTouch(Touch touch)
    {
        foreach (FingerTouch ft in fingerTouches.ToList())
        {
            if (ft.touch.fingerId == touch.fingerId)
            {
                Vector3 fingerPos = touch.position;
                fingerPos.z = 5;
                ft.circle.transform.position = Camera.main.ScreenToWorldPoint(fingerPos);
            }
        }
    }

    public void CheckForResult()
    {
        float lastFingerTime = GetLastFingerTime();
        if (lastFingerTime != 0)
        {
            if (Time.time - lastFingerTime >= 3f)
            {
                DelayPassed = true;
                if (CrossSceneInfos.mode == Mode.INDIVIDUAL)
                {
                    PickRandomFinger();
                }
                else
                {
                    PickTeams();
                }
            }
        }
    }

    private void PickRandomFinger()
    {
        List<int> indexes = GetRandomIndexes();
        List<FingerTouch> winners = new List<FingerTouch>();
        foreach (int i in indexes)
        {
            winners.Add(fingerTouches[i]);
        }
        androidController.PlayPickSound();
        foreach (FingerTouch ft in fingerTouches.ToList())
        {
            if (!winners.Contains(ft))
            {
                Destroy(ft.circle);
                fingerTouches.Remove(ft);
            }
            else
            {
                ft.circle.GetComponent<Animator>().enabled = false;
                ft.circle.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
        Invoke("ClearList", 5f);
    }

    private void PickTeams()
    {
        List<List<FingerTouch>> teams = GenerateTeams();
        for (int i = 0; i < teams.Count; i++)
        {
            for (int j = 0; j < teams[i].Count; j++)
            {
                teams[i][j].circle.GetComponent<SpriteRenderer>().color = colors[i];
                teams[i][j].circle.GetComponent<Animator>().enabled = false;
                teams[i][j].circle.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            }
        }
        Invoke("ClearList", 5f);
    }

    private float GetLastFingerTime()
    {
        float max = 0;
        foreach (var ft in fingerTouches.ToList())
        {
            if (ft.time > max)
            {
                max = ft.time;
            }
        }
        return max;
    }

    private List<List<FingerTouch>> GenerateTeams()
    {
        List<FingerTouch> allTeams = fingerTouches.ToList();
        if (CrossSceneInfos.NbOfWinners >= allTeams.Count)
        {
            return new List<List<FingerTouch>> { allTeams };
        }
        List<FingerTouch> randomList = ShuffleList<FingerTouch>(allTeams);
        List<List<FingerTouch>> allTeamGroups = new List<List<FingerTouch>>();
        for (int i = 0; i < CrossSceneInfos.NbOfWinners; i++)
        {
            allTeamGroups.Add(new List<FingerTouch>());
        }
        for (int i = 0; i < randomList.Count; i++)
        {
            int j = i % CrossSceneInfos.NbOfWinners;
            allTeamGroups[j].Add(randomList[i]);
        }
        return allTeamGroups;
    }

    private List<E> ShuffleList<E>(List<E> inputList)
    {
        List<E> randomList = new List<E>();

        System.Random r = new System.Random();
        int randomIndex = 0;
        while (inputList.Count > 0)
        {
            randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
            randomList.Add(inputList[randomIndex]); //add it to the new, random list
            inputList.RemoveAt(randomIndex); //remove to avoid duplicates
        }

        return randomList; //return the new random list
    }

    private void NextColorIndex()
    {
        colorIndex++;
        if (colorIndex >= colors.Count) colorIndex = 0;
    }

    private void ClearList()
    {
        foreach (FingerTouch ft in fingerTouches.ToList())
        {
            Destroy(ft.circle);
            fingerTouches.Remove(ft);
        }
        DelayPassed = false;
    }

    private List<int> GetRandomIndexes()
    {
        int max = CrossSceneInfos.NbOfWinners;
        if (max > fingerTouches.Count) max = fingerTouches.Count;
        List<int> indexes = new List<int>();
        while (indexes.Count < max)
        {
            int i = UnityEngine.Random.Range(0, fingerTouches.Count);
            if (!indexes.Contains(i))
            {
                indexes.Add(i);
            }
        }
        return indexes;
    }

}
