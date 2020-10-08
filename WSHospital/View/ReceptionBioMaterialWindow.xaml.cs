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

        public CheckBox check;
        public ReceptionBioMaterialWindow(Users u, int age, BitmapImage ph)
        {
            InitializeComponent();    
            
            using (ModelDB md = new ModelDB())
            {
                var serv = from s in md.SetService
                           select new
                           {
                               ID = s.ID,
                               Name = s.Name
                           };

                var fio = from p in md.Patients
                          select new {
                              Name = p.FIO
                          };

                foreach(var item in fio)
                {
                    CombFIO.Items.Add(item.Name);
                }


                foreach(var item in serv)
                {
                    CombServ.Items.Add(item.Name);
                }
            }

        }

        public ReceptionBioMaterialWindow() {}

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

        public Orderr orderr;
        public NumberAnalyze numAn;

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            using (ModelDB md = new ModelDB())
            {

                try
                {
                    var IdPat = md.Patients.Where(p => p.FIO.Equals(CombFIO.SelectedItem)).FirstOrDefault();

                    var ServNam = md.LabServices.Where(p => p.Name.Equals(CombServ.SelectedItem)).FirstOrDefault();

                    orderr = new Orderr
                    {
                        IDPatient = IdPat.ID,
                        IDService = ServNam.ID,
                        Status = ""
                    };

                    md.Orderr.Add(orderr);
                    md.SaveChanges();

                    numAn = new NumberAnalyze
                    {
                        IDPatient = IdPat.ID,
                        IDService = ServNam.ID
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
            PrintDialog printDialog = new PrintDialog();
            printDialog.ShowDialog();
            printDialog.PrintVisual(canv, "");
        }

        private void CombServ_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DopServ.Items.Clear();

            using (ModelDB md = new ModelDB())
            {
                var ServSetNam = from s in md.SetService
                              select new
                              {
                                  NameSetServ = s.Name,
                                  IDServ = s.IDService
                              };                

                foreach(var item in ServSetNam)
                {
                    if (CombServ.SelectedItem.ToString().Equals(item.NameSetServ))
                    {
                        var ServNam = from s in md.LabServices
                                      where s.ID == item.IDServ
                                      select new
                                      {
                                          NameServ = s.Name
                                      };

                        foreach (var item1 in ServNam)
                        {
                            CheckBox check = new CheckBox();
                            check.Content = item1.NameServ;
                            DopServ.Items.Add(check);
                        }
                    }
                }

            }
        }
    }
}
