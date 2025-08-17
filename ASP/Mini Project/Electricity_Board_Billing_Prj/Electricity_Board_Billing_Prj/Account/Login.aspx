<%@ Page Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Electricity_Board_Billing_Prj.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="form-card">
            <h2>Admin Login</h2>

            <div class="form-row">
                <label for="txtUsername">Username:</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="input-box" placeholder="Enter username"></asp:TextBox>
            </div>

            <div class="form-row">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="input-box" placeholder="Enter password"></asp:TextBox>
            </div>

            <div class="form-row" style="justify-content: center;">
                <asp:Button ID="btnLogin" runat="server" CssClass="btn" Text="Login" OnClick="btnLogin_Click" />
            </div>

            <div class="form-row" style="justify-content: center; margin-top:10px;">
                <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
            </div>
        </div>
    </div>

    <footer>
        &copy; 2025 Electricity Board Billing System. All rights reserved.
    </footer>

</asp:Content>
