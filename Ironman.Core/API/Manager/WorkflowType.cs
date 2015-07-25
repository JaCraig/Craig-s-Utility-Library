/*
Copyright (c) 2014 <a href="http://www.gutgames.com">James Craig</a>

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironman.Core.API.Manager
{
    /// <summary>
    /// Workflow type
    /// </summary>
    [Flags]
    public enum WorkflowType
    {
        /// <summary>
        /// The none
        /// </summary>
        None = 0,

        /// <summary>
        /// The pre all
        /// </summary>
        PreAll,

        /// <summary>
        /// The pre any
        /// </summary>
        PreAny,

        /// <summary>
        /// The pre delete
        /// </summary>
        PreDelete,

        /// <summary>
        /// The pre save
        /// </summary>
        PreSave,

        /// <summary>
        /// The post all
        /// </summary>
        PostAll,

        /// <summary>
        /// The post any
        /// </summary>
        PostAny,

        /// <summary>
        /// The post delete
        /// </summary>
        PostDelete,

        /// <summary>
        /// The post save
        /// </summary>
        PostSave,

        /// <summary>
        /// The pre service
        /// </summary>
        PreService,

        /// <summary>
        /// The post service
        /// </summary>
        PostService
    }
}