using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine
{
    public static class BinaryWriterExtension
    {
        public static void Write(this BinaryWriter writer, Matrix value)
        {
            writer.Write(value.M11);
            writer.Write(value.M12);
            writer.Write(value.M13);
            writer.Write(value.M14);
            writer.Write(value.M21);
            writer.Write(value.M22);
            writer.Write(value.M23);
            writer.Write(value.M24);
            writer.Write(value.M31);
            writer.Write(value.M32);
            writer.Write(value.M33);
            writer.Write(value.M34);
            writer.Write(value.M41);
            writer.Write(value.M42);
            writer.Write(value.M43);
            writer.Write(value.M44);
        }
        public static void Write( this BinaryWriter writer, Quaternion value )
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }
        public static void Write( this BinaryWriter writer, Vector2 value )
        {
            writer.Write(value.X);
            writer.Write(value.Y);
        }
        public static void Write( this BinaryWriter writer, Vector3 value )
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
        }
        public static void Write( this BinaryWriter writer, Vector4 value )
        {
            writer.Write(value.X);
            writer.Write(value.Y);
            writer.Write(value.Z);
            writer.Write(value.W);
        }

        public static void Write( this BinaryWriter writer, Color value )
        {
            writer.Write( value.R );
            writer.Write( value.B );
            writer.Write( value.G );
            writer.Write( value.A );
        }

        public static void Write( this BinaryWriter writer, ref Rectangle value )
        {
            writer.Write( value.X );
            writer.Write( value.Y );
            writer.Write( value.Height );
            writer.Write( value.Width );
        }

        public static void Write( this BinaryWriter writer, ref Point value )
        {
            writer.Write( value.X );
            writer.Write( value.Y );
        }
    }
}