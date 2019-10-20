using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Program_for_excercises_Csh
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox[,] elements;
        TextBox[] PX, PY;
        double[] PXi, PYk;
        double PYprovPX, PXprovPY;
        double Rx, Ry;
        int x, y;

        const string HXstring = "H(X) = ";
        const string HYstring = "H(Y) = ";
        const string HXandYstring = "H(X, Y) = ";
        const string HXprovidingYstring = "H(X/Y) = ";
        const string HYprovidingXstring = "H(Y/X) = ";
        const string RXstring = "Rx = ";
        const string RYstring = "Ry = ";

        public MainWindow()
        {
            InitializeComponent();
        } 

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid1_Loaded(object sender, RoutedEventArgs e)
        {
            CreateMatrix(3, 3);
        }     

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            int width = Convert.ToInt32(sizeX.Text),
                height = Convert.ToInt32(sizeY.Text);
            if ((bool)_XandY.IsChecked)
                CreateMatrix(height, width);
            else if ((bool)_X.IsChecked)
                CreateMatrix(1, width);
            else if ((bool)_YprovX_x.IsChecked)
            {
                CreateMatrix(height, width);
                PX = new TextBox[width];
                int top = -350;
                for (int i = 0; i < PX.Length; i++) 
                {
                    PX[i] = new TextBox();
                    PX[i].Width = 48;
                    PX[i].Height = 24;
                    PX[i].Margin = new Thickness(-600, top, 0, 0);
                    grid1.Children.Add(PX[i]);
                    top += 70;
                }
            }
            else if ((bool)_XprovY_y.IsChecked)
            {
                CreateMatrix(height, width);
                PY = new TextBox[height];
                int top = -350;
                for (int i = 0; i < PY.Length; i++)
                {
                    PY[i] = new TextBox();
                    PY[i].Width = 48;
                    PY[i].Height = 24;
                    PY[i].Margin = new Thickness(-600, top, 0, 0);
                    grid1.Children.Add(PY[i]);
                    top += 70;
                }
            }
        }

        private void calculateButton_Click(object sender, RoutedEventArgs e)
        {
            PXi = CalculatePXi();
            PYk = CalculatePYk();
            HXresult.Text = HXstring + CalculateHX();
            HYresult.Text = HYstring + CalculateHY();
            HXandYresult.Text = HXandYstring + CalculateHXandY();
            HYprovXresult.Text = HYprovidingXstring + CalculateHYprovHX();
            HXprovYresult.Text = HXprovidingYstring + CalculateHXprovHY();
            Rx = 1 - CalculateHX() / Math.Log(Convert.ToDouble(sizeX.Text), 2);
            RX.Text = RXstring + Rx;
            Ry = 1 - CalculateHY() / Math.Log(Convert.ToDouble(sizeY.Text), 2);
            RY.Text = RYstring + Ry;
        }

        double[] CalculatePXi()
        {
            double[] PXi = new double[x];
            if ((bool)_XandY.IsChecked || (bool)_X.IsChecked)
            {
                double sum = 0;
                for (int j = 0, t = 0; j < x; j++, t++)
                {
                    for (int i = 0; i < y; i++)
                        sum += Convert.ToDouble(elements[i, j].Text);
                    PXi[t] = sum;
                    sum = 0;
                }
                return PXi;
            }
            else if ((bool)_YprovX_x.IsChecked) 
            {
                for (int i = 0; i < PXi.Length; i++)
                    PXi[i] = Convert.ToDouble(PX[i].Text);
                return PXi;
            }
            return null;
        }

        double[] CalculatePYk()
        {
            double[] PYk = new double[y];
            if ((bool)_XandY.IsChecked)
            {
                double sum = 0;
                for (int i = 0, t = 0; i < y; i++, t++)
                {
                    for (int j = 0; j < x; j++)
                        sum += Convert.ToDouble(elements[i, j].Text);
                    PYk[t] = sum;
                    sum = 0;
                }
                return PYk;
            }
            else if ((bool)_XprovY_y.IsChecked) 
            {
                for (int i = 0; i < PYk.Length; i++)
                    PYk[i] = Convert.ToDouble(PX[i].Text);
                return PYk;
            }
            return null;
        }

        double CalculateHX()
        {
            double HX = 0;
            for (int i = 0; i < PXi.Length; i++)
                HX -= PXi[i] * Math.Log(PXi[i], 2);
            return HX;
        }

        double CalculateHY()
        {
            double HY = 0;
            for (int k = 0; k < PYk.Length; k++)
                HY -= PYk[k] * Math.Log(PYk[k], 2);
            return HY;
        }

        double[,] CalculatePXAndY()
        {
            double[,] matrix = new double[y, x];
            if ((bool)_XandY.IsChecked || (bool)_X.IsChecked)
                for (int i = 0; i < y; i++)
                    for (int j = 0; j < x; j++)
                        matrix[i, j] = Convert.ToDouble(elements[i, j].Text);
            else if ((bool)_YprovX_x.IsChecked)
                for (int j = 0; j < x; j++)
                    for (int i = 0; i < y; i++)
                        matrix[i, j] = PXi[j] * Convert.ToDouble(elements[i, j].Text);
            else if ((bool)_XprovY_y.IsChecked)
                for (int i = 0; i < y; i++)
                    for (int j = 0; j < x; j++)
                        matrix[i, j] = PYk[j] * Convert.ToDouble(elements[i, j].Text);
            return matrix;
        }

        private void _YprovXRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void _XandYRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        double CalculateHXandY()
        {
            double HXandY = 0;
            double[,] elems = new double[y, x];
            for (int j = 0; j < x; j++)
                for (int i = 0; i < y; i++)
                {
                    elems[i, j] = Convert.ToDouble(elements[i, j].Text);
                    HXandY -= elems[i, j] * Math.Log(elems[i, j], 2);
                }
            return HXandY;
        }

        private void _YprovX_xRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void _YprovX_yRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        double CalculateHYprovHX()
        {
            double HYprovHX = 0;
            double[,] elems = new double[y, x],
                YXtemp = new double[y, x];
            for (int j = 0; j < x; j++)
                for (int i = 0; i < y; i++)
                {
                    elems[i, j] = Convert.ToDouble(elements[i, j].Text);
                    YXtemp[i, j] = elems[i, j] / PXi[j];
                    HYprovHX -= elems[i, j] * Math.Log(YXtemp[i, j], 2);
                }
            return HYprovHX;
        }

        private void _X_Checked(object sender, RoutedEventArgs e)
        {

        }

        double CalculateHXprovHY()
        {
            double PXprovPY = 0;
            double[,] elems = new double[y, x],
                XYtemp = new double[y, x];
            for (int j = 0; j < x; j++)
                for (int i = 0; i < y; i++)
                {
                    elems[i, j] = Convert.ToDouble(elements[i, j].Text);
                    XYtemp[i, j] = elems[i, j] / PYk[i];
                    PXprovPY -= elems[i, j] * Math.Log(XYtemp[i, j], 2);
                }
            return PXprovPY;
        }

        private void CreateMatrix(int height, int width)
        {
            if (elements != null)
                for (int i = 0; i < y; i++)
                    for (int j = 0; j < x; j++)
                        grid1.Children.Remove(elements[i, j]);
            x = width;
            y = height;
            sizeX.Text = Convert.ToString(width);
            sizeY.Text = Convert.ToString(height);
            int left = -100, top = -350;
            elements = new TextBox[y, x];
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    TextBox element = new TextBox();
                    element.Width = 48;
                    element.Height = 24;
                    element.Margin = new Thickness(left, top, 0, 0);
                    element.Foreground = new SolidColorBrush(Colors.Black);
                    left += 120;
                    elements[i, j] = element;
                    grid1.Children.Add(elements[i, j]);
                }
                top += 70;
                left = -100;
            }
        }
    }
}
