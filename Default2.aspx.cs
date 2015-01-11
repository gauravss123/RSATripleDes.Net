using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Text;
using System.Diagnostics;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label2.Text = null;
        Label1.Text = null;
        if (Session["user"] == null)
        {
            Response.Redirect("~\\/Default.aspx");
        }
        Label1.Text = Label1.Text + Session["user"].ToString(); //+ Session["TestSessionValue"];
        if (Convert.ToInt16(Session["et"]) > 0)
        {
            Label3.Text = Session["et"].ToString();
            Session["et"] = -1;
        }
        if (Convert.ToInt16(Session["dt"]) > 0)
        {
            Label4.Text = Session["dt"].ToString();
            Session["dt"] = -1;
        }
        if (Session["text"] != null)
        {
            content123.InnerText= Session["text"].ToString();

            //TextBox1.Text = Session["text"].ToString();
            Session["text"] = null;
        }
        if (Session["decrtype"] != null)
        {
            Label6.Text = Session["decrtype"].ToString();
            Session["decrtype"] = "No Data";
        }
        
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
     /*   sql = "TRUNCATE TABLE [file]";
        connection = new SqlConnection(connetionString);
        connection.Open();
        command = new SqlCommand(sql, connection);
        command.ExecuteReader();
        connection.Close();*/
        sql = "Select * from [file] WHERE person='" + Session["user"].ToString()+"'";
        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
        try
        {
            connection = new SqlConnection(connetionString);

            connection.Open();
            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                int i = 0;
                if (!IsPostBack)
                {
                    
                    DropDownList1.Items.Add(new ListItem(dataReader.GetValue(1).ToString(), dataReader.GetValue(1).ToString()));
                    //RadioButtonList1.Items.Add(new ListItem(dataReader.GetValue(1).ToString(), dataReader.GetValue(1).ToString()));
                    //radio1.Items.Add(new ListItem("Apple", "1"));
                }
                dataReader.GetValue(3).ToString();
                //Label1.Text = Label1.Text + "" + dataReader.GetValue(1) + dataReader.GetValue(2);
                //MessageBox.Show(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2)); } 
                
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
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if(RadioButton1.Checked)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSAParameters rsaParams = RSA.ExportParameters(false);
                        
            int modulus = rsaParams.Modulus.Length;
            int block_size = modulus - 11;

            string privatekey = RSA.ToXmlString(true);
            string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

            string encr_path = Server.MapPath("~/Files/") + fileName  + fileExtension;
            string decr_path = Server.MapPath("~/Files/") + fileName + "_dec" + fileExtension;
            
            byte[] plaintext = FileUpload1.FileBytes;
            int datalength = plaintext.Length;

            int lastBlocIndex = block_size * (datalength / block_size);
            int lastBlocLength = datalength % block_size;

            byte[] encryptedData, decryptedData;
            byte[] tempData = new byte[block_size];
            byte[] a = new byte[128];
            
            List<byte> processlist = new List<byte>();
            for (int index = 0; index < lastBlocIndex; index += block_size)
            {
                Array.Copy(plaintext, index, tempData, 0, block_size);
                processlist.AddRange(RSAEncrypt(tempData, RSA.ExportParameters(false), false));

            }
            
            if (lastBlocLength != 0)
            {
                Array.Copy(plaintext, lastBlocIndex, tempData, 0, lastBlocLength);
                Array.Resize(ref tempData, lastBlocLength);
                processlist.AddRange(RSAEncrypt(tempData, RSA.ExportParameters(false), false));

            }
            encryptedData = processlist.ToArray();
            
            string publicPrivateKey = RSA.ToXmlString(true);
            Label2.Text = publicPrivateKey;

            FileStream encr = new FileStream(encr_path, FileMode.Create);
            encr.Write(encryptedData, 0, encryptedData.Length);
            encr.Close();
            stopwatch.Stop();
            
            Session["et"]= stopwatch.ElapsedMilliseconds.ToString();
            rsaParams = RSA.ExportParameters(true);
            dbconn(fileName, Label1.Text, encr_path, RadioButton1.Text, privatekey, rsaParams.D.ToString(), stopwatch.ElapsedMilliseconds.ToString(), fileExtension);
            SqlConnection connection;
            SqlCommand command;
            
            string connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";  
            //sql = "INSERT INTO [file] (dtime) VALUES ('" + stopwatch.ElapsedMilliseconds.ToString() + "')" + " Where filename='"+RadioButtonList1.SelectedValue.ToString()+"'";
            string sql = "UPDATE [file] SET dtime='" + stopwatch.ElapsedMilliseconds.ToString() + "'" + " Where filename='"+fileName+"'";
            //UPDATE components SET Quantity=x WHERE components.sno='y'  
            //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
            connection = new SqlConnection(connetionString);

            connection.Open();
            command = new SqlCommand(sql, connection);
            command.ExecuteReader();

            command.Dispose();
            connection.Close();
            Session["text"] =  System.Text.Encoding.UTF8.GetString(encryptedData);
            
        
        
        }
        else if (RadioButton2.Checked)
        {

            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
            //your sample code
            //System.Threading.Thread.Sleep(500);
            System.Threading.Thread.Sleep(500);
            // Must be 64 bits, 8 bytes.
            // Distribute this key to the user who will decrypt this file.
            string sSecretKey;

            // Get the key for the file to encrypt.
            //sSecretKey = GenerateKey();
            //Console.Write(sSecretKey);
            string fileName = Path.GetFileNameWithoutExtension(FileUpload1.PostedFile.FileName);
            string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);

            string input = Server.MapPath("~/Files/") + fileName + fileExtension;
            string output = Server.MapPath("~/Files/") + fileName + "_enc" + fileExtension;
            string de = Server.MapPath("~/Files/") + fileName + "dec" + fileExtension;
            //string f = FileUpload1.FileName;
            //string e=FileUpload1.
            sSecretKey = "\fB???E??Z|$\0?G?????m??";
            string initVector = "HR$2pIjH";
            FileUpload1.SaveAs(input);
            // For additional security pin the key.
            //GCHandle gch = GCHandle.Alloc(sSecretKey, GCHandleType.Pinned);

            // Encrypt the file.        
            EncryptFile(input, output, sSecretKey);
            // Decrypt the file.
           
            //DecryptFile(output, de, sSecretKey);
            File.Delete(input);
            Label2.Text += "File Encrypted";
            stopwatch.Stop();
            Session["et"] = stopwatch.ElapsedMilliseconds.ToString();
            FileStream fsInput = new FileStream(output, FileMode.Open, FileAccess.Read);
            byte[] encryptedData = new byte[fsInput.Length];
            fsInput.Read(encryptedData, 0, encryptedData.Length);
            Session["text"] = System.Text.Encoding.UTF8.GetString(encryptedData);
            string user = Session["user"].ToString();

            dbconn(fileName, user, output, RadioButton2.Text, sSecretKey, initVector, stopwatch.ElapsedMilliseconds.ToString(), fileExtension);
            //TextBox1.Text = Server.MapPath("~/Files/");
            //TextBox2.Text = stopwatch.ElapsedMilliseconds.ToString();
            fsInput.Close();
     
        }
        else
        {
            Label2.Text = "Kindly select an encryption algorithm type";
        }
        //TextBox1.Text = "asdjkashkdjhaskd";
        Response.Redirect("~\\/Default2.aspx");

    }

    public void dbconn(string filename, string user, string filepath, string algo, string key, string iv, string time, string fileExtension)
    {
        
        string connetionString = null;//"ConnectionString";
        SqlConnection connection;
        SqlCommand command;
        string sql = null;

        //SqlDataReader dataReader;
        //connetionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2010\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True;User Instance=True";
        connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";  
        sql = "INSERT INTO [file] (filename,person,filepath,algo,pkey,iv,etime,ext) VALUES ('" + filename + "','" + user + "','" + filepath + "','" + algo + "','" + key + "','" + iv + "','" + time + "','" + fileExtension + "')";
        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
        try
        {
            connection = new SqlConnection(connetionString);

            connection.Open();
            command = new SqlCommand(sql, connection);
            command.ExecuteReader();
            
            command.Dispose();
            connection.Close();

        }
        catch (Exception ex)
        {
            Label1.Text = "Exception:" + ex;
            //MessageBox.Show("Can not open connection ! "); 
        }

    }
    static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
    {
        FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
        FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);

        //DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        //TripleDES DES = TripleDES.Create();
        string initVector = "HR$2pIjH";
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        DES.IV = ASCIIEncoding.ASCII.GetBytes(initVector);

        ICryptoTransform desencrypt = DES.CreateEncryptor();

        CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
        byte[] bytearrayinput = new byte[fsInput.Length];
        fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
        //StreamWriter sWriter = new StreamWriter(cryptostream);
        //sWriter.WriteLine(Data);
        cryptostream.Close();
        //fsEncrypted.Flush();
        //fsEncrypted.Close();
        fsInput.Close();
        fsInput.Close();
        //fsEncrypted.Close();


    }
    static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
    {
        TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
        //Set initialization vector.
        string initVector = "HR$2pIjH";
        DES.IV = ASCIIEncoding.ASCII.GetBytes(initVector);

        //Create a file stream to read the encrypted file back.
        FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
        //Create a DES decryptor from the DES instance.
        ICryptoTransform desdecrypt = DES.CreateDecryptor();
        //Create crypto stream set to read and do a 
        //DES decryption transform on incoming bytes.
        CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
        //Print the contents of the decrypted file.
        StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
        fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
        fsDecrypted.Flush();
        fsDecrypted.Close();
        fsread.Close();
    }

    static public byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try
        {
            byte[] encryptedData;
            
            //Create a new instance of RSACryptoServiceProvider. 
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs 
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        //Catch and display a CryptographicException   
        //to the console. 
        catch (CryptographicException e)
        {
            string p = e.Message;
            //string o = "asdasdasd";
            return null;
        }

    }
    static public byte[] RSADecrypt(byte[] DataToDecrypt, string privatekey, bool DoOAEPPadding)
    {
        try
        {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider. 
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(1024))
            {
                //Import the RSA Key information. This needs 
                //to include the private key information.
                //RSA.ImportParameters(RSAKeyInfo);
                RSA.FromXmlString(privatekey);

                //Decrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;
        }
        //Catch and display a CryptographicException   
        //to the console. 
        catch (CryptographicException e)
        {
            string p = e.Message;
            //string o = "asdasdasd";

            return null;
        }

    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        string nnn = null;
        
        //TextBox1.Text = "decrypt start";
       //TextBox1.Text=nnn;
            
        try
        {   
            string file=null;
            string connetionString = null;//"ConnectionString";
            SqlConnection connection;
            SqlCommand command;
            string sql = null;

            SqlDataReader dataReader;
            //connetionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2010\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True;User Instance=True";
            connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";
            sql = "Select * from [file] WHERE filename='" + DropDownList1.SelectedValue.ToString() + "'";
            //sql = "Select * from [file] WHERE filename='" + RadioButtonList1.SelectedValue.ToString() + "'";
            connection = new SqlConnection(connetionString);
            
            connection.Open();
            command = new SqlCommand(sql, connection);
            dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    
                    if (dataReader.GetValue(4).ToString() == "Triple DES")
                    {
                        Session["decrtype"] = "Triple DES";
                        Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                        string sSecretKey;
                        string de = Server.MapPath("~/Files/temp") + dataReader.GetValue(9).ToString();
                        file= dataReader.GetValue(9).ToString();
                        sSecretKey = "\fB???E??Z|$\0?G?????m??";
                        //string initVector = "HR$2pIjH";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + dataReader.GetValue(1).ToString());
                        DecryptFile(dataReader.GetValue(3).ToString(), de, sSecretKey);
                        stopwatch.Stop();
                        Session["dt"] = stopwatch.ElapsedMilliseconds.ToString();
                        Response.ContentType="Plain/text";
                
                        //Response.WriteFile(de);
                        command.Dispose();
                        sql = "UPDATE [file] SET dtime='" + stopwatch.ElapsedMilliseconds.ToString() + "'" + " Where filename='" + DropDownList1.SelectedValue.ToString() + "'";
                        //sql = "UPDATE [file] SET dtime='" + stopwatch.ElapsedMilliseconds.ToString() + "'" + " Where filename='" + RadioButtonList1.SelectedValue.ToString() + "'";
                        //UPDATE components SET Quantity=x WHERE components.sno='y'  
                        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
                        connection = new SqlConnection(connetionString);

                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.ExecuteReader();

                        command.Dispose();
                        FileStream fsInput = new FileStream(de, FileMode.Open, FileAccess.Read);
                        byte[] encryptedData = new byte[fsInput.Length];
                        fsInput.Read(encryptedData, 0, encryptedData.Length);
                        string text = System.Text.Encoding.UTF8.GetString(encryptedData);
                        Session["text"] = text;
                        //File.Delete(de);
                        fsInput.Close();

                    }
                    else if (dataReader.GetValue(4).ToString() == "RSA")
                    {
                        Session["decrtype"] = "RSA";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + dataReader.GetValue(1).ToString());
                        Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                        string privatekey = dataReader.GetValue(5).ToString();
                         //creates and start the instance of Stopwatch
                        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                        RSAParameters rsaParams = RSA.ExportParameters(false);
                        int modulus = rsaParams.Modulus.Length;
                        int block_size = modulus - 11;

                        //Get the Input File Name and Extension.
                        string fileName = DropDownList1.SelectedValue.ToString();// +dataReader.GetValue(9).ToString();
                        //string fileName = RadioButtonList1.SelectedValue.ToString();// +dataReader.GetValue(9).ToString();
                        string fileExtension = dataReader.GetValue(9).ToString();

                        //Build the File Path for the original (input) and the encrypted (output) file.
                        //string input = Server.MapPath("~/Files/") + fileName + fileExtension;
                        //string encr_path = Server.MapPath("~/Files/") + fileName + "_enc" + fileExtension;
                        string decr_path = Server.MapPath("~/Files/") + fileName + "_dec" + fileExtension;
                        //te.Text = decr_path;
                        //string output2 = Server.MapPath("~/Files/") + fileName + "_cloud" + fileExtension;

                        //string input1 = Server.MapPath("~/Files/h.txt");

                        //get byte of input file
                        byte[] encryptedData = null;
                        string filename1 = dataReader.GetValue(3).ToString();
                        FileStream f = new FileStream(filename1, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(f);
                        long numBytes = f.Length;//new FileInfo(fileName).Length;
                        encryptedData = br.ReadBytes((int)numBytes);
                        //FileUpload1.FileBytes;// ByteConverter.GetBytes(fileName);

                        //int datalength = encryptedData.Length;
                        //get number of required to store input file, with each block 100 bytes of data
                        //int lastBlocIndex = block_size * (datalength / block_size);
                        //get lenth of last block if it's not a multiple of 100
                        //int lastBlocLength = datalength % block_size;

                        //Save the Input File, Encrypt it and save the encrypted file in output path.
                        //FileUpload1.SaveAs(input);
                        //te.Text += datalength.ToString() + " " + lastBlocIndex.ToString();

                        //byte[] encryptedData, decryptedData;
                        byte[] decryptedData;
                        byte[] tempData = new byte[block_size];
                        byte[] a = new byte[128];
                        //byte[] tempProcessData = new byte[100];
                        List<byte> processlist = new List<byte>();
                        int len = encryptedData.Length;
                        Array.Resize(ref tempData, modulus);

                        int no = encryptedData.Length / modulus;
                        //processlist.Clear();
                        for (int ind = 0; ind < len; ind += modulus)
                        {
                            Array.Copy(encryptedData, ind, tempData, 0, modulus);
                            //processlist.AddRange(RSADecrypt(tempData, RSA.ExportParameters(true), false));
                            processlist.AddRange(RSADecrypt(tempData, privatekey, false));
                        }
                        decryptedData = processlist.ToArray();
                        //dbconn(FileUpload1.FileName.ToString(), Session['u'],encr_path,RadioButton1.Text,Label1.Text);

                        FileStream decr = new FileStream(decr_path, FileMode.Create);
                        decr.Write(decryptedData, 0, decryptedData.Length);
                        decr.Close();
                        stopwatch.Stop();
                        Session["dt"] = stopwatch.ElapsedMilliseconds.ToString();
                        string text = System.Text.Encoding.UTF8.GetString(decryptedData);
                        Session["text"] = text;
                        //dbconn(fileName, Label1.Text, decr_path, RadioButton1.Text, privatekey, rsaParams.D.ToString(), stopwatch.ElapsedMilliseconds.ToString(), fileExtension);
                        
                        //sql = "INSERT INTO [file] (dtime) VALUES ('" + stopwatch.ElapsedMilliseconds.ToString() + "')" + " Where filename='"+RadioButtonList1.SelectedValue.ToString()+"'";
                        sql = "UPDATE [file] SET dtime='" + stopwatch.ElapsedMilliseconds.ToString() + "'" + " Where filename='" + fileName + "'";
                        //UPDATE components SET Quantity=x WHERE components.sno='y'  
                        //string conn = ConfigurationManager.ConnectionStrings["SQLDbConnection"].ToString();
                        connection = new SqlConnection(connetionString);
                        f.Close();
                        connection.Open();
                        command = new SqlCommand(sql, connection);
                        command.ExecuteReader();

                        command.Dispose();
                        connection.Close();
                        Response.Clear();
                        Response.ContentType = "Text/Plain";
                        Response.WriteFile(decr_path);
                
                    }
                    Label2.Text = dataReader.GetValue(3).ToString();
                    
                 
                }
                dataReader.Close();
                command.Dispose();
                connection.Close();
                
                Response.Redirect(Request.RawUrl, true);         
                Response.Flush();
                
                    
        }
        catch (Exception ex)
        {
                Label1.Text = "Exception:" + ex;
        }
        //Response.Redirect("~\\/Default2.aspx");
        //Response.AppendHeader("Refresh", 0+ "; URL=\\Default2.aspx");
        //Response.Redirect("~\\/Default2.aspx");
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Array.ForEach(Directory.GetFiles(Server.MapPath("~/Files/")),File.Delete);
        string connetionString = null;//"ConnectionString";
        SqlConnection connection;
        SqlCommand command;
        string sql = null;

        //connetionString = "Data Source=.\\SQLEXPRESS;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2010\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True;User Instance=True";
        connetionString = "Data Source=tcp:c62xyeqw26.database.windows.net,1433;Initial Catalog=minorProject_db;User Id=minor@c62xyeqw26;Password=Saurabh@12";
        //"Data Source=(LocalDB)\v11.0;AttachDbFilename='C:\\Users\\root\\Documents\\Visual Studio 2012\\WebSites\\minorproject\\App_Data\\Database.mdf';Integrated Security=True";
        sql = "TRUNCATE TABLE [file]";
        connection = new SqlConnection(connetionString);
        connection.Open();
        command = new SqlCommand(sql, connection);
        command.ExecuteReader();
        connection.Close();
        //TextBox1.Text = "asdjkashkdjhaskd";
        Response.Redirect("~\\/Default2.aspx");
        Response.AppendHeader("Refresh", 30 + "; URL=\\Default2.aspx");
    }
}