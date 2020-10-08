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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public Search()
        {
            InitializeComponent();

            using(ModelBD md = new ModelBD())
            {
                var ServNam = from s in md.LabServices
                              select new
                              {
                                  NameServ = s.Name
                              };

                foreach(var item in ServNam)
                {
                    CombServNam.Items.Add(item.NameServ);
                }
            }

        }

        static int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;

        private static int LevenshteinDistance(string item, string nam)
        {
            var n = item.Length;
            var m = nam.Length;
            var matrix = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (int i = 0; i < n; i++)
            {
                matrix[i, 0] = i;
            }
            for (int j = 0; j < m; j++)
            {
                matrix[0, j] = j;
            }

            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < m; j++)
                {
                    var substitutionCost = item[i - 1] == nam[j - 1] ? 0 : 1;

                    matrix[i, j] = Minimum(matrix[i - 1, j] + deletionCost,          // удаление
                                            matrix[i, j - 1] + insertionCost,         // вставка
                                            matrix[i - 1, j - 1] + substitutionCost); // замена
                }
            }

            return matrix[n - 1, m - 1];
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PatList.Items.Clear();

                using (ModelBD md = new ModelBD())
                {
                    var fioPat = from p in md.Patients select p;
                    var namServ = md.LabServices.Where(p => p.Name.Equals(CombServNam.SelectedItem.ToString())).FirstOrDefault();

                    foreach (var item in fioPat)
                    {
                        if (LevenshteinDistance(item.FIO.Split(' ')[0], nam.Text) <= 3 && CombServNam.SelectedItem.ToString() == namServ.Name)
                        {
                            PatList.Items.Add(item.FIO);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //private void nam_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        PatList.Items.Clear();

        //        using (ModelDB md = new ModelDB())
        //        {
        //            var fioPat = from p in md.Patients select p;
        //            var namServ = md.LabServices.Where(p => p.Name.Equals(ServNam.Text)).FirstOrDefault();

        //            foreach (var item in fioPat)
        //            {
        //                if (LevenshteinDistance(item.FIO.Split(' ')[0], nam.Text) <= 3 && ServNam.Text == namServ.Name)
        //                {
        //                    PatList.Items.Add(item.FIO);
        //                }
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}
    }
}
