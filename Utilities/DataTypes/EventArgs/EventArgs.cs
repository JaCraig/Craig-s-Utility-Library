/*
Copyright (c) 2012 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

#region Usings

#endregion

namespace Utilities.DataTypes.EventArgs
{
    #region Event Args

    /// <summary>
    /// Base event args for the events used in the system
    /// </summary>
    public class BaseEventArgs : System.EventArgs
    {
        /// <summary>
        /// Should the event be stopped?
        /// </summary>
        public bool Stop { get; set; }
        /// <summary>
        /// Content of the event
        /// </summary>
        public object Content { get; set; }
    }

    /// <summary>
    /// Saved event args
    /// </summary>
    public class SavedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Saving event args
    /// </summary>
    public class SavingEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Deleted event args
    /// </summary>
    public class DeletedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Deleting event args
    /// </summary>
    public class DeletingEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Changed event args
    /// </summary>
    public class ChangedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Loaded event args
    /// </summary>
    public class LoadedEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// Loading event args
    /// </summary>
    public class LoadingEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On start event args
    /// </summary>
    public class OnStartEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On end event args
    /// </summary>
    public class OnEndEventArgs : BaseEventArgs
    {
    }

    /// <summary>
    /// On error event args
    /// </summary>
    public class OnErrorEventArgs : BaseEventArgs
    {
    }

    #endregion
}