<%@ Page Language="C#" MasterPageFile="~/Master/Site.Master" AutoEventWireup="true" CodeBehind="BillEntry.aspx.cs" Inherits="Electricity_Board_Billing_Prj.BillEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Customer Electricity Bill Entry</h2>

    <div id="divNumberOfBills" runat="server">
        <div class="form-row">
            <label>Enter how many Number of Bills you want to insert:</label>
            
            <asp:TextBox ID="txtNumberOfBills" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnStartEntry" runat="server" CssClass="btn" Text="Start Entry" OnClick="btnStartEntry_Click" />
    </div>

    <div id="divBillEntry" runat="server" visible="false">
        <h3>Enter Customer Bill Details</h3>
        <div class="form-row">
            <label>Consumer Number:</label>
            <asp:TextBox ID="txtConsumerNumber" runat="server"></asp:TextBox>
        </div>
        <div class="form-row">
            <label>Consumer Name:</label>
            <asp:TextBox ID="txtConsumerName" runat="server"></asp:TextBox>
        </div>
        <div class="form-row">
            <label>Units Consumed:</label>
            <asp:TextBox ID="txtUnitsConsumed" runat="server"></asp:TextBox>
        </div>
        <asp:Button ID="btnAddBill" runat="server" CssClass="btn" Text="Add Bill" OnClick="btnAddBill_Click" />
        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-danger" Text="Reset" OnClick="btnReset_Click" />

        <div class="form-row">
            <strong>Total Bills:</strong> <asp:Label ID="lblTotalBills" runat="server" />
            &nbsp;&nbsp; <strong>Current Bill:</strong> <asp:Label ID="lblCurrentBill" runat="server" />
        </div>
    </div>

    <div id="divResults" runat="server" visible="false">
        <h3> Processed Bills</h3>
        <asp:Label ID="lblResults" runat="server" />
        <br /><br />
        <asp:Button ID="btnStartOver" runat="server" CssClass="btn" Text="Start Over" OnClick="btnStartOver_Click" Visible="false" />
    </div>

    <div class="form-row">
        <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
    </div>
     <footer>
        &copy; 2025 Electricity Board Billing System. All rights reserved.
    </footer>
</asp:Content>
