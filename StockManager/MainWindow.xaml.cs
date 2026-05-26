using Microsoft.Data.Sqlite;
using StockManager.Models;
using StockManager.Models;
using System.Collections;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection;
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
using System.Xml.Linq;

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
            Get_List();
            Get_Category();
            sqlconnetion();


        }
        private void sqlconnetion()
        {
            SqliteConnection sql = new SqliteConnection("Data Source = Equipment.db");
            sql.Open();
            string query = @"
                            CREATE TABLE IF NOT EXISTS Equipment
                            (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                NAME TEXT,
                                CATEGORY TEXT,
                                QUANTITY INTEGER,
                                LOCATION  TEXT,
                                STATUS  TEXT,
                                MEMO TEXT
                            )";
            SqliteCommand cmd = new SqliteCommand(query, sql);
            cmd.ExecuteNonQuery();

            

            sql.Close();
        }
        private void Get_List()
        {
            SqliteConnection sql = new SqliteConnection("Data Source=Equipment.db");
            sql.Open();

            string SQL = @"SELECT * FROM Equipment";

            SqliteCommand cmd = new SqliteCommand(SQL, sql);
            SqliteDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            dt.Columns.Add("CHK", typeof(bool));
            foreach (DataRow row in dt.Rows)
            {
                row["CHK"] = false;
            }
            gridresult.ItemsSource = dt.DefaultView;
            
            sql.Close();



       

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

      
        private bool vaid_check()
        {
            if (string.IsNullOrEmpty(AsName.Text) || string.IsNullOrEmpty(AsQna.Text) || string.IsNullOrEmpty(AsLocation.Text))
            {
                MessageBox.Show("빈칸을 입력해주세요");
                return false;
            }
            return true;
        }

        private void Add_AssetItem_Click(object sender, RoutedEventArgs e)
        {
            if (vaid_check()) {
                string name = AsName.Text;
                string Qna = AsQna.Text;
                string location = AsLocation.Text;
                string category = ComboCate2.Text;
                string status = "-";
                string memo = Asmemo.Text;

                SqliteConnection sql = new SqliteConnection("Data Source=Equipment.db");
                sql.Open();

                string SQL = $@"
                        INSERT INTO Equipment
                        (NAME, CATEGORY, QUANTITY, LOCATION, STATUS, MEMO)
                        VALUES
                        ('{name}', '{category}', '{Qna}', '{location}', '{status}', '{memo}')";

                SqliteCommand cmd = new SqliteCommand(SQL, sql);
                cmd.ExecuteNonQuery();

                MessageBox.Show("추가 완료");

                sql.Close();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("정말 삭제하시겠습니까?","삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DataView view = gridresult.ItemsSource as DataView;
                foreach (DataRowView row in view)
                {
                    bool isChecked = (bool)row["CHK"];
                    if (isChecked == true)
                    {
                        SqliteConnection sql = new SqliteConnection("Data Source=Equipment.db");
                        sql.Open();

                        int id = Convert.ToInt32(row["ID"]);
                        string SQL = $@"
                        DELETE FROM Equipment
                        WHERE ID = {id}";
                        

                        SqliteCommand cmd = new SqliteCommand(SQL, sql);
                        cmd.ExecuteNonQuery();

                        

                        sql.Close();
                       
                    }
                }

                //CheckBox.Ischecked
            }
            MessageBox.Show("삭제 완료");
            Get_List();
        }
    }
 
}