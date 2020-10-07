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

        public ReceptionBioMaterialWindow(Patients pat)
        {
            FIO.Text = pat.FIO;
            PassDat.Text = pat.PassportData;
            PhoneP.Text = pat.Phone.ToString();
            InsPol.Text = pat.InsurancePolicy.ToString();
            DateBirth.Text = pat.DateOfBirth.ToString();
            EmailP.Text = pat.Email;
            TypePol.Text = pat.TypeOfPolicy;
        }

        public LabServices Serv; 
        public NumberAnalyze numAn;

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();

            for (int i = 0; i <= 6; i++)
            {
                
            }
            

            using(ModelDB md = new ModelDB())
            {
                int shtrih;
            }
           

            for(int i = 0; i < 15; i++){



                //Rectangle rectangle = new Rectangle();
                //rectangle.Width = 5;
                //rectangle.Height = 120;
                //canv.Children.Add(rectangle);
            }

            AddPatient addPatient = new AddPatient(Shtr.Text);
            addPatient.Show();
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
    }
}
