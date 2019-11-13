﻿using Forge.Core.Components;
using Forge.Core.Interfaces;
using Forge.Core.Resources;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Forge.Core.Sound
{
    public class MusicManager : Component, IInit
    {
        [Inject] ResourceManager<Song> SongManager { get; set; }

        public void Initialise()
        {
        }

        public void Start(string songResource)
        {
            var song = SongManager.Get(songResource);
            if (song == null)
            {
                Console.Error.WriteLine($"Song resource not located: {songResource}");
                return;
            }
            MediaPlayer.Play(song);
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }
    }
}