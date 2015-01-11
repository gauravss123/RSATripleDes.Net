<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <label class="text">USERNAME:</label> 
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <label class="text1">PASSWORD:</label> 
    <asp:Label ID="Label1" runat="server" ></asp:Label>
    <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
    <asp:LinkButton ID="LinkButton1" onclick="Button1_Click" runat="server">Login</asp:LinkButton>
<br />
</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <link href="Default.css" rel="stylesheet" type="text/css" />
</asp:Content>


