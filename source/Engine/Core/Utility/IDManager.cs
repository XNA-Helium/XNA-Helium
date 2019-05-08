using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Core
{
    /// <summary>
    /// ID Manager required for Save/Load 
    /// TypeOf( T ) still kept around as it's very useful
    /// </summary>
    static class IDManager
    {
        public static float InstanceID = 10000.0f;
        public static int ID = 0;
        public static int MenuID = 0;
        static System.Collections.Generic.Dictionary<int, Type> IntList = new Dictionary<int, Type>( 64 );
        static System.Collections.Generic.Dictionary<Type, int> TypeList = new Dictionary<Type, int>( 64 );
        static System.Collections.Generic.Dictionary<int, IFactory> ObjectList = new Dictionary<int, IFactory>( 64 );
        static System.Collections.Generic.Dictionary<int, Engine.UI.ReturnMenu> MenuList = new Dictionary<int, Engine.UI.ReturnMenu>(64);
        static System.Collections.Generic.Dictionary<Type, int> ReverseMenuList = new Dictionary<Type, int>(64);
        public static int GetNextID<T>( ReturnsNew retnewdel ) where T : Factoryable
        {
            AddClass<T>( retnewdel );
            return GetNextID();
        }
        public static int GetNextID()
        {
            return ++ID;
        }
        internal static void SetNextInstanceID(float value)
        {
            InstanceID = value;
        }
        public static float GetNextInstanceID( )
        {
            return ++InstanceID;
        }
        public static int GetNextMenuID()
        {
            return ++MenuID;
        }
        public static Object ReturnClass( int id, bool InWorld, float uID )
        {
            return ObjectList[id].ReturnNew( InWorld,uID );
        }
        public static Object ReturnClass( int id )
        {
            return ObjectList[id].ReturnNew(false, -1);
        }
        public static Type MapType( int type )
        {
            if ( IntList.ContainsKey( type ) )
            {
                return IntList[type];
            }
            else
            {
                DebugHelper.Break( DebugHelper.DebugLevels.HighlyCritical );
                return null;
            }
        }
        public static int MapType( Type type )
        {
            DebugHelper.Break(type == null,DebugHelper.DebugLevels.Critical);
            if ( TypeList.ContainsKey( type ) )
            {
                return TypeList[type];
            }
            else
            {
                DebugHelper.Break( DebugHelper.DebugLevels.HighlyCritical );
                return -1;
            }
        }
        public static void AddClass<T>(ReturnsNew retnewdel ) where T : Factoryable
        {
            Factory<T> f = new Factory<T>(retnewdel);
            ObjectList.Add( f.IntType, f );
            TypeList.Add( f.Type, f.IntType );
            IntList.Add( f.IntType, f.Type );
#if DEBUG
            LoggingSystem.Instance.Log(f.Type.ToString() + " = " + f.IntType,LoggingSystem.LoggingLevels.Explosive);
#endif
        }

        public static void GenerateIDs()
        {
            AddClass < DrawableText>( DrawableText.ReturnNew );
            AddClass < Drawable25D>( Drawable2D.ReturnNew);
            AddClass < Drawable3D>( Drawable3D.ReturnNew);
            AddClass < Drawable2D>( Drawable2D.ReturnNew );

            AddClass < Physics2D>( Physics2D.ReturnNew);
            AddClass < Placeable>( Placeable.ReturnNew);

            AddClass < TeamMember>( TeamMember.ReturnNew);

            AddClass < HitPoints> ( HitPoints.ReturnNew);

            //AddClass< BaseObject>( BaseObject.ReturnNew );
        }


        internal static void AddNewMenu(int id, Engine.UI.ReturnMenu menu, Type t)
        {
            MenuList.Add(id, menu);
            ReverseMenuList.Add(t, id);
        }

        public static Engine.UI.BaseMenu GetNewMenu(int id)
        {
            return MenuList[id]();
        }
        public static int GetMenuID(Type t)
        {
            return ReverseMenuList[t];
        }

    }
}
