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
        internal static void waddch(IntPtr win, uint ch)
        {
            int ret = wrap_waddch(win, ch);
            InternalException.Verify(ret, "waddch");
        }
        
        internal static void waddwch(IntPtr win, ushort ch)
        {
            int ret = wrap_wadd_wch(win, ch);
            InternalException.Verify(ret, "waddch");
        }

        internal static void mvwaddch(IntPtr win, int y, int x, uint ch)
        {
            int ret = wrap_mvwaddch(win, y, x, ch);
            InternalException.Verify(ret, "mvwaddch");
        }

        internal static void wechochar(IntPtr win, uint ch)
        {
            int ret = wrap_wechochar(win, ch);
            InternalException.Verify(ret, "wechochar");
        }

        [DllImport("CursesWrapper")]
        private static extern int wrap_wechochar(IntPtr win, uint ch);
        [DllImport("CursesWrapper")]
        private static extern int wrap_waddch(IntPtr win, uint ch);
        [DllImport("CursesWrapper")]
        private static extern int wrap_mvwaddch(IntPtr win, int y, int x, uint ch);
        [DllImport("CursesWrapper")]
        private static extern int wrap_wadd_wch(IntPtr win, ushort ch);
    }
}
