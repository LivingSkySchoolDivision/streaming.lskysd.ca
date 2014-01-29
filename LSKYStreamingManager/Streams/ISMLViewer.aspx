<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="ISMLViewer.aspx.cs" Inherits="LSKYStreamingManager.Streams.ISMLViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:Table ID="tblISMLList" runat="server">
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnView" runat="server" Text="View ISML stream" /></asp:TableCell>            
        </asp:TableRow>        
    </asp:Table>

</asp:Content>
