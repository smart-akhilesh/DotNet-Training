<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validator.aspx.cs" Inherits="Asp_Assignment_1.Validator" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Validator Assignment</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width:400px; margin:auto;">
            <h2>Validation Form</h2>

            Name:<br />
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="Name" runat="server" ControlToValidate="txtName"
                ErrorMessage="* Required" ForeColor="Red"></asp:RequiredFieldValidator>
            <br /><br />

            Family Name:<br />
            <asp:TextBox ID="txtFamily" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="customvalidate" runat="server" 
                ControlToValidate="txtFamily"  OnServerValidate="NameFamily_ServerValidate"
                ErrorMessage="Family Name required and must be different from Name" ForeColor="Red" ValidateEmptyText="True">
            </asp:CustomValidator>
            <br /><br />

            Address:<br />
            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="Address" runat="server" ControlToValidate="txtAddress"
                ValidationExpression="^.{2,}$" ErrorMessage="At least 2 letters" ForeColor="Red"></asp:RegularExpressionValidator>
            <br /><br />

            City:<br />
            <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="City" runat="server" ControlToValidate="txtCity"
                ValidationExpression="^.{2,}$" ErrorMessage="At least 2 letters" ForeColor="Red"></asp:RegularExpressionValidator>
            <br /><br />

            Zip Code:<br />
            <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="Zip" runat="server" ControlToValidate="txtZip"
                ValidationExpression="^\d{5}$" ErrorMessage="Zip must be 5 digits" ForeColor="Red"></asp:RegularExpressionValidator>
            <br /><br />

            Phone:<br />
            <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="Phone" runat="server" ControlToValidate="txtPhone"
                ValidationExpression="^\d{2}-\d{7}$|^\d{3}-\d{7}$"
                ErrorMessage="Format: XX-XXXXXXX or XXX-XXXXXXX" ForeColor="Red"></asp:RegularExpressionValidator>
            <br /><br />

            Email:<br />
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="Email" runat="server" ControlToValidate="txtEmail"
                ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                ErrorMessage="Invalid email" ForeColor="Red"></asp:RegularExpressionValidator>
            <br /><br />

          

            <asp:Button ID="btnCheck" runat="server" Text="Check" OnClick="btnCheck_Click" />
            <br /><br />

            <asp:Label ID="lblResult" runat="server" ForeColor="Green"></asp:Label>
        </div>
    </form>
</body>
</html>
