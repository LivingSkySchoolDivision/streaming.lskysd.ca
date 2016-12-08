<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingManager.VideoCategories.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">    
    <h2>Add new category</h2>
    <asp:Table ID="tblNewCategory" runat="server" CssClass="datatable" CellPadding="5" Width="100%">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableCell>Name</asp:TableCell>
            <asp:TableCell>Hidden</asp:TableCell>
            <asp:TableCell>Private</asp:TableCell>
            <asp:TableCell>Parent</asp:TableCell>
            <asp:TableCell></asp:TableCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="txtNewCategoryName" runat="server" Width="95%"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:CheckBox ID="chkHidden" runat="server" />                
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:CheckBox ID="chkPrivate" runat="server" />                
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="drpParent" runat="server" Width="95%"></asp:DropDownList>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Button ID="btnNewCategory" runat="server" Text="Add" OnClick="btnNewCategory_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><br />
    <h2>Video Categories</h2>
    <asp:Table ID="tblCategories" runat="server" CssClass="datatable" CellPadding="5" Width="100%">
        <asp:TableHeaderRow CssClass="datatable_header">
            <asp:TableHeaderCell>Name</asp:TableHeaderCell>
            <asp:TableHeaderCell>Hidden</asp:TableHeaderCell>
            <asp:TableHeaderCell>Private</asp:TableHeaderCell>
            <asp:TableHeaderCell>ID</asp:TableHeaderCell>
            <asp:TableHeaderCell>Parent</asp:TableHeaderCell>
            <asp:TableHeaderCell>Menu Level</asp:TableHeaderCell>
            <asp:TableHeaderCell>#Videos</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
    

</asp:Content>
