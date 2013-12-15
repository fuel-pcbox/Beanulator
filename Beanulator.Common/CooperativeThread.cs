// beanulator's code is licensed under the 4 clause BSD license:
//
// Copyright (c) 2013, beannaich
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by beannaich.
// 4. Neither the name of beanulator nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Runtime.InteropServices;

namespace Beanulator.Common
{
#if __WINDOWS__

    public delegate void CooperativeThreadProc(object lpParameter);

    public sealed class CooperativeThread
    {
        private static CooperativeThread mainThread;

        private Action Action;
        private uint Id;

        public CooperativeThread(Action action)
        {
            this.Action = action;
            this.Id = CreateFiber(0, EntryPoint, action);
        }
        public CooperativeThread(uint id)
        {
            this.Action = null;
            this.Id = id;
        }

        private static void EntryPoint(object lpParameter)
        {
            ((Action)lpParameter)();
        }

        #region P/Invoke

        [DllImport("Kernel32.dll")] internal static extern uint CreateFiber(uint dwStackSize, CooperativeThreadProc lpStartAddress, object lpParameter = null);
        [DllImport("Kernel32.dll")] internal static extern uint ConvertThreadToFiber(object lpParameter = null);
        [DllImport("Kernel32.dll")] internal static extern void DeleteFiber(uint id);
        [DllImport("Kernel32.dll")] internal static extern void SwitchToFiber(uint id);

        #endregion

        public static void Abort()
        {
            DeleteFiber(mainThread.Id);
            mainThread = null;
        }
        public static void Start()
        {
            mainThread = new CooperativeThread(ConvertThreadToFiber());
            SwitchToFiber(mainThread.Id);
        }

        public void Enter() { SwitchToFiber(Id); }
        public void Leave() { SwitchToFiber(mainThread.Id); }
    }

#endif
#if __ANDROID__

#error "No CooperativeThread implementation available for Android."

	public sealed class CooperativeThread
	{
		public CooperativeThread(Action action) { }
		public CooperativeThread(uint id) { }

		public static void Abort() { }
		public static void Start() { }

		public void Enter() { }
		public void Leave() { }
	}

#endif
#if __APPLE__

#error "No CooperativeThread implementation available for Apple."

	public sealed class CooperativeThread
	{
		public CooperativeThread(Action action) { }
		public CooperativeThread(uint id) { }

		public static void Abort() { }
		public static void Start() { }

		public void Enter() { }
		public void Leave() { }
	}

#endif
#if __LINUX__

#error "No CooperativeThread implementation available for Linux."

	public sealed class CooperativeThread
	{
		public CooperativeThread(Action action) { }
		public CooperativeThread(uint id) { }

		public static void Abort() { }
		public static void Start() { }

		public void Enter() { }
		public void Leave() { }
	}

#endif
}