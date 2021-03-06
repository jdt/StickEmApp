﻿using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows;
using StickEmApp.Dal;

namespace StickEmApp.Windows
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window
    {
        public Shell()
        {
            var dbFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "data.db");

            var window = new LoadingWindow();
            window.Show();

            UnitOfWorkManager.Initialize(dbFile, DatabaseFileMode.CreateIfNotExists);

            window.Close();

            InitializeComponent();
        }
    }
}
