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
        double[] PXi;
        double[] PYk;
        double PYprovPX;
        double PXprovPY;
        int x, y;

        const string HXstring = "H(X) = ";
        const string HYstring = "H(Y) = ";
        const string HXandYstring = "H(X, Y) = ";
        const string HXprovidingYstring = "H(X/Y) = ";
        const string HYprovidingXstring = "H(Y/X) = ";

        public MainWindow()
        {
            InitializeComponent();
        } 

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Grid1_Loaded(object sender, RoutedEventArgs e)
        {
            CreateMatrix();
        }

        private void CreateMatrix()
        {
            if (elements != null)
                for (int i = 0; i < y; i++)
                    for (int j = 0; j < x; j++)
                        grid1.Children.Remove(elements[i, j]);
            x = Convert.ToInt32(sizeX.Text);
            y = Convert.ToInt32(sizeY.Text);
            double left = -100, top = -350;
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

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            CreateMatrix();
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
        }

        double[] CalculatePXi()
        {
            if ((bool)_XandY.IsChecked)
            {
                double[] PXi = new double[x];
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
            else if((bool)_YprovX.IsChecked)
            {
                return null;
            }
            return null;
        }

        double[] CalculatePYk()
        {
            double[] PYk = new double[y];
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
    }
}
