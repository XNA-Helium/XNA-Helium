using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Engine
{
    public static class BinaryReaderExtension
    {

        public static Matrix ReadMatrix(this BinaryReader reader)
        {
            Matrix matrix = new Matrix();
            matrix.M11 = reader.ReadSingle();
            matrix.M12 = reader.ReadSingle();
            matrix.M13 = reader.ReadSingle();
            matrix.M14 = reader.ReadSingle();
            matrix.M21 = reader.ReadSingle();
            matrix.M22 = reader.ReadSingle();
            matrix.M23 = reader.ReadSingle();
            matrix.M24 = reader.ReadSingle();
            matrix.M31 = reader.ReadSingle();
            matrix.M32 = reader.ReadSingle();
            matrix.M33 = reader.ReadSingle();
            matrix.M34 = reader.ReadSingle();
            matrix.M41 = reader.ReadSingle();
            matrix.M42 = reader.ReadSingle();
            matrix.M43 = reader.ReadSingle();
            matrix.M44 = reader.ReadSingle();
            return matrix;
        }
        public static Quaternion ReadQuaternion( this BinaryReader reader )
        {
            Quaternion quaternion = new Quaternion();
            quaternion.X = reader.ReadSingle();
            quaternion.Y = reader.ReadSingle();
            quaternion.Z = reader.ReadSingle();
            quaternion.W = reader.ReadSingle();
            return quaternion;
        }
        public static Vector2 ReadVector2( this BinaryReader reader )
        {
            Vector2 vector = new Vector2();
            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            return vector;
        }
        public static Vector3 ReadVector3( this BinaryReader reader )
        {
            Vector3 vector = new Vector3();
            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
            return vector;
        }
        public static Vector4 ReadVector4( this BinaryReader reader )
        {
            Vector4 vector = new Vector4();
            vector.X = reader.ReadSingle();
            vector.Y = reader.ReadSingle();
            vector.Z = reader.ReadSingle();
            vector.W = reader.ReadSingle();
            return vector;
        }
        public static Color ReadColor( this BinaryReader reader )
        {
            Color color = new Color();
            color.R = reader.ReadByte();
            color.B = reader.ReadByte();
            color.G = reader.ReadByte();
            color.A = reader.ReadByte();
            return color;
        }
        public static Rectangle ReadRectangle( this BinaryReader reader )
        {
            Rectangle rect = new Rectangle();
            rect.X = reader.ReadInt32();
            rect.Y = reader.ReadInt32();
            rect.Height = reader.ReadInt32();
            rect.Width = reader.ReadInt32();
            return rect;
        }
        public static Point ReadPoint( this BinaryReader reader )
        {
            Point point = new Point();
            point.X = reader.ReadInt32();
            point.Y = reader.ReadInt32();
            return point;
        }
    }
}