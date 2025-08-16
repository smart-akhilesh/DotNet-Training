<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="Asp_Assignment_1.Products" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Products Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width:400px; margin:auto;">
            <h2>Select a Product</h2>

            <asp:DropDownList ID="ddlProducts" runat="server" AutoPostBack="true"
                OnSelectedIndexChanged="ddlProducts_SelectedIndexChanged">
            </asp:DropDownList>
            <br /><br />

            <asp:Image ID="imgProduct" runat="server" Width="200" Height="200" />
            <br /><br />

            <asp:Button ID="btnGetPrice" runat="server" Text="Get Price" OnClick="btnGetPrice_Click" />
            <br /><br />

            <asp:Label ID="lblPrice" runat="server" Font-Bold="true" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
