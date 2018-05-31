using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Framework;

namespace hejiazhi_36
{
    public class Show : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Show()
        {
        }

        protected override void OnClick()
        {
            UID dockWinID = new UIDClass();
            dockWinID.Value = ThisAddIn.IDs.DockableWindow1;
            IDockableWindow dockWindow = ArcMap.DockableWindowManager.GetDockableWindow(dockWinID);
            if (dockWindow != null && !dockWindow.IsVisible())
                dockWindow.Show(true);
        }

        protected override void OnUpdate()
        {
        }
    }
}
