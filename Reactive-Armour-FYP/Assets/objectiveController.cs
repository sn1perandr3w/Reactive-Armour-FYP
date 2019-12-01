using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class objectiveController : MonoBehaviour
{

    [Serializable]
    public class Objective
    {
        public string objectiveDesc = "NO DESC";

        //-1 for fail, 0 for incomplete, 1 for complete
        public int status;


        //0 Defend until area clear, 1 Item retrieval, 2 Escort, 3 Activation
        public int objectiveType;

        public List<GameObject> objectiveActors;

        public List<GameObject> friendlyObjectiveActors;
        public int minFriendliesToSurvive;


        public List<GameObject> enemyObjectiveActors;
        public int minEnemiesAlive;

        public float distanceThreshold;
    }


    public List<Objective> objectives;

    public GameObject objectivesListText;

    public string[] objectiveStateText = new string[]{"Failed","Incomplete","Complete"};



    // Start is called before the first frame update
    void Start()
    {


        UpdateObjectives();

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            //Cleanup before checking objective status.
            for (int j = 0; j < objectives[i].objectiveActors.Count; j++)
            {
                if (objectives[i].objectiveActors[j] == null)
                {
                    objectives[i].objectiveActors.Remove(objectives[i].objectiveActors[j]);
                }
            }

            for (int j = 0; j < objectives[i].friendlyObjectiveActors.Count; j++)
            {
                if (objectives[i].friendlyObjectiveActors[j] == null)
                {
                    objectives[i].friendlyObjectiveActors.Remove(objectives[i].friendlyObjectiveActors[j]);
                }
            }

            for (int j = 0; j < objectives[i].enemyObjectiveActors.Count; j++)
            {
                if (objectives[i].enemyObjectiveActors[j] == null)
                {
                    objectives[i].enemyObjectiveActors.Remove(objectives[i].enemyObjectiveActors[j]);
                }
            }


            //0 Defend until area clear
            if (objectives[i].objectiveType == 0)
            {
                //Losses too much, objective failed.
                if (objectives[i].friendlyObjectiveActors.Count < objectives[i].minFriendliesToSurvive)
                {
                    objectives[i].status = -1;
                    UpdateObjectives();
                }
                //Losses acceptable. Enough enemies eliminated.
                else if (objectives[i].friendlyObjectiveActors.Count >= objectives[i].minFriendliesToSurvive && objectives[i].enemyObjectiveActors.Count <= objectives[i].minEnemiesAlive)
                {
                    objectives[i].status = 1;
                    UpdateObjectives();
                }

            } else
            //1 Item retrieval
            if (objectives[i].objectiveType == 1)
            {
                //Item(s) retrieved. Mission complete.
                if (objectives[i].objectiveActors.Count == 0)
                {
                    objectives[i].status = 1;
                    UpdateObjectives();
                }
            }
            else
            //2 Escort
            if (objectives[i].objectiveType == 2)
            {
                //Losses too much, objective failed.
                if (objectives[i].friendlyObjectiveActors.Count < objectives[i].minFriendliesToSurvive)
                {
                    objectives[i].status = -1;
                    UpdateObjectives();
                }

                //Losses acceptable. Enough enemies eliminated.
                else if (objectives[i].friendlyObjectiveActors.Count >= objectives[i].minFriendliesToSurvive)
                {
                    float distanceToEndpoint = Vector3.Distance(objectives[i].objectiveActors[0].transform.position, objectives[i].objectiveActors[1].transform.position);

                    if (distanceToEndpoint <= objectives[i].distanceThreshold)
                    {
                        objectives[i].status = 1;
                        UpdateObjectives();
                    }
                }
            }
            else
            //3 Activation (Must be controlled from script attached to object.)
            if (objectives[i].objectiveType == 3)
            {
                
                if (objectives[i].objectiveActors[0].GetComponent<interactableScript>().isActivated == true)
                {
                    objectives[i].status = 1;
                    UpdateObjectives();
                }
                
            }
        }
    }


    public void UpdateObjectives()
    {
        objectivesListText.GetComponent<Text>().text = "Objectives:" + "\n";

        for (int i = 0; i < objectives.Count; i++)
        {
            objectivesListText.GetComponent<Text>().text += "- " + objectives[i].objectiveDesc + " [" + objectiveStateText[objectives[i].status + 1] + "]\n";

        }
    }
}
