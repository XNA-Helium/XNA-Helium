using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public static class DebugHelper
    {
        public enum DebugLevels : int {Informative = 0, Trivial = 1, Curious = 2, Important = 3, Critical = 4, HighlyCritical = 5, Explosive = 6, IgnoreAll = 7 };

        public static DebugLevels MinDebugLevel = DebugLevels.Informative;

        public static bool Validate()
        {
#if WINDOWS_PHONE
            return System.Diagnostics.Debugger.IsAttached;
#else
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Launch();
            }
            return System.Diagnostics.Debugger.IsAttached;
#endif
        }

        public static void DebugBreak(DebugLevels level)
        {
#if DEBUG
            if(level >= MinDebugLevel && Validate())
                System.Diagnostics.Debugger.Break();
#endif
        }
        public static bool DebugBreak(bool condition, DebugLevels level)
        {
#if DEBUG
            if (level >= MinDebugLevel && condition && Validate())
                System.Diagnostics.Debugger.Break();
#endif
            return condition;
        }


        public static void Break(DebugLevels level )
        {
            if (level >= MinDebugLevel && Validate())
                System.Diagnostics.Debugger.Break();
        }

        public static bool Break( bool condition, DebugLevels level )
        {
            if (level >= MinDebugLevel && condition && Validate())
                System.Diagnostics.Debugger.Break();
            return condition;
        }
        public static void BreakEquatable<T>(T one, T two, DebugLevels level ) where T : IEquatable<T>
        {
            if (level >= MinDebugLevel && one.Equals(two) && Validate())
            {
                System.Diagnostics.Debugger.Break();
            }
        }
        public static void BreakComparable<T>( T one, T two, DebugLevels level ) where T : IComparable<T>
        {
            if (level > MinDebugLevel && one.CompareTo(two) == 0 && Validate())
            {
                System.Diagnostics.Debugger.Break();
            }
        }
        public static void Break( Exception ex, DebugLevels level )
        {
            Engine.DebugHelper.Break(level);
            Engine.FileMananger.Instance.WriteException(ex);
#if DEBUG
            throw ex;
#endif
        }
        public static void Break( string ex, DebugLevels level )
        {
            Engine.DebugHelper.Break(level);
            if(level > MinDebugLevel)
                throw new Exception(ex);
        }
    }
}
