// BackgroundMusic.cs
using System;
using WMPLib;

namespace FP
{
    public class BackgroundMusic : IDisposable
    {
        private WindowsMediaPlayer player;
        private string musicFilePath;

        /// <summary>
        /// Konstruktor untuk BackgroundMusic.
        /// </summary>
        /// <param name="filePath">Path ke file musik yang akan diputar.</param>
        public BackgroundMusic(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path tidak boleh kosong.", nameof(filePath));

            if (!System.IO.File.Exists(filePath))
                throw new System.IO.FileNotFoundException("File musik tidak ditemukan.", filePath);

            musicFilePath = filePath;
            player = new WindowsMediaPlayer();
            player.settings.autoStart = false;
            player.settings.setMode("loop", true); // Memutar musik secara loop
        }

        /// <summary>
        /// Mulai memutar musik.
        /// </summary>
        public void Play()
        {
            player.URL = musicFilePath;
            player.controls.play();
        }

        /// <summary>
        /// Hentikan pemutaran musik.
        /// </summary>
        public void Stop()
        {
            player.controls.stop();
        }

        /// <summary>
        /// Menghentikan pemutaran musik dan membersihkan resource.
        /// </summary>
        public void Dispose()
        {
            if (player != null)
            {
                player.controls.stop();
                player.close();
                player = null;
            }
        }
    }
}
