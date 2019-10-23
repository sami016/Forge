using Forge.Core.Components;
using Forge.Core.Resources;
using Forge.UI.Glass.Templates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Layout
{
    public class UserInterfaceManager : Component
    {
        private readonly IList<ITemplate> _templates;

        public IEnumerable<ITemplate> Templates => _templates;
        public int ActiveTemplateCount => _templates.Count;

        [Inject] public ResourceManager<SpriteFont> Fonts { get; set; }

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
                        Fonts
                    )
                );
            }
        }
    }
}
