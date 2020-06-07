using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Stylings
{
    public interface IBackgroundStyling
    {
    }

    public sealed class ColourBackgroundStyling : IBackgroundStyling
    {
        public string ColourResource { get; set; }
        public Color? Colour { get; set; }
        public float BorderRadius { get; set; }

        public ColourBackgroundStyling(Color? colour)
        {
            Colour = colour;
        }

        public ColourBackgroundStyling()
        {
        }
    }

    public sealed class ImageBackgroundStyling : IBackgroundStyling
    {
        public string ImageResource { get; set; }
        public Texture2D Image { get; set; }

        public bool BorderStretchMode { get; set; }
        public float BorderStretchTop { get; set; }
        public float BorderStretchBottom { get; set; }
        public float BorderStretchLeft { get; set; }
        public float BorderStretchRight { get; set; }

        public ImageBackgroundStyling()
        {
        }

        public ImageBackgroundStyling(string imageResource)
        {
            ImageResource = imageResource;
        }
    }

}
