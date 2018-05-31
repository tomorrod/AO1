using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using System.Windows;

namespace hejiazhi_36
{
    public class Info : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public Info()
        {
        }

        protected override void OnClick()
        {
            ILayer layer = ArcMap.Document.SelectedLayer;
            IFeatureLayer fl = layer as IFeatureLayer;
            if (fl != null)
            {
                IFeatureClass fc = fl.FeatureClass;
                int count = fc.FeatureCount(null);
                string featureType = fc.FeatureType.ToString();
                string geoType = fc.ShapeType.ToString();
                string info = string.Format("selected layer has {0} features; FeatureType is {1}; ShapeType is {2}", count, featureType, geoType);
                MessageBox.Show(info);
            }
            else
                MessageBox.Show("Please select a featurelayer");
        }

        protected override void OnUpdate()
        {
        }
    }
}
