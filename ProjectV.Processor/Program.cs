#nullable enable
global using System;
global using System.Diagnostics;
global using System.IO;
global using System.Windows.Forms;
global using static ProjectV.BcdEdit;
global using static ProjectV.GlobalConstants;
using System.Threading;

// It is unfortunate but we have to set it to Unknown first.
Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
Thread.CurrentThread.SetApartmentState(ApartmentState.STA);

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);

Application.Run(new ProjectV.Processor.FormMain());