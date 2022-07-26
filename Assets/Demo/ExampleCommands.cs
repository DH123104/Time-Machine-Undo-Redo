
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// This command shows a game object.
/// </summary>
public class ShowGameObjectCommand : TMAbstractCommand
{
    /// <summary>
    /// The game object to modify.
    /// </summary>
    private GameObject m_oGameObject = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject">The game object to modify.</param>
    public ShowGameObjectCommand(GameObject gameObject)
    {
        m_oGameObject = gameObject;
        Label = "Show Object";
    }

    /// <summary>
    /// This executes this command.
    /// </summary>
    public override void Execute()
    {
        if (m_oGameObject != null &&
            m_oGameObject.GetComponent<Renderer>() != null)
            m_oGameObject.GetComponent<Renderer>().enabled = true;
    }

    /// <summary>
    /// This gets the undo command for this command.
    /// </summary>
    /// <returns>The undo command.</returns>
    public override TMCommandInterface GetUndoCommand()
    {
        return new HideGameObjectCommand(m_oGameObject);
    }
}

/// <summary>
/// This command hides a game object.
/// </summary>
public class HideGameObjectCommand : TMAbstractCommand
{
    private GameObject m_oGameObject = null;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject">The game object to modify.</param>
    public HideGameObjectCommand(GameObject gameObject)
    {
        m_oGameObject = gameObject;
        Label = "Hide Object";
    }

    /// <summary>
    /// This executes this command.
    /// </summary>
    public override void Execute()
    {
        if (m_oGameObject != null &&
            m_oGameObject.GetComponent<Renderer>() != null)
            m_oGameObject.GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// This gets the undo command for this command.
    /// </summary>
    /// <returns>The undo command.</returns>
    public override TMCommandInterface GetUndoCommand()
    {
        return new ShowGameObjectCommand(m_oGameObject);
    }
}

/// <summary>
/// This command sets a game objects material color.
/// </summary>
public class SetGameObjectMaterialColorCommand : TMAbstractCommand
{
    /// <summary>
    /// The game object to modify.
    /// </summary>
    private GameObject m_oGameObject = null;
    
    /// <summary>
    /// The old color of the material.
    /// </summary>
    private Color m_oOldColor;

    /// <summary>
    /// The new color of the material.
    /// </summary>
    private Color m_oNewColor;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject">The game object to modify.</param>
    /// <param name="newColor">The new color to set.</param>
    public SetGameObjectMaterialColorCommand(GameObject gameObject, Color newColor)
    {
        m_oGameObject = gameObject;
        m_oOldColor = m_oGameObject.GetComponent<Renderer>().material.color;
        m_oNewColor = newColor;
        Label = "Set Material Color";
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="gameObject">The game object to modify.</param>
    /// <param name="newColor">The new color to set.</param>
    /// <param name="oldColor">The old color.</param>
    public SetGameObjectMaterialColorCommand(GameObject gameObject, Color newColor, Color oldColor)
    {
        m_oGameObject = gameObject;
        m_oOldColor = oldColor;
        m_oNewColor = newColor;
        Label = "Set Material Color";
    }

    /// <summary>
    /// This executes this command.
    /// </summary>
    public override void Execute()
    {
        m_oGameObject.GetComponent<Renderer>().material.color = m_oNewColor;
    }

    /// <summary>
    /// This gets the undo command for this command.
    /// </summary>
    /// <returns>The undo command.</returns>
    public override TMCommandInterface GetUndoCommand()
    {
        return new SetGameObjectMaterialColorCommand(m_oGameObject, m_oOldColor, m_oNewColor);
    }
}
