#region Copyright 2009 Robert Konklewski
/*
 * CursesSharp
 * 
 * Copyright 2009 Robert Konklewski
 * 
 * This library is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at your
 * option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * www.gnu.org/licenses/>.
 * 
 */
#endregion

using System;
using System.Runtime.InteropServices;

namespace CursesSharp.Internal
{
    internal static partial class CursesMethods
    {
        internal static void waddchnstr(IntPtr win, uint[] chstr, int n)
        {
            int ret = wrap_waddchnstr(win, chstr, n);
            InternalException.Verify(ret, "waddchnstr");
        }

        internal static void mvwaddchnstr(IntPtr win, int y, int x, uint[] chstr, int n)
        {
            int ret = wrap_mvwaddchnstr(win, y, x, chstr, n);
            InternalException.Verify(ret, "mvwaddchnstr");
        }
        
        internal static void mvwaddwchnstr(IntPtr win, int y, int x, CChar[] chstr, int n)
        {
            int ret = wrap_mvwadd_wchnstr(win, y, x, chstr, n);
            InternalException.Verify(ret, "mvwaddchnstr");
        }

        [DllImport("CursesWrapper")]
        private static extern int wrap_waddchnstr(IntPtr win, uint[] chstr, int n);
        [DllImport("CursesWrapper")]
        private static extern int wrap_mvwaddchnstr(IntPtr win, int y, int x, uint[] chstr, int n);
        [DllImport("CursesWrapper")]
        private static extern int wrap_mvwadd_wchnstr(IntPtr win, int y, int x, CChar[] chstr, int n);
    }
}
