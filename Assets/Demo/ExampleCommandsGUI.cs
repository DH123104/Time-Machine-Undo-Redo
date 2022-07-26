
using UnityEngine;
using System.Collections;

/// <summary>
/// This is the mono behaviour used for the example commands GUI.
/// </summary>
public class ExampleCommandsGUI : MonoBehaviour 
{
    /// <summary>
    /// The target game object to modify.
    /// </summary>
    public GameObject target = null;

    /// <summary>
    /// Indicates if there is a command group active.
    /// </summary>
    private bool groupActive = false;    

    /// <summary>
    /// Displays the GUI.
    /// </summary>
    public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(Screen.width - 175,
                                     Screen.height - 300,
                                     150,
                                     275),
                            "Cube Controls",
                            GUI.skin.window);

        //HINT:  This section shows how to set the position of a transform component using the set field command.
        if (GUILayout.Button("Random Position"))
        {
            Vector3 newPos = new Vector3(Random.Range(-2.0f, 2.0f),
                                         Random.Range(-2.0f, 2.0f),
                                         Random.Range(-2.0f, 2.0f)) +
                             target.transform.position;
            TMHelper.SetField(target.transform, "position", newPos);
        }

        //HINT:  This section shows how to set the rotation of a transform component using the set field command.
        if (GUILayout.Button("Random Rotation"))
        {
            Vector3 rotation = new Vector3(Random.Range(-180.0f, 180.0f),
                                           Random.Range(-180.0f, 180.0f),
                                           Random.Range(-180.0f, 180.0f));
            TMHelper.SetField(target.transform, "rotation", Quaternion.Euler(rotation));
        }

        //HINT:  This section shows how to set the scale of a transform component using the set field command.
        if (GUILayout.Button("Random Scale"))
        {
            Vector3 scale = new Vector3(Random.Range(0.25f, 3.0f),
                                        Random.Range(0.25f, 3.0f),
                                        Random.Range(0.25f, 3.0f));
            TMHelper.SetField(target.transform, "localScale", scale);
        }

        //HINT:  This section shows how to set the color of the material set to a game object using a CUSTOM command.
        if (GUILayout.Button("Random Color"))
        {
            Color color = new Color(Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f));
            TMHelper.CommandHistory.Do(new SetGameObjectMaterialColorCommand(target, color));
        }

        //HINT:  This section shows how to show/hide a game object using a CUSTOM command.
        if (target.GetComponent<Renderer>().enabled)
        {
            if (GUILayout.Button("Hide Cube"))
            {
                TMHelper.CommandHistory.Do(new HideGameObjectCommand(target));
            }
        }
        else
        {
            if (GUILayout.Button("Show Cube"))
            {
                TMHelper.CommandHistory.Do(new ShowGameObjectCommand(target));
            }
        }

        GUILayout.FlexibleSpace();

        //HINT:  This secton shows how to push/pop a command group.
        if (!groupActive)
        {
            if (GUILayout.Button("Push Cmd Group"))
            {
                TMHelper.CommandHistory.PushCommandGroup("Test Command Group");
                groupActive = true;
            }
        }
        else
        {
            if (GUILayout.Button("Pop Cmd Group"))
            {
                TMHelper.CommandHistory.PopCommandGroup();
                groupActive = false;
            }
        }

        //HINT: This section shows how to undo/redo/clear and reset a command history.
        if (!groupActive)
        {
            if (GUILayout.Button("Undo (" + TMHelper.CommandHistory.UndoCommands.Count + " cmds)"))
            {
                TMHelper.CommandHistory.Undo();
            }

            if (GUILayout.Button("Redo (" + TMHelper.CommandHistory.RedoCommands.Count + " cmds)"))
            {
                TMHelper.CommandHistory.Redo();
            }

            if (GUILayout.Button("Reset"))
            {
                while (TMHelper.CommandHistory.CanUndo)
                    TMHelper.CommandHistory.Undo();
                TMHelper.CommandHistory.Clear();
            }
        }

        GUILayout.EndArea();

        //HINT:  This section shows how to display all of the current undo commands.
        GUILayout.BeginArea(new Rect(25,
                                     Screen.height - 300,
                                     200,
                                     275),
                            "Undo Commands",
                            GUI.skin.window);

        foreach (TMCommandInterface command in TMHelper.CommandHistory.UndoCommands)
        {
            GUILayout.Label(command.Label);
        }

        GUILayout.EndArea();


        //HINT:  This section shows how to display all of the current redo commands.
        GUILayout.BeginArea(new Rect(250,
                                     Screen.height - 300,
                                     200,
                                     275),
                            "Redo Commands",
                            GUI.skin.window);

        foreach (TMCommandInterface command in TMHelper.CommandHistory.RedoCommands)
        {
            GUILayout.Label(command.Label);
        }

        GUILayout.EndArea();
    }
}
