using Newtonsoft.Json;
using OpenCvSharp;
using ShogunVS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShogunVS.Settings
{

    public class FiltersSettings
    {
        #region Fields

        #endregion

        #region Constructors

        public FiltersSettings()
        {
            SettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

            // If there's no settings file then create one.
            if (!File.Exists(SettingsFilePath))
            {
                Save();
            }
        }

        #endregion

        #region Properties


        public string SettingsFilePath { get; set; }

        public ColorLimits Yellow = new ColorLimits();

        public ColorLimits Red = new ColorLimits();

        public ColorLimits Blue = new ColorLimits();

        public ColorLimits Black = new ColorLimits();

        public ColorLimits Purple = new ColorLimits();

        public ColorLimits Green = new ColorLimits();

        public int GaussianBlurSize;
        public Rect ROI { get; set; } = new Rect(50,50,50,50);

        #endregion

        #region Methods 

        /// <summary>
        /// Stores current state in file.
        /// </summary>
        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(SettingsFilePath))
            {
                sw.Write(JsonConvert.SerializeObject(this, Formatting.Indented));
            }
        }

        /// <summary>
        /// Loads settings from file and populates the object.
        /// </summary>
        public void Load()
        {
            using (StreamReader sr = new StreamReader(SettingsFilePath))
            {
                FiltersSettings settings = JsonConvert.DeserializeObject<FiltersSettings>(sr.ReadToEnd());
                Yellow = settings.Yellow;
                Red = settings.Red;
                Blue = settings.Blue;
                Black = settings.Black;
                Purple = settings.Purple;
                Green = settings.Green;
                GaussianBlurSize = settings.GaussianBlurSize;
                ROI = settings.ROI;
            }
        }

        #endregion
    }
}