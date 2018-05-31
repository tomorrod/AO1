using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hejiazhi_36
{
    /// <summary>
    /// Designer class of the dockable window add-in. It contains WPF user interfaces that
    /// make up the dockable window.
    /// </summary>
    public partial class DockableWindow1 : UserControl
    {
        public DockableWindow1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Implementation class of the dockable window add-in. It is responsible for 
        /// creating and disposing the user interface class of the dockable window.
        /// </summary>
        public class AddinImpl : ESRI.ArcGIS.Desktop.AddIns.DockableWindow
        {
            private System.Windows.Forms.Integration.ElementHost m_windowUI;

            public AddinImpl()
            {
            }

            protected override IntPtr OnCreateChild()
            {
                m_windowUI = new System.Windows.Forms.Integration.ElementHost();
                m_windowUI.Child = new DockableWindow1();
                return m_windowUI.Handle;
            }

            protected override void Dispose(bool disposing)
            {
                if (m_windowUI != null)
                    m_windowUI.Dispose();

                base.Dispose(disposing);
            }

        }

        IMap map = null;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            lblayer.Items.Clear();
            lbfields.Items.Clear();
            lbvalue.Items.Clear();

             map = ArcMap.Document.FocusMap;

            for (int i = 0; i < map.LayerCount; i++)
            {
                ILayer layer = map.Layer[i];
                lblayer.Items.Add(layer.Name);
            }
        }
        private ILayer m_pLayer;
        private void lblayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedLayerName = lblayer.SelectedItem.ToString();
            
            for (int i = 0; i < ArcMap.Document.FocusMap.LayerCount; i++)
            {
                ILayer layer = ArcMap.Document.FocusMap.Layer[i];
                if (selectedLayerName == layer.Name)
                {
                    fl = layer as IFeatureLayer;
                    m_pLayer = layer;

                    break;
                }
            }

            if (fl != null)
            {
                lbfields.Items.Clear();

                IFeatureClass fc = fl.FeatureClass;
                //IFeature pFeature = null;
                //pFeature = fc as IFeature;
                //IPolygon pPolygon = pFeature.Shape as IPolygon;
                IFields flds = fc.Fields;

                for (int i = 0; i < flds.FieldCount; i++)
                {
                    IField fld = flds.get_Field(i);
                    lbfields.Items.Add(fld.Name);
                }
            }
        }

        IFeatureLayer fl = null;
        int selectedField = 0;
        private void lbfields_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFieldName = lbfields.SelectedItem.ToString();
            IFields flds = fl.FeatureClass.Fields;

            int fldIndex = 0;
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField fld = flds.get_Field(i);
                if (fld.Name == selectedFieldName)
                {
                    fldIndex = i;
                    selectedField = i;
                    break;
                }
            }

            lbvalue.Items.Clear();

            IFeatureCursor ftCur = fl.FeatureClass.Search(null, false);

            IFeature ft = ftCur.NextFeature();
            while (ft != null)
            {
                object value = ft.get_Value(fldIndex);
                lbvalue.Items.Add(value);
                //MessageBox.Show("{0}", ft.get_Value(fldIndex));
                ft = ftCur.NextFeature();
            }
        }

        private void lbvalue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFieldName = lbvalue.SelectedItem.ToString();
            IFeatureCursor ftCur = fl.FeatureClass.Search(null, false);
            IFeature ft = ftCur.NextFeature();
            IFeature fldIndex = null;
            while (ft != null)     
            {
                
                object xx = ft.get_Value(selectedField);
                
                if (selectedFieldName == xx.ToString())
                {
                    fldIndex = ft;
                    
                    break;
                }
                ft = ftCur.NextFeature();
            }
            
            IPolygon pPolygon = fldIndex.Shape as IPolygon;

            ISimpleFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Style = esriSimpleFillStyle.esriSFSDiagonalCross;
            pFillSymbol.Color = getRGB(0, 255, 255);

            IFillShapeElement pPolygonEle = new PolygonElementClass();
            pPolygonEle.Symbol = pFillSymbol;
        
            IElement pEle = pPolygonEle as IElement;
            pEle.Geometry = pPolygon;

            IGraphicsContainer pGraphicsContainer = ArcMap.Document.FocusMap as IGraphicsContainer;  
            pGraphicsContainer.AddElement(pEle, 0);
            MessageBox.Show("OK!");
            ArcMap.Document.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lbvalue.Items.Clear();
            string selectedFieldName = lbfields.SelectedItem.ToString();

            IFields flds = fl.FeatureClass.Fields;

            int fldIndex = 0;
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField fld = flds.get_Field(i);
                if (fld.Name == selectedFieldName)
                {
                    fldIndex = i;
                    break;
                }
            }

            lbvalue.Items.Clear();

            IFeatureCursor ftCur = fl.FeatureClass.Search(null, false);

            IFeature ft = ftCur.NextFeature();
            List<object> rank1 = new List<object>();
            while (ft != null)
            {
                object value = ft.get_Value(fldIndex);
                rank1.Add(value);
                //lbvalue.Items.Add(value);
                ft = ftCur.NextFeature();
            }
            object iden=rank1[0];
            bool numb = true;
            foreach (object num in rank1)
            {
                //if (num != iden)
                //    numb = 1;
                numb = iden.Equals(num);
            }
            if (numb)
                MessageBox.Show("所选属性全部相同！");
            rank1.Sort();
            foreach (object num in rank1)
            {
                lbvalue.Items.Add(num);     
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lbvalue.Items.Clear();
            string selectedFieldName = lbfields.SelectedItem.ToString();

            IFields flds = fl.FeatureClass.Fields;

            int fldIndex = 0;
            for (int i = 0; i < flds.FieldCount; i++)
            {
                IField fld = flds.get_Field(i);
                if (fld.Name == selectedFieldName)
                {
                    fldIndex = i;
                    break;
                }
            }

            lbvalue.Items.Clear();

            IFeatureCursor ftCur = fl.FeatureClass.Search(null, false);

            IFeature ft = ftCur.NextFeature();
            List<object> rank1 = new List<object>();
            while (ft != null)
            {
                object value = ft.get_Value(fldIndex);
                rank1.Add(value);
                //lbvalue.Items.Add(value);
                ft = ftCur.NextFeature();
            }
            rank1.Sort();
            rank1.Reverse();
            foreach (object num in rank1)
            {
                lbvalue.Items.Add(num);
            }
        }
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //AttributeTableFrm attributeTable = new AttributeTableFrm();
            //attributeTable.ShowDialog();
            Form1 f2 = new Form1();
            f2.CreateAttributeTable(m_pLayer);
            f2.ShowDialog();
            //if (f2.DialogResult == DialogResult.OK)
            //{
            //    this.textBox1.Text = f2.str;
            //}
        }
        public IColor getRGB(int yourRed, int yourGreen, int yourBlue)
        {
            IRgbColor pRGB = new RgbColorClass();
            pRGB.Red = yourRed;
            pRGB.Green = yourGreen;
            pRGB.Blue = yourBlue;
            pRGB.UseWindowsDithering = true;
            return pRGB;
        }
    }
}
