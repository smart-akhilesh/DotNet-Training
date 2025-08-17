<%@ Page Language="C#" MasterPageFile="~//Master/Site.Master" AutoEventWireup="true" CodeBehind="ViewLastBills.aspx.cs" Inherits="Electricity_Board_Billing_Prj.ViewLastBills" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Enter the number of bills you want to view </h2>

    <div class="form-row">
        <label>Enter numbe of bills you want to retrieve:</label>
        <asp:TextBox ID="txtLastNBills" runat="server"></asp:TextBox>
        <asp:Button ID="btnViewLastBills" runat="server" CssClass="btn" Text="View" OnClick="btnViewLastBills_Click" />
    </div>

    <div class="form-row">
        <asp:Label ID="lblLastBills" runat="server"></asp:Label>
    </div>
     <footer>
        &copy; 2025 Electricity Board Billing System. All rights reserved.
    </footer>
</asp:Content>
