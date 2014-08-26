﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Network;

namespace CVARC.BotDemo
{
    static class Program
    {
        private static Competitions competitions;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var settings = new BotDemoSettings();
            try
            {
                competitions = Competitions.Load(settings.CompetitionsName, "Level1");
                competitions.HelloPackage = new HelloPackage { MapSeed = -1 };
                competitions.Initialize(new CVARCEngine(competitions.CvarcRules), new[] { new RobotSettings(0, true), new RobotSettings(0, true)});
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC BotDemo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < competitions.RobotCount; i++)
            {
                if (i == settings.BotNames.Length) break;
                if (settings.BotNames[i] == "None") continue;
                bots.Add(competitions.CreateBot(settings.BotNames[i], i));
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new TutorialForm(competitions);
            new Thread(() => competitions.ProcessParticipants(true, int.MaxValue, bots.ToArray())) { IsBackground = true }.Start();
            Application.Run(form);
        }
    }
}