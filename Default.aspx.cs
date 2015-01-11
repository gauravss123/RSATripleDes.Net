using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string connetionString = null;
        SqlConnection connection;
        SqlCommand command;
        string sql = null;
        
        
        //connetionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2010\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True;User Instance=True";
        connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";  
        //"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2012\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True";
        sql = "TRUNCATE TABLE [user]";
        connection = new SqlConnection(connetionString);
        connection.Open();
        command = new SqlCommand(sql, connection);
        
        
        command.ExecuteReader();
        command.Dispose();
        connection.Close();

        sql = "INSERT INTO [user](username,password) VALUES('saurabh','saurabh')";
        //sql = "INSERT INTO [user]";
        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
        try
        {
           // connection = new SqlConnection(connetionString);
            connection.Open();
            command = new SqlCommand(sql, connection);
            command.ExecuteReader();

            command.Dispose();
            connection.Close();
        }
        catch(Exception ex)
        {
            Label1.Text = "" +ex ;
        }

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        int t = 0;
        string connetionString = null;//"ConnectionString";
        SqlConnection connection;
        SqlCommand command;
        string sql = null;

        SqlDataReader dataReader;
        //Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True
        //connetionString = "Data Source=(LocalDB)\v11.0;Integrated Security=True;AttachDbFilename=|DataDirectory|\\Database.mdf;";
        //Data Source=(LocalDB)\v11.0;AttachDbFilename="C:\\Users\\root\\Documents\\Visual Studio 2012\\WebSites\\minorproject\\App_Data\\Database.mdf";Integrated Security=True
        //connetionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2010\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True;User Instance=True";
        connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";  
        //"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2012\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True";
        sql = "Select * from [user]";
        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
        try
        {
            connection = new SqlConnection(connetionString);

            connection.Open();
            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                //Label1.Text = Label1.Text + "" + dataReader.GetValue(1) + dataReader.GetValue(2);
                //MessageBox.Show(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2)); } 
                if (TextBox1.Text == dataReader.GetValue(1).ToString())
                {
                    if (TextBox2.Text == dataReader.GetValue(2).ToString())
                    {
                        t = 1;
                        break;


                    }
                }
            }
                dataReader.Close();
                command.Dispose();
                connection.Close();
            
        }
        catch (Exception ex)
        {
            Label1.Text = "Exception:" + ex;
            //MessageBox.Show("Can not open connection ! "); 
        }
       
        if (t == 1)
        {
            HttpContext.Current.Session["TestSessionValue"] = TextBox1.Text;
            Session["user"]=TextBox1.Text;
            Session["et"] = -1;
            Session["dt"] = -1;
            Session["decrtype"] = null;
            Session["text"] = null;
            Label1.Text = Label1.Text + "valid login";
            Response.Redirect("~\\/Default2.aspx");
            Response.AppendHeader("Refresh", 30 + "; URL=\\Default2.aspx");
            
        }
        else
        {
            Label1.Text = Label1.Text + "Invalid Credential";
        }
    }
}