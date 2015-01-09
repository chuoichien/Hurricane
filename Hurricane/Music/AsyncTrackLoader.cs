﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hurricane.Music.Data;

namespace Hurricane.Music
{
    class AsyncTrackLoader
    {
        private static AsyncTrackLoader _instance;
        public static AsyncTrackLoader Instance
        {
            get { return _instance ?? (_instance = new AsyncTrackLoader()); }
        }


        private AsyncTrackLoader()
        {
            PlaylistsToCheck = new List<Playlist>();
        }

        private bool _isrunning;

        public List<Playlist> PlaylistsToCheck { get; set; }

        private bool havetocheck;
        public void RunAsync(List<Playlist> lst)
        {
            PlaylistsToCheck.AddRange(lst.Where(p => !PlaylistsToCheck.Contains(p))); //We only add tracks which aren't already in our queue
            havetocheck = true;
            var t = LoadTracks();
        }

        protected async Task LoadTracks()
        {
            if (_isrunning) return;
            _isrunning = true;
            havetocheck = false;
            foreach (var p in PlaylistsToCheck.ToList())
            {
                foreach (var track in p.Tracks)
                {
                    //if(track.NotChecked && !await track.CheckTrack()) p.RemoveTrackWithAnimation(track);
                }
                PlaylistsToCheck.Remove(p);
            }

            _isrunning = false;
            if (havetocheck) await LoadTracks();
        }
    }
}