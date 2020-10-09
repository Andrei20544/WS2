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
    /// Логика взаимодействия для Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        public Order()
        {
            InitializeComponent();
        }

        private string link;

        public Order(int ordnum, int numprob, double? polnum, string fio, DateTime? datof, IList<ListBox> serv, double? cost)
        {
            InitializeComponent();

            DateTime dat = DateTime.Now;

            OrderDateOne.Text = DateTime.Now.ToString();
            OrderNum.Text = ordnum.ToString();
            NumProb.Text = numprob.ToString();
            PoliceNum.Text = polnum.ToString();
            FIO.Text = fio;
            DateOfBirthP.Text = datof.ToString();
            foreach(var item in serv)
            {
                ServCount.Items.Add(item);
            }           
            CostServ.Text = cost.ToString();

            link = $"https://wsrussia.ru/?data=base64({dat}&{ordnum}&{numprob}&{polnum}&{fio}&{datof}&{serv}&{cost}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog print = new PrintDialog();
            print.PrintVisual(otch, "");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ссылка: " + link);
        }
    }
}
