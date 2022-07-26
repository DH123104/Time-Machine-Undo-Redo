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

#region Event Classes

/// <summary>
/// This enumeration contains the different events that can be listened to for the command history.
/// </summary>
public enum TMCommandEvents
{
    /// <summary>
    /// This indicates a "Do" event.
    /// </summary>
    Do,

    /// <summary>
    /// This indicates a "Undo" event.
    /// </summary>
    Undo,

    /// <summary>
    /// This indicates a "Redo" event.
    /// </summary>
    Redo,

    /// <summary>
    /// This indicates a "Clear" event.
    /// </summary>
    Clear
}

/// <summary>
/// This delegate is used to listen to the events in a command history.
/// </summary>
/// <param name="sender">The command history that this event occurred on.</param>
/// <param name="commandEvent">The event that was proccesed.</param>
public delegate void TMCommandHistoryEventHandler(TMCommandHistory sender, TMCommandEvents commandEvent);

/// <summary>
/// This delegate is used to listen to the commands executed in a command history.
/// </summary>
/// <param name="command">The command that was exectued.</param>
/// <param name="commandEvent">The command event that executed this command.</param>
public delegate void TMCommandExecutedEventHandler(TMCommandInterface command, TMCommandEvents commandEvent);

#endregion

/// <summary>
/// This class is used to manage all events and provide functions to do/undo/redo/clear commands.
/// </summary>
public class TMCommandHistory
{
    private Stack<TMCommandInterface> m_oUndoCommands = new Stack<TMCommandInterface>();

    /// <summary>
    /// This gets the current stack of undo commands.
    /// </summary>
    public Stack<TMCommandInterface> UndoCommands 
    {
        get
        {
            return m_oUndoCommands;
        }
    }

    private Stack<TMCommandInterface> m_oRedoCommands = new Stack<TMCommandInterface>();

    /// <summary>
    /// This gets the current stackk of redo commands.
    /// </summary>
    public Stack<TMCommandInterface> RedoCommands
    {
        get
        {
            return m_oRedoCommands;
        }
    }

    private Stack<TMCommandGroup> m_oCommandGroupStack = new Stack<TMCommandGroup>();

    /// <summary>
    /// This indicates if this command history can currently Undo a command.
    /// </summary>
    public bool CanUndo
    {
        get
        {
            return m_oUndoCommands.Count > 0 && m_oCommandGroupStack.Count <= 0;
        }
    }

    /// <summary>
    /// This indicates if this command history can currently Redo a command.
    /// </summary>
    public bool CanRedo
    {
        get
        {
            return m_oRedoCommands.Count > 0 && m_oCommandGroupStack.Count <= 0;
        }
    }

    /// <summary>
    /// This event is triggered when the command history has changed.
    /// </summary>
    public event TMCommandHistoryEventHandler CommandHistoryChanged = null;

    /// <summary>
    /// This event is triggered when the command history has executed a command.
    /// </summary>
    public event TMCommandExecutedEventHandler CommandExecuted = null;

    /// <summary>
    /// This pushes/adds a command group.  This should be used if you want a group of commands
    /// to be treated as the same command for undo/redo.  For example, moving and rotating
    /// a game object could be treated as one command using this, even though it takes
    /// two commands to execute.
    /// </summary>
    /// <param name="szLabel">This is the label for this command group.</param>
    public void PushCommandGroup(string szLabel)
    {
        TMCommandGroup newCommandGroup = new TMCommandGroup(szLabel);

        //if stack is not empty, add to current command group
        if (m_oCommandGroupStack.Count > 0)
        {
            m_oCommandGroupStack.Peek().Commands.Add(newCommandGroup);
        }

        //add to stack
        m_oCommandGroupStack.Push(newCommandGroup);
    }

    /// <summary>
    /// This pops/removes a command group that is currently active, and executes all the commands in it.
    /// </summary>
    public void PopCommandGroup()
    {
        if (m_oCommandGroupStack.Count > 0)
        {
            TMCommandGroup commandGroup = m_oCommandGroupStack.Pop();

            //if there are no more command groups in the stack, add it to the undo list
            if (m_oCommandGroupStack.Count <= 0 && commandGroup.Commands.Count > 0)
            {
                Do(commandGroup);
                //m_oUndoCommands.Push(commandGroup);
            }
        }
    }

    /// <summary>
    /// This executes a command.
    /// </summary>
    /// <param name="command">The command to be executed.</param>
    public void Do(TMCommandInterface command)
    {
        //execute command
        if (!(command is TMCommandGroup))
            command.Execute();

        //add to either the command group or the undo commands
        if (m_oCommandGroupStack.Count > 0)
        {
            m_oCommandGroupStack.Peek().Commands.Add(command);
        }
        else
        {
            m_oUndoCommands.Push(command.GetUndoCommand());
        }

        //wipe out redo
        m_oRedoCommands.Clear();

        if (CommandHistoryChanged != null)
        {
            CommandHistoryChanged(this, TMCommandEvents.Do);
        }

        if (CommandExecuted != null)
        {
            CommandExecuted(command, TMCommandEvents.Do);
        }
    }

    /// <summary>
    /// This perfoms an undo using the first command on the undo stack.
    /// </summary>
    public void Undo()
    {
        if (CanUndo)
        {
            TMCommandInterface command = null;
            do
            {
                command = m_oUndoCommands.Pop();
                command.Execute();
                m_oRedoCommands.Push(command.GetUndoCommand());
            }
            while (false);//command.IsHidden == true);

            if (CommandHistoryChanged != null)
            {
                CommandHistoryChanged(this, TMCommandEvents.Undo);
            }

            if (CommandExecuted != null)
            {
                if (command is TMCommandGroup)
                {
                    TMCommandGroup group = command as TMCommandGroup;
                    for (int i = 0; i < group.Commands.Count; i++)
                    {
                        CommandExecuted(group.Commands[i], TMCommandEvents.Undo);
                    }
                }
                else
                {
                    CommandExecuted(command, TMCommandEvents.Undo);
                }
            }
        }
    }

    /// <summary>
    /// This perfoms an redo using the first command on the undo stack.
    /// </summary>
    public void Redo()
    {
        if (CanRedo)
        {
            TMCommandInterface command = null;
            do
            {
                command = m_oRedoCommands.Pop();
                command.Execute();
                m_oUndoCommands.Push(command.GetUndoCommand());
            }
            while (false);//m_oRedoCommands.Count > 0 && m_oRedoCommands.Peek().IsHidden == true);

            if (CommandHistoryChanged != null)
            {
                CommandHistoryChanged(this, TMCommandEvents.Redo);
            }

            if (CommandExecuted != null)
            {
                if (command is TMCommandGroup)
                {
                    TMCommandGroup group = command as TMCommandGroup;
                    for (int i = 0; i < group.Commands.Count; i++)
                    {
                        CommandExecuted(group.Commands[i], TMCommandEvents.Redo);
                    }
                }
                else
                {
                    CommandExecuted(command, TMCommandEvents.Redo);
                }
            }
        }
    }

    /// <summary>
    /// This clears all commands from this command history.
    /// </summary>
    public void Clear()
    {
        m_oCommandGroupStack.Clear();
        m_oRedoCommands.Clear();
        m_oUndoCommands.Clear();

        if (CommandHistoryChanged != null)
        {
            CommandHistoryChanged(this, TMCommandEvents.Clear);
        }
    }
}
