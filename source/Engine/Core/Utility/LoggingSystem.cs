using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

namespace Engine
{
#if DEBUG
    public class LoggingSystem
    {
        public enum LoggingLevels : int { Informative = 0, Trivial = 1, Curious = 2, Important = 3, Critical = 4, HighlyCritical = 5, Explosive = 6, IgnoreAll = 7 };
        public static LoggingLevels MinLoggingLevel = LoggingLevels.Informative;

        protected static LoggingSystem instance;

        public static LoggingSystem Instance
        {
            get
            {
                if (instance == null)
                    instance = new LoggingSystem();
                return instance;
            }
        }

        protected LoggingSystem()
        {
        }

        ~LoggingSystem()
        {
        }

        protected void WriteErrorData(string data)
        {
            if(System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debug.WriteLine(data);
        }

        protected void WriteData(string data)
        {
            //Does nothing
        }

        public void DebugLog(string logme, LoggingLevels level)
        {
#if DEBUG
            if (level >= MinLoggingLevel)
                WriteErrorData(logme);
            else
                WriteData(logme);
#endif
        }
        public void DebugLog(string text, Engine.Core.BaseObject entity, LoggingLevels level)
        {
#if DEBUG
            if (level >= MinLoggingLevel)
            {
                WriteErrorData(text + " " + entity.Type.ToString() + " " + entity.ID + " " + entity.InWorld.ToString());
            }
            else
            {
                WriteData(text + " " +  entity.Type.ToString() + " " + entity.ID + " " + entity.InWorld.ToString());
            }
#endif
        }
        public void DebugLog(string text, Engine.Core.BaseComponent component, LoggingLevels level)
        {
#if DEBUG
            if (level >= MinLoggingLevel)
            {
                WriteErrorData(text + " " + component.Type.ToString() + " " + component.ID + " " + component.AddToWorld + " Parent:" + component.Parent.ID);
            }
            else
            {
                WriteData(text + " " + component.Type.ToString() + " " + component.ID + " " + component.AddToWorld + " Parent:" + component.Parent.ID);
            }

            #endif
        }
        public bool DebugLog(bool condition, string logme, LoggingLevels level)
        {
#if DEBUG
            if (condition)
                Log(logme, level);
#endif
            return condition;
        }


        public void Log(string logme, LoggingLevels level)
        {
            if (level >= MinLoggingLevel)
                WriteErrorData(logme);
            else
                WriteData(logme);
        }

        public void Log(string text, Engine.Core.BaseObject entity, LoggingLevels level)
        {
            if (level >= MinLoggingLevel)
            {
                WriteErrorData(text + " " + entity.GetType().ToString() + " " + entity.ID + " " + entity.InWorld.ToString());
            }
            else
            {
                WriteData(text + " " + entity.GetType().ToString() + " " + entity.ID + " " + entity.InWorld.ToString());
            }
        }

        public void Log(string text, Engine.Core.BaseComponent component, LoggingLevels level)
        {
            if (level >= MinLoggingLevel)
            {
                WriteErrorData(text + " " + component.GetType().ToString() + " " + component.ID + " " + component.AddToWorld + " Parent:" + component.Parent.ID);
            }
            else
            {
                WriteData(text + " " + component.GetType().ToString() + " " + component.ID + " " + component.AddToWorld + " Parent:" + component.Parent.ID);
            }
        }

        public bool Log(bool condition, string logme, LoggingLevels level)
        {
            if (condition)
                Log(logme, level);

            return condition;
        }
    }
#endif
}
