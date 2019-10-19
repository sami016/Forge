using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core
{
    public class ForgeGameBuilder
    {
        private IServiceCollection _serviceProvider;

        public ForgeGameBuilder()
        {

        }

        public ForgeGameBuilder UseServiceContainer()
        {
            return this;
        }

        public ForgeGame Create()
        {
            return new ForgeGame();
        }
    }
}
