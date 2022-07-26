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
/// This is the interface for a command.
/// </summary>
public interface TMCommandInterface
{
    /// <summary>
    /// This is the label that displays to indicate what this command is doing.
    /// </summary>
    string Label
    {
        get;
        set;
    }

    /// <summary>
    /// This executes this command.
    /// </summary>
    void Execute();

    /// <summary>
    /// This gets the undo command for this command.
    /// </summary>
    /// <returns></returns>
    TMCommandInterface GetUndoCommand();
}
