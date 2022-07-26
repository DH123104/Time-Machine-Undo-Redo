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
/// This behavior contains a command history.
/// </summary>
[AddComponentMenu("Time Machine/Command History")]
public class TMComanndHistoryBehavior : MonoBehaviour
{
    /// <summary>
    /// The command history.
    /// </summary>
    private TMCommandHistory m_oCommandHistory = new TMCommandHistory();
    
    /// <summary>
    /// This gets the command history.
    /// </summary>
    public TMCommandHistory CommandHistory
    {
        get { return m_oCommandHistory; }
    }

    /// <summary>
    /// This is called when the behavior is reset.
    /// </summary>
    public void Reset()
    {
        m_oCommandHistory.Clear();
    }

    /// <summary>
    /// This is called when the behavior is awoken.
    /// </summary>
    public void Awake()
    {
        m_oCommandHistory.Clear();
        TMHelper.CommandHistory = m_oCommandHistory;
    }
}
