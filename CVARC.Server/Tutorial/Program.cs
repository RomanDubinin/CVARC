﻿using System;
using System.Threading;
using System.Windows.Forms;
using CVARC.Basic;
using CVARC.Basic.Controllers;
using CVARC.Network;

namespace CVARC.Tutorial
{
    internal static class Program
    {
        private static TutorialForm form;
        private static CompetitionsBundle CompetitionsBundle;
        private static readonly KeyboardController Controller = new KeyboardController();
        private const string BotName = "Sanguine";

        [STAThread]
        private static void Main()
        {
            try
            {
                var settings = new TutorialSettings();
                CompetitionsBundle = CompetitionsBundle.Load(settings.CompetitionsName, "Level1");
                if (settings.HasMap)
                    CompetitionsBundle.competitions.HelloPackage = new HelloPackage { MapSeed = settings.MapSeed };

                CompetitionsBundle.competitions.Initialize(new CVARCEngine(CompetitionsBundle.Rules), new[] { new RobotSettings(0, false), new RobotSettings(1, true) });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "CVARC Tutorial", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new TutorialForm(CompetitionsBundle.competitions);
            form.KeyPreview = true;
            form.KeyDown += (sender, e) => CompetitionsBundle.competitions.ApplyCommand(Controller.GetCommand(e.KeyCode));
            form.KeyUp += (sender, e) => CompetitionsBundle.competitions.ApplyCommand(Command.Sleep(0));
            new Thread(() => CompetitionsBundle.competitions.ProcessParticipants(true, int.MaxValue, new[] { CompetitionsBundle.competitions.CreateBot(BotName, 1) }))
                {
                    IsBackground = true
                }.Start();
            Application.Run(form);
        }
    }
}