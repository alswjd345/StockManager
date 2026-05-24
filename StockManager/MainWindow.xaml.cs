using StockManager.Models;
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
using StockManager.Models;

namespace StockManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            add_data();
            Get_Category();
        }
        private void add_data()
        {
            List<AssetItem> list = new List<AssetItem>();
            AssetItem item = new AssetItem();
            item.Name = "마우스";
            item.Category = "사무용품";

            list.Add(item);
            gridresult.ItemsSource = list;

        }
        private void Get_Category()
        {
            
            ComboCate.Items.Add("사무용품");
            ComboCate.Items.Add("가구");
            ComboCate.Items.Add("IT용품");
            ComboCate.Items.Add("기타");
            ComboCate.SelectedIndex = 0;

            ComboCate2.Items.Add("사무용품");
            ComboCate2.Items.Add("가구");
            ComboCate2.Items.Add("IT용품");
            ComboCate2.Items.Add("기타");
            ComboCate2.SelectedIndex = 0;
        }
    }
 
}