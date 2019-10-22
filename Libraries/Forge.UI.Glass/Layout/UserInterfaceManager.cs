using Forge.Core.Components;
using Forge.UI.Glass.Templates;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.UI.Glass.Layout
{
    public class UserInterfaceManager : Component
    {
        private IList<ITemplate> _templates;

        public IEnumerable<ITemplate> Templates => _templates;
        public int ActiveTemplateCount => _templates.Count;

        public void Create(ITemplate template)
        {
            template.Initialise();
            template.Reevaluate();
            _templates.Add(template);
        }
    }
}
