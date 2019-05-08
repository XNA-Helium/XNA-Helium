using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine;
using Engine.Core;

namespace Engine.Persistence
{
    public enum Mode { Read, Write };

    public class PersistenceManager
    {
        float CurrentVal = 4.5f;

        List<float> Order = new List<float>();
        System.Collections.Generic.Dictionary<float,BaseObject> Objects = new Dictionary<float,BaseObject>();
        
        protected System.IO.Stream stream = null;
        protected System.IO.BinaryReader binaryReader = null;
        protected System.IO.BinaryWriter binaryWriter = null;

        protected Mode Mode;        
        
        public PersistenceManager(System.IO.Stream Stream, Mode mode)
        {
            Mode = mode;
            stream = Stream;

            if (mode == Persistence.Mode.Read)
            {
                binaryReader = new System.IO.BinaryReader(stream);
                DebugHelper.Break(CurrentVal != binaryReader.ReadSingle(), DebugHelper.DebugLevels.Explosive);
            }
            if (mode == Persistence.Mode.Write)
            {
                binaryWriter = new System.IO.BinaryWriter(stream);
                binaryWriter.Write(CurrentVal);
            }
        }

        public System.IO.BinaryReader BinaryReader
        {
            get
            {
                DebugHelper.Break(Mode != Persistence.Mode.Read, DebugHelper.DebugLevels.Critical);
#if DEBUG
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
                LoggingSystem.Instance.Log(sf.GetMethod().ReflectedType.Name + " " + sf.GetMethod() + " " + binaryReader.BaseStream.Position, LoggingSystem.LoggingLevels.Curious);
#endif
                return binaryReader;
            }
        }

        public System.IO.BinaryWriter BinaryWriter
        {
            get
            {
                DebugHelper.Break(Mode != Persistence.Mode.Write, DebugHelper.DebugLevels.Critical);
#if DEBUG
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
                LoggingSystem.Instance.Log(sf.GetMethod().ReflectedType.Name + " " + sf.GetMethod() + " " + binaryWriter.BaseStream.Position, LoggingSystem.LoggingLevels.Curious);
#endif
                return binaryWriter;
            }
        }

        public System.IO.Stream Stream
        {
            get
            {
                return stream;
            }
        }

        public BaseObject this[float ID]
        {
            get
            {
                Order.Add(ID);
                return Objects[ID];
            }
            set
            {
                if (value == null)
                {
                    Order.Remove(ID);
                }
                Objects[ID] = value;
            }
        }
    }
}
