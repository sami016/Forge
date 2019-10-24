using Forge.Core.Components;
using Forge.Core.Resources;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass
{
    public class UserInterfaceManager : Component
    {
        private readonly IList<ITemplate> _templates = new List<ITemplate>();

        public IEnumerable<ITemplate> Templates => _templates;
        public int ActiveTemplateCount => _templates.Count;

        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }
        [Inject] public ResourceManager<Color> Colours { get; set; }

        public void Create(ITemplate template)
        {
            template.Initialise();
            template.Reevaluate();
            _templates.Add(template);
        }

        public void Render()
        {
            GameTime gameTime = null;
            SpriteBatch spriteBatch = null;
            //TODO use engine renderable interface.
            foreach (var element in _templates)
            {
                element.Render(
                    new RenderContext
                    (
                        spriteBatch,
                        gameTime,
                        Fonts,
                        Colours
                    )
                );
            }
        }
    }
}
