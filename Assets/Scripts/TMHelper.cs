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

/// <summary>
/// This is a helper class used to help use the command history and generic commands.
/// </summary>
public static class TMHelper
{
    private static TMCommandHistory m_oGlobalCommandHistory = null;
        
    /// <summary>
    /// This gets/sets the global command history class.
    /// </summary>
    public static TMCommandHistory CommandHistory
    {
        get
        {
            if (m_oGlobalCommandHistory == null)
                m_oGlobalCommandHistory = new TMCommandHistory();
            return m_oGlobalCommandHistory;
        }
        set
        {
            m_oGlobalCommandHistory = value;
        }
    }

    /// <summary>
    /// This sets the field/variable of a target component.
    /// </summary>
    /// <param name="target">This is the target component to set the field/variable on.</param>
    /// <param name="field">This is the field/variable name to set.</param>
    /// <param name="value">This is the value to set the field/variable to.</param>
    /// <returns>This indicates if the field was set successfully.</returns>
    public static bool SetField(Component target, string field, object value)
    {
        TMSetFieldCommand command = new TMSetFieldCommand(target, field, value);
        if (!command.IsValid)
            return false;
        CommandHistory.Do(command);
        return true;
    }
}
