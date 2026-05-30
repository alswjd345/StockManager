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
        private int selectedId;
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

                Get_List();
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

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            SqliteConnection sql = new SqliteConnection("Data Source=Equipment.db");
            sql.Open();

            string search_txt = SearchTxt.Text.Trim();
            string search_combo = ComboCate.Text;


            string SQL = $@"
                            SELECT * FROM Equipment
                            WHERE NAME = '{search_txt}'
                            AND CATEGORY = '{search_combo}'";


            SqliteCommand cmd = new SqliteCommand(SQL, sql);
            SqliteDataReader reader = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            gridresult.ItemsSource = dt.DefaultView;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            DataView dt = (DataView)gridresult.ItemsSource;
            DataView view = gridresult.ItemsSource as DataView;
            int count = 0;
            gridresult.CommitEdit();

            foreach (DataRowView row in view)
            {
                bool isChecked = (bool)row["CHK"];
                DataRowView checkedRow = null;
                if (isChecked == true)
                {
                    count++;
                    checkedRow = row;
                    if (count == 1)
                    {
                        AsName.Text = checkedRow["NAME"].ToString();
                        ComboCate2.SelectedItem = checkedRow["CATEGORY"].ToString();
                        AsQna.Text = checkedRow["QUANTITY"].ToString();
                        AsLocation.Text = checkedRow["LOCATION"].ToString();
                        Asmemo.Text = checkedRow["MEMO"].ToString();
                        selectedId = Convert.ToInt32(row["ID"]);
                    }
                }
                
            }
            if (count >= 2)
            {
                MessageBox.Show("수정할 비품을 하나만 선택해주세요");
                return; 
            }
            if (count == 0)
            {
                MessageBox.Show("수정할 비품을 선택해주세요");
                return;
            }
           
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string name = AsName.Text;
            string combo= ComboCate2.SelectedItem.ToString();
            int qna=int.Parse(AsQna.Text);
            string location=AsLocation.Text;
            string memo=Asmemo.Text;
            

            SqliteConnection sql = new SqliteConnection("Data Source=Equipment.db");
            sql.Open();

           
            string SQL = $@"
                        UPDATE  Equipment SET
                        NAME='{name}',CATEGORY='{combo}' ,QUANTITY='{qna}', LOCATION ='{location}', MEMO ='{memo}'
                       WHERE ID = {selectedId}";


            SqliteCommand cmd = new SqliteCommand(SQL, sql);
            int result= cmd.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("수정이 완료되었습니다.");
                Get_List();
            }
            else
            {
                MessageBox.Show("수정할 데이터를 찾을 수 없습니다.");
            }
            

        }
    }
 
}