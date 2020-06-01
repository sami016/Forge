using Forge.UI.Glass.Interaction;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Elements
{
    public static class ElementExtensions
    {
        public static T ConfigureEvents<T>(this T t, Action<UIEvents> configure)
            where T : IElement
        {
            configure(t.Events);
            return t;
        }

        public static T WithPosition<T>(this T t, Rectangle position)
            where T : IElement
        {
            t.Position = position;
            return t;
        }

        public static T WithPosition<T>(this T t, float x, float y)
            where T : IElement
        {
            t.Position = new Rectangle((int)x, (int)y, t.Position.Width, t.Position.Height);
            return t;
        }

        public static T WithSize<T>(this T t, float width, float height)
            where T : IElement
        {
            t.Position = new Rectangle(t.Position.X, t.Position.Y, (int)width, (int)height);
            return t;
        }

        public static T SizeToChildren<T>(this T t)
            where T : IElement
        {
            var width = 0;
            var height = 0;
            foreach (var child in t.Children)
            {
                var newWidth = child.Position.X + child.Position.Width;
                var newHeight = child.Position.Y + child.Position.Height;

                if (newWidth > width)
                {
                    width = newWidth;
                }
                if (newHeight > height)
                {
                    height = newHeight;
                }
            }
            t.Position = new Rectangle(t.Position.X, t.Position.Y, (int)width, (int)height);
            return t;
        }
    }
}
