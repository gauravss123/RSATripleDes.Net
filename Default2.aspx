<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="Default2.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID ="div" ContentPlaceHolderID="div" runat="server">
    <div runat="server" id="content123" >
    DECRYPTED OR ENCRYPTED DATA IS DISPLAYED HERE    
    </div>
    <!--<asp:TextBox ID="TextBox1" runat="server"  Width="573px" TextMode="MultiLine" ></asp:TextBox>    -->

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
    &nbsp;
    
    <div id="rad">
    <asp:RadioButton ID="RadioButton1" runat="server" GroupName="file" Text="RSA" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="file" Text="Triple DES" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    Encryption Time : 
    <asp:Label ID="Label3" runat="server"></asp:Label>
    </div>
    <asp:LinkButton ID="Button1" runat="server" onclick="Button1_Click" Text="Encrypt" />
    <div id="selectfile" >
    SelectFile:&nbsp; <asp:FileUpload ID="FileUpload1" runat="server" Width="278px" />
    </div>
    </asp:Content>

<asp:Content  ID="Content4" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">
    <div id="decdiv" >
    Decrypted Technique: <asp:Label ID="Label6" runat="server" Text="No Data"/>
    &nbsp;Decryption Time :
    <asp:Label ID="Label4" runat="server"></asp:Label>
    </div>
    <div id="decfilelist">
        Select File: <asp:DropDownList  ID="DropDownList1" runat="server" Width="100px" AppendDataBoundItems="true"></asp:DropDownList>
         
    </div>
    <asp:LinkButton ID="Button2" runat="server" Text="Decrypt" onclick="Button2_Click" />
    <asp:LinkButton ID="Button3" runat="server" Text="Clear database" onclick="Button3_Click" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<!--    <p style="height: 47px; width: 553px">
        <br />
        WELCOME&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </p>
    <p>
        <asp:FileUpload ID="FsileUpload1" runat="server" />
    </p>
    <p>
        &nbsp;</p>
    <asp:RadioButton ID="RadioButton7" runat="server" GroupName="file" Text="RSA" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:RadioButton ID="RadioButton8" runat="server" GroupName="file" 
        Text="Triple DES" />
    <br />
    <asp:Label ID="Label2" runat="server"></asp:Label>
    <br />
    Encryption Time :
    <asp:Label ID="Label33" runat="server"></asp:Label>
    <br />
    Decryption Time :
    <asp:Label ID="Label43" runat="server"></asp:Label>
    <p>
        <asp:Button ID="Butto2n1" runat="server" onclick="Button1_Click" Text="UPLOAD" />
    </p>
    <asp:RadioButtonList ID="RadioButtonList1" runat="server" Height="18px">
</asp:RadioButtonList>
<asp:SqlDataSource ID="SqlDataS2ource1" runat="server" 
    ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
    SelectCommand="SELECT * FROM [file]"></asp:SqlDataSource>
    <p>
        <asp:Button ID="Button22" runat="server" Text="Decrypt" 
            onclick="Button2_Click" />
</p>
    <p>
        <asp:Button ID="Button23" runat="server" Text="Clear database" 
            onclick="Button3_Click" />
        &nbsp;</p>-->
</asp:Content>

