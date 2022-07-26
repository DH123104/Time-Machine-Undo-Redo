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

/// <summary>
/// This provides a base abstract implementation of the command interface.
/// </summary>
public abstract class TMAbstractCommand : TMCommandInterface
{
    #region TMCommandInterface Members

    /// <summary>
    /// This is the label.
    /// </summary>
    private string m_szLabel = "";

    /// <summary>
    /// This gets/sets the label for this command, useful if showing commands in a visible way.
    /// </summary>
    public string Label
    {
        get
        {
            return m_szLabel;
        }
        set
        {
            m_szLabel = value;
        }
    }

    /// <summary>
    /// This executes this command.
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// This gets the undo command for this command.
    /// </summary>
    /// <returns>The undo command.</returns>
    public abstract TMCommandInterface GetUndoCommand();

    #endregion
}
