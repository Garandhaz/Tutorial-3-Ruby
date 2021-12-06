using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NonPlayerCharacter : MonoBehaviour
{
    public float displayTime = 4.0f;

    public string[] dialogue;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DisplayDialog(int dialogue_option)
    {
        //Singleton Reference
        UIDialogue ui_diag = UIDialogue.instance;

        //Checks if dialogue array is more than 0, and contains dialogue_option
        if (dialogue.Length > 0 && dialogue_option < dialogue.Length && dialogue_option >= 0)
        {
            ui_diag.SetText(dialogue[dialogue_option]);
            ui_diag.Show(displayTime);
        }
    }

    public void Interact()
    {
        //Singleton Reference
        GameModel model = GameModel.instance;
        if (model.HasKeyItem())
        {
            DisplayDialog(1);
            model.Win();
        }
        else
        {
            DisplayDialog(0);

            //Singleton References
            UIObjective ui_objective = UIObjective.instance;
            LevelInfo level_info = LevelInfo.instance;

            ui_objective.SetText(level_info.objective_text);
        }
    }
}
