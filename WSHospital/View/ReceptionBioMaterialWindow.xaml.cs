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
using System.Windows.Shapes;

namespace WSHospital.View
{
    /// <summary>
    /// Логика взаимодействия для ReceptionBioMaterialWindow.xaml
    /// </summary>
    public partial class ReceptionBioMaterialWindow : Window
    {
        public ReceptionBioMaterialWindow(Users u, int age, BitmapImage ph)
        {
            InitializeComponent();    
            
            using (ModelDB md = new ModelDB())
            {
                var serv = from s in md.LabServices
                           select new
                           {
                               ID = s.ID,
                               Name = s.Name
                           };

                foreach(var item in serv)
                {
                    CheckBox check = new CheckBox();
                    check.Content = item.ID + ". " + item.Name;
                    comb.Items.Add(check);
                }
            }

        }

        public ReceptionBioMaterialWindow() {}

        public LabServices Serv; 
        public NumberAnalyze numAn;
        public Random rnd;

        public string GetControlEan(string str)
        {
            int ch = 0;
            int nch = 0;
            for(int i = 1; i<6; i++)
            {
                ch += int.Parse(str.Substring(2 * i, 1));
                nch += int.Parse(str.Substring(2 * i - 1, 1));
            }
            ch += 3;
            int cntr = 10 * (ch + nch) % 10;

            return cntr == 10 ? "0" : cntr.ToString();
        }

        public string BarCodeGenerate()
        {
            rnd = new Random();
            long CodeZakaza = rnd.Next();
            long day = DateTime.Now.Millisecond;
            string prefix = CodeZakaza.ToString().Substring(0, 2);
            string ShCode = prefix.Length == 0 ? "40":prefix + ("0000000000" + CodeZakaza).Substring(("0000000000" + CodeZakaza).Length - 11);
            string StrShk = ShCode + GetControlEan(ShCode);

            return StrShk;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string barcode = BarCodeGenerate();

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanel.Width = 520;
            stackPanel.Height = 140;     
            

            for (int i = 0; i < barcode.Length; i++)
            {
                Rectangle rectangle = new Rectangle();
                Label label = new Label();
                label.VerticalAlignment = VerticalAlignment.Bottom;

                StackPanel stackPanel1 = new StackPanel();
                stackPanel1.Orientation = Orientation.Horizontal;
                stackPanel1.VerticalAlignment = VerticalAlignment.Bottom;

                rectangle.Fill = Brushes.Black;
                rectangle.Width = int.Parse(barcode[i].ToString());
                rectangle.Height = 100;

                label.Content = barcode[i];
                label.Margin = new Thickness(0, 20, 0, 0);

                stackPanel1.Children.Add(label);
                stackPanel1.Children.Add(rectangle);         

                stackPanel.Children.Add(stackPanel1);
            }

            canv.Children.Add(stackPanel);

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (ModelDB md = new ModelDB())
            {

                try
                {
                    Serv = new LabServices
                    {

                    };

                    md.LabServices.Add(Serv);
                    md.SaveChanges();

                    numAn = new NumberAnalyze
                    {

                    };

                    md.NumberAnalyze.Add(numAn);
                    md.SaveChanges();

                    MessageBox.Show("Данные успешно созранены в БД");
                }
                catch (Exception)
                {
                    MessageBox.Show("Что-то пошло не так!");
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Shtr_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (Shtr.Text.Length != 0)
            {
                gen.IsEnabled = false;
            }
            else if (Shtr.Text.Length == 0)
            {
                gen.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPatient addPatient = new AddPatient(Shtr.Text);
            addPatient.Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }
    }
}
