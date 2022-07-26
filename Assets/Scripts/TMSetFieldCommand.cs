/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Reflection;

/// <summary>
/// This command is a general command used to set the field/variable of any component.
/// </summary>
public class TMSetFieldCommand : TMAbstractCommand
{
    /// <summary>
    /// This is the target component of this command.
    /// </summary>
    private Component m_oTarget = null;

    /// <summary>
    /// This is the member info of the field to modify.
    /// </summary>
    private MemberInfo m_oField = null;

    /// <summary>
    /// This is the name of the field to modify.
    /// </summary>
    private string m_szFieldName = null;

    /// <summary>
    /// This is the old value of the field.
    /// </summary>
    private object m_oOldValue = null;

    /// <summary>
    /// This is the new value of the field.
    /// </summary>
    private object m_oNewValue = null;

    /// <summary>
    /// This indicates if this set field command is valid.
    /// </summary>
    private bool m_bIsValid = true;

    /// <summary>
    /// This gets the target component of this command.
    /// </summary>
    public Component Target
    {
        get { return m_oTarget; }
    }

    /// <summary>
    /// This gets the target field of this command.
    /// </summary>
    public MemberInfo Field
    {
        get { return m_oField; }
    }

    /// <summary>
    /// This gets the target field name of this command.
    /// </summary>
    public string FieldName
    {
        get { return m_szFieldName; }
    }

    /// <summary>
    /// This gets the old value of the field before this command was executed.
    /// </summary>
    public object OldValue
    {
        get { return m_oOldValue; }
    }

    /// <summary>
    /// This gets the new value of the field.
    /// </summary>
    public object NewValue
    {
        get { return m_oNewValue; }
    }

    /// <summary>
    /// This indicates if this set field command is valid with the data passed in.
    /// </summary>
    public bool IsValid
    {
        get { return m_bIsValid; }
    }

    /// <summary>
    /// This is the constructor for the set field command.
    /// </summary>
    /// <param name="target">The target component to set the field on.</param>
    /// <param name="field">This is the field to set.</param>
    /// <param name="value">This is the value to set the field to.</param>
    public TMSetFieldCommand(Component target, string field, object value)
    {
        m_oTarget = target;
        m_szFieldName = field;
        if (m_oTarget != null)
        {
            Type type = m_oTarget.GetType();
            m_oField = type.GetProperty(field);
            if (m_oField == null)
                m_oField = type.GetField(field);
        }
        m_oNewValue = value;

        if (m_oTarget == null)
        {
            Debug.LogError("[TimeMachine] No target specified for SetFieldCommand!");
            m_bIsValid = false;
            return;
        }

        if (m_oField == null)
        {
            Debug.LogError("[TimeMachine] No field \"" + field + "\" found for SetFieldCommand!");

            m_bIsValid = false;
            return;
        }

        if (((m_oField is PropertyInfo) && (m_oField as PropertyInfo).PropertyType != m_oNewValue.GetType()) ||
            ((m_oField is FieldInfo) && (m_oField as FieldInfo).FieldType != m_oNewValue.GetType()))
        {
            Debug.LogError("[TimeMachine] Invalid value passed in for field \"" + field + "\" for SetFieldCommand!");
            m_bIsValid = false;
            return;
        }

        if (m_oField is PropertyInfo)
            m_oOldValue = (m_oField as PropertyInfo).GetValue(m_oTarget, null);
        else if (m_oField is FieldInfo)
            m_oOldValue = (m_oField as FieldInfo).GetValue(m_oTarget);

        Label = "SetField: \"" + m_szFieldName + "\" in \"" + m_oTarget.name + "\" set to \"" + m_oNewValue.ToString() + "\"";
    }

    /// <summary>
    /// This is the constructor for the set field command.
    /// </summary>
    /// <param name="target">The target component to set the field on.</param>
    /// <param name="field">This is the field to set.</param>
    /// <param name="oldValue">This is the old value of the field.</param>
    /// <param name="newValue">This is the new value to set the field to.</param>
    private TMSetFieldCommand(Component target, MemberInfo field, object oldValue, object newValue)
    {
        m_oTarget = target;
        m_oField = field;
        m_szFieldName = m_oField.Name;
        m_oOldValue = oldValue;
        m_oNewValue = newValue;

        Label = "SetField: \"" + m_szFieldName + "\" in \"" + m_oTarget.name + "\" set to \"" + m_oNewValue.ToString() + "\"";
    }

    /// <summary>
    /// This executes this set field command.
    /// </summary>
    public override void Execute()
    {
        if (m_oTarget == null)
        {
            Debug.LogError("[TimeMachine] No target set for SetFieldCommand!");
            return;
        }

        if (m_oField == null)
        {
            Debug.LogError("[TimeMachine] No field set for SetFieldCommand!");
            return;
        }

        if (((m_oField is PropertyInfo) && (m_oField as PropertyInfo).PropertyType != m_oNewValue.GetType()) ||
            ((m_oField is FieldInfo) && (m_oField as FieldInfo).FieldType != m_oNewValue.GetType()))
        {
            Debug.LogError("[TimeMachine] Invalid value passed in for field SetFieldCommand!");
            return;
        }


        if (m_oField is PropertyInfo)
            (m_oField as PropertyInfo).SetValue(m_oTarget, m_oNewValue, null);
        else if (m_oField is FieldInfo)
            (m_oField as FieldInfo).SetValue(m_oTarget, m_oNewValue);
    }

    /// <summary>
    /// This gets the undo command for this set field command.
    /// </summary>
    /// <returns>The new command.</returns>
    public override TMCommandInterface GetUndoCommand()
    {
        return new TMSetFieldCommand(m_oTarget, m_oField, m_oNewValue, m_oOldValue);
    }
}
