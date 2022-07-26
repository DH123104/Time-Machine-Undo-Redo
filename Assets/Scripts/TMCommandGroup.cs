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
/// This is a command group, which is used to contain other commands as if it was one logic command.
/// For example, it would be useful to move and rotate an object using two commands, but when performing
/// undo/redo, have them treated as one command.
/// </summary>
public class TMCommandGroup : TMAbstractCommand
{
    /// <summary>
    /// The list of commands that this group holds.
    /// </summary>
    private List<TMCommandInterface> m_oCommands = new List<TMCommandInterface>();

    /// <summary>
    /// This gets the list of commands that this group holds.
    /// </summary>
    public List<TMCommandInterface> Commands
    {
        get { return m_oCommands; }
    }

    /// <summary>
    /// This is the main constructor.
    /// </summary>
    /// <param name="szLabel">This is the label for this command group.</param>
    public TMCommandGroup(string szLabel)
    {
        this.Label = szLabel;
    }

    /// <summary>
    /// This executes this command group.
    /// </summary>
    public override void Execute()
    {
        for (int i = 0; i < m_oCommands.Count; i++)
        {
            m_oCommands[i].Execute();
        }
    }

    /// <summary>
    /// This gets the undo command group for this command group.
    /// </summary>
    /// <returns>The undo command group.</returns>
    public override TMCommandInterface GetUndoCommand()
    {
        TMCommandGroup undoCommandGroup = new TMCommandGroup(Label);
        for (int i = m_oCommands.Count - 1; i >= 0; i--)
        {
            undoCommandGroup.Commands.Add(m_oCommands[i].GetUndoCommand());
        }
        return undoCommandGroup;
    }
}
