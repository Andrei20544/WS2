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
    /// Логика взаимодействия для AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Window
    {
        public AddPatient(string shtrih)
        {
            InitializeComponent();

            Shtrih.Text = shtrih;

            using (ModelDB md = new ModelDB())
            {
                var comp = from c in md.Company
                           select new
                           {
                               NameComp = c.Name,
                               IdComp = c.ID
                           };

                foreach(var item in comp)
                {
                    CompName.Items.Add(item.IdComp + ". "+ item.NameComp);
                }
            }

        }

        public Patients pat;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using(ModelDB md = new ModelDB())
            {
                try
                {
                    pat = new Patients
                    {
                        FIO = pFIO.Text,
                        DateOfBirth = DaT.DisplayDate,
                        Email = pEmail.Text,
                        PassportData = pPassportData.Text,
                        Phone = int.Parse(pPhone.Text),
                        InsurancePolicy = int.Parse(pInsPolicy.Text),
                        TypeOfPolicy = pTypePolicy.Text,
                        IDCompany = int.Parse(CompName.SelectedItem.ToString().Split('.')[0])
                    };

                    md.Patients.Add(pat);
                    md.SaveChanges();

                    ReceptionBioMaterialWindow rbmw = new ReceptionBioMaterialWindow(pat);

                    MessageBox.Show("Данные успешно созранены в БД");



                    //ReceptionBioMaterialWindow RBMW = new ReceptionBioMaterialWindow();
                    //RBMW.SetComboFio();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
