using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

using Engine;
using Engine.Core;

namespace Engine.Core
{
    /// <summary>
    /// A position item in a quadtree
    /// </summary>
    /// <typeparam name="T">The type of the QuadTree item's parent</typeparam>
    public class QuadTreePositionItem<T>
    {

        public delegate void MoveHandler(QuadTreePositionItem<T> positionItem);
        public delegate void DestroyHandler(QuadTreePositionItem<T> positionItem);

        public event MoveHandler Move;
        public event DestroyHandler Destroy;

        protected void OnMove()
        {
            // Update rectangles
            rect.TopLeft = position - (size * .5f);
            rect.BottomRight = position + (size * .5f);


            // Call event handler
            if (Move != null) Move(this);
        }


        protected void OnDestroy()
        {
            if (Destroy != null) Destroy(this);
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                OnMove();
            }
        }

        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
            set
            {
                size = value;
                rect.TopLeft = position - (size / 2f);
                rect.BottomRight = position + (size / 2f);
                OnMove();
            }
        }


        private FRect rect;
        public FRect Rect
        {
            get { return rect; }
        }

        private T parent;


        public T Parent
        {
            get { return parent; }
        }



        public QuadTreePositionItem(T parent, Vector2 position, Vector2 size)
        {
            this.rect = new FRect(0f, 0f, 1f, 1f);

            this.parent = parent;
            this.position = position;
            this.size = size;
            OnMove();
        }

        public void Delete()
        {
            OnDestroy();
        }

    }


    public class QuadTreeNode<T>
    {
        public delegate void ResizeDelegate(FRect newSize);

        protected FRect rect;


        public FRect Rect
        {
            get { return rect; }
            protected set { rect = value; }
        }


        protected int MaxItems;
        protected bool IsPartitioned;
        protected QuadTreeNode<T> ParentNode;
        protected QuadTreeNode<T> TopLeftNode;
        protected QuadTreeNode<T> TopRightNode;
        protected QuadTreeNode<T> BottomLeftNode;
        protected QuadTreeNode<T> BottomRightNode;
        protected List<QuadTreePositionItem<T>> Items;
        protected ResizeDelegate WorldResize;

        public QuadTreeNode(QuadTreeNode<T> parentNode, FRect rect, int maxItems)
        {
            ParentNode = parentNode;
            Rect = rect;
            MaxItems = maxItems;
            IsPartitioned = false;
            Items = new List<QuadTreePositionItem<T>>();
        }

        public QuadTreeNode(FRect rect, int maxItems, ResizeDelegate worldResize)
        {
            ParentNode = null;
            Rect = rect;
            MaxItems = maxItems;
            WorldResize = worldResize;
            IsPartitioned = false;
            Items = new List<QuadTreePositionItem<T>>();
        }

        public void Insert(QuadTreePositionItem<T> item)
        {
            // If partitioned, try to find child node to add to
            if (!InsertInChild(item))
            {
                item.Destroy += new QuadTreePositionItem<T>.DestroyHandler(ItemDestroy);
                item.Move += new QuadTreePositionItem<T>.MoveHandler(ItemMove);
                Items.Add(item);

                // Check if this node needs to be partitioned
                if (!IsPartitioned && Items.Count >= MaxItems)
                {
                    Partition();
                }
            }
        }

        protected bool InsertInChild(QuadTreePositionItem<T> item)
        {
            if (!IsPartitioned) return false;

            if (TopLeftNode.ContainsRect(item.Rect))
                TopLeftNode.Insert(item);
            else if (TopRightNode.ContainsRect(item.Rect))
                TopRightNode.Insert(item);
            else if (BottomLeftNode.ContainsRect(item.Rect))
                BottomLeftNode.Insert(item);
            else if (BottomRightNode.ContainsRect(item.Rect))
                BottomRightNode.Insert(item);

            else return false; // insert in child failed

            return true;
        }

        public bool PushItemDown(int i)
        {
            if (InsertInChild(Items[i]))
            {
                RemoveItem(i);
                return true;
            }

            else return false;
        }

        public void PushItemUp(int i)
        {
            QuadTreePositionItem<T> m = Items[i];

            RemoveItem(i);
            ParentNode.Insert(m);
        }

        protected void Partition()
        {
            // Create the nodes
            Vector2 MidPoint = Vector2.Divide(Vector2.Add(Rect.TopLeft, Rect.BottomRight), 2.0f);

            TopLeftNode = new QuadTreeNode<T>(this, new FRect(Rect.TopLeft, MidPoint), MaxItems);
            TopRightNode = new QuadTreeNode<T>(this, new FRect(new Vector2(MidPoint.X, Rect.Top), new Vector2(Rect.Right, MidPoint.Y)), MaxItems);
            BottomLeftNode = new QuadTreeNode<T>(this, new FRect(new Vector2(Rect.Left, MidPoint.Y), new Vector2(MidPoint.X, Rect.Bottom)), MaxItems);
            BottomRightNode = new QuadTreeNode<T>(this, new FRect(MidPoint, Rect.BottomRight), MaxItems);

            IsPartitioned = true;

            // Try to push items down to child nodes
            int i = 0;
            while (i < Items.Count)
            {
                if (!PushItemDown(i))
                {
                    i++;
                }
            }
        }

        public void GetItems(Vector2 Point, ref List<QuadTreePositionItem<T>> ItemsFound)
        {
            // test the point against this node
            if (Rect.Contains(Point))
            {
                // test the point in each item
                foreach (QuadTreePositionItem<T> Item in Items)
                {
                    if (Item.Rect.Contains(Point)) ItemsFound.Add(Item);
                }

                // query all subtrees
                if (IsPartitioned)
                {
                    TopLeftNode.GetItems(Point, ref ItemsFound);
                    TopRightNode.GetItems(Point, ref ItemsFound);
                    BottomLeftNode.GetItems(Point, ref ItemsFound);
                    BottomRightNode.GetItems(Point, ref ItemsFound);
                }
            }
        }

        /// <summary>
        /// Gets a list of items intersecting a specified rectangle
        /// </summary>
        /// <param name="Rect">The rectangle</param>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        /// <remarks>ItemsFound is assumed to be initialized, and will not be cleared</remarks>
        public void GetItems(FRect Rect, ref List<QuadTreePositionItem<T>> ItemsFound)
        {
            // test the point against this node
            if (Rect.Intersects(this.rect))
            {
                // test the point in each item
                foreach (QuadTreePositionItem<T> Item in Items)
                {
                    if (Item.Rect.Intersects(Rect)) 
                        ItemsFound.Add(Item);
                }

                // query all subtrees
                if (IsPartitioned)
                {
                    TopLeftNode.GetItems(Rect, ref ItemsFound);
                    TopRightNode.GetItems(Rect, ref ItemsFound);
                    BottomLeftNode.GetItems(Rect, ref ItemsFound);
                    BottomRightNode.GetItems(Rect, ref ItemsFound);
                }
            }
        }

        /// <summary>
        /// Gets a list of all items within this node
        /// </summary>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        /// <remarks>ItemsFound is assumed to be initialized, and will not be cleared</remarks>
        public void GetAllItems(ref List<QuadTreePositionItem<T>> ItemsFound)
        {
            ItemsFound.AddRange(Items);

            // query all subtrees
            if (IsPartitioned)
            {
                TopLeftNode.GetAllItems(ref ItemsFound);
                TopRightNode.GetAllItems(ref ItemsFound);
                BottomLeftNode.GetAllItems(ref ItemsFound);
                BottomRightNode.GetAllItems(ref ItemsFound);
            }
        }

        /// <summary>
        /// Finds the node containing a specified item
        /// </summary>
        /// <param name="Item">The item to find</param>
        /// <returns>The node containing the item</returns>
        public QuadTreeNode<T> FindItemNode(QuadTreePositionItem<T> Item)
        {
            if (Items.Contains(Item)) return this;

            else if (IsPartitioned)
            {
                QuadTreeNode<T> n = null;

                // Check the nodes that could contain the item
                if (TopLeftNode.ContainsRect(Item.Rect))
                {
                    n = TopLeftNode.FindItemNode(Item);
                }
                if (n == null &&
                    TopRightNode.ContainsRect(Item.Rect))
                {
                    n = TopRightNode.FindItemNode(Item);
                }
                if (n == null &&
                    BottomLeftNode.ContainsRect(Item.Rect))
                {
                    n = BottomLeftNode.FindItemNode(Item);
                }
                if (n == null &&
                    BottomRightNode.ContainsRect(Item.Rect))
                {
                    n = BottomRightNode.FindItemNode(Item);
                }

                return n;
            }

            else return null;
        }

        /// <summary>
        /// Destroys this node
        /// </summary>
        public void Destroy()
        {
            // Destroy all child nodes
            if (IsPartitioned)
            {
                TopLeftNode.Destroy();
                TopRightNode.Destroy();
                BottomLeftNode.Destroy();
                BottomRightNode.Destroy();

                TopLeftNode = null;
                TopRightNode = null;
                BottomLeftNode = null;
                BottomRightNode = null;
            }

            // Remove all items
            while (Items.Count > 0)
            {
                RemoveItem(0);
            }
        }

        /// <summary>
        /// Removes an item from this node
        /// </summary>
        /// <param name="item">The item to remove</param>
        public void RemoveItem(QuadTreePositionItem<T> item)
        {
            // Find and remove the item
            if (Items.Contains(item))
            {
                item.Move -= new QuadTreePositionItem<T>.MoveHandler(ItemMove);
                item.Destroy -= new QuadTreePositionItem<T>.DestroyHandler(ItemDestroy);
                Items.Remove(item);
            }
        }

        /// <summary>
        /// Removes an item from this node at a specific index
        /// </summary>
        /// <param name="i">the index of the item to remove</param>
        protected void RemoveItem(int i)
        {
            if (i < Items.Count)
            {
                Items[i].Move -= new QuadTreePositionItem<T>.MoveHandler(ItemMove);
                Items[i].Destroy -= new QuadTreePositionItem<T>.DestroyHandler(ItemDestroy);
                Items.RemoveAt(i);
            }
        }

        /// <summary>
        /// Handles item movement
        /// </summary>
        /// <param name="item">The item that moved</param>
        public void ItemMove(QuadTreePositionItem<T> item)
        {
            // Find the item
            if (Items.Contains(item))
            {
                int i = Items.IndexOf(item);

                // Try to push the item down to the child
                if (!PushItemDown(i))
                {
                    // otherwise, if not root, push up
                    if (ParentNode != null)
                    {
                        PushItemUp(i);
                    }
                    else if (!ContainsRect(item.Rect))
                    {
                        WorldResize(new FRect(
                             Vector2.Min(Rect.TopLeft, item.Rect.TopLeft) * 2,
                             Vector2.Max(Rect.BottomRight, item.Rect.BottomRight) * 2));
                    }

                }
            }
            else
            {
                // this node doesn't contain that item, stop notifying it about it
                item.Move -= new QuadTreePositionItem<T>.MoveHandler(ItemMove);
            }
        }

        /// <summary>
        /// Handles item destruction
        /// </summary>
        /// <param name="item">The item that is being destroyed</param>
        public void ItemDestroy(QuadTreePositionItem<T> item)
        {
            RemoveItem(item);
        }

        /// <summary>
        /// Tests whether this node contains a rectangle
        /// </summary>
        /// <param name="rect">The rectangle to test</param>
        /// <returns>Whether or not this node contains the specified rectangle</returns>
        public bool ContainsRect(FRect rect)
        {
            return (rect.TopLeft.X >= Rect.TopLeft.X &&
                    rect.TopLeft.Y >= Rect.TopLeft.Y &&
                    rect.BottomRight.X <= Rect.BottomRight.X &&
                    rect.BottomRight.Y <= Rect.BottomRight.Y);
        }
    }

    /// <summary>
    /// A QuadTree for partitioning a space into rectangles
    /// </summary>
    /// <typeparam name="T">The type of the QuadTree's items' parents</typeparam>
    /// <remarks>This QuadTree automatically resizes as needed</remarks>
    public class QuadTree<T>
    {

        /// <summary>
        /// The head node of the QuadTree
        /// </summary>
        protected QuadTreeNode<T> rootNode;

        /// <summary>
        /// Gets the world rectangle
        /// </summary>
        public FRect WorldRect
        {
            get { return rootNode.Rect; }
        }

        /// <summary>
        /// The maximum number of items in any node before partitioning
        /// </summary>
        protected int maxItems;


        /// <summary>
        /// QuadTree constructor
        /// </summary>
        /// <param name="worldRect">The world rectangle for this QuadTree (a rectangle containing all items at all times)</param>
        /// <param name="maxItems">Maximum number of items in any cell of the QuadTree before partitioning</param>
        public QuadTree(FRect worldRect, int maxItems)
        {
            this.rootNode = new QuadTreeNode<T>(worldRect, maxItems, Resize);
            this.maxItems = maxItems;
        }

        /// <summary>
        /// QuadTree constructor
        /// </summary>
        /// <param name="size">The size of the QuadTree (i.e. the bottom-right with a top-left of (0,0))</param>
        /// <param name="maxItems">Maximum number of items in any cell of the QuadTree before partitioning</param>
        /// <remarks>This constructor is for ease of use</remarks>
        public QuadTree(Vector2 size, int maxItems)
            : this(new FRect(Vector2.Zero, size), maxItems)
        {
            // Nothing extra to initialize
        }

        /// <summary>
        /// Inserts an item into the QuadTree
        /// </summary>
        /// <param name="item">The item to insert</param>
        /// <remarks>Checks to see if the world needs resizing and does so if needed</remarks>
        public QuadTreePositionItem<T> Insert(QuadTreePositionItem<T> item)
        {
            // check if the world needs resizing
            if (!rootNode.ContainsRect(item.Rect))
            {
                Resize(new FRect(
                    Vector2.Min(rootNode.Rect.TopLeft, item.Rect.TopLeft) * 2,
                    Vector2.Max(rootNode.Rect.BottomRight, item.Rect.BottomRight) * 2));
            }

            rootNode.Insert(item);

            return item;
        }

        /// <summary>
        /// Inserts an item into the QuadTree
        /// </summary>
        /// <param name="parent">The parent of the new position item</param>
        /// <param name="position">The position of the new position item</param>
        /// <param name="size">The size of the new position item</param>
        /// <returns>A new position item</returns>
        /// <remarks>Checks to see if the world needs resizing and does so if needed</remarks>
        public QuadTreePositionItem<T> Insert(T parent, Vector2 position, Vector2 size)
        {
            QuadTreePositionItem<T> item = new QuadTreePositionItem<T>(parent, position, size);

            // check if the world needs resizing
            if (!rootNode.ContainsRect(item.Rect))
            {
                Resize(new FRect(
                    Vector2.Min(rootNode.Rect.TopLeft, item.Rect.TopLeft) * 2,
                    Vector2.Max(rootNode.Rect.BottomRight, item.Rect.BottomRight) * 2));
            }

            rootNode.Insert(item);

            return item;
        }

        /// <summary>
        /// Resizes the Quadtree field
        /// </summary>
        /// <param name="newWorld">The new field</param>
        /// <remarks>This is an expensive operation, so try to initialize the world to a big enough size</remarks>
        public void Resize(FRect newWorld)
        {
            // Get all of the items in the tree
            List<QuadTreePositionItem<T>> Components = new List<QuadTreePositionItem<T>>();
            GetAllItems(ref Components);

            // Destroy the head node
            rootNode.Destroy();
            rootNode = null;

            // Create a new head
            rootNode = new QuadTreeNode<T>(newWorld, maxItems, Resize);

            // Reinsert the items
            foreach (QuadTreePositionItem<T> m in Components)
            {
                rootNode.Insert(m);
            }
        }

        /// <summary>
        /// Gets a list of items containing a specified point
        /// </summary>
        /// <param name="Point">The point</param>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetItems(Vector2 Point, ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                rootNode.GetItems(Point, ref ItemsList);
            }
        }

        /// <summary>
        /// Gets a list of items intersecting a specified rectangle
        /// </summary>
        /// <param name="Rect">The rectangle</param>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetItems(FRect Rect, ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                rootNode.GetItems(Rect, ref ItemsList);
            }
        }

        /// <summary>
        /// Get a list of all items in the quadtree
        /// </summary>
        /// <param name="ItemsFound">The list to add found items to (list will not be cleared first)</param>
        public void GetAllItems(ref List<QuadTreePositionItem<T>> ItemsList)
        {
            if (ItemsList != null)
            {
                rootNode.GetAllItems(ref ItemsList);
            }
        }
    }
}
