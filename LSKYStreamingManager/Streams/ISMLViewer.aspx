<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="ISMLViewer.aspx.cs" Inherits="LSKYStreamingManager.Streams.ISMLViewer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:Table ID="tblISMLList" runat="server" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
                <asp:DropDownList ID="drpISMLFiles" runat="server"></asp:DropDownList>

            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="drpPlayers" runat="server"></asp:DropDownList>

            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="btnView" runat="server" Text="View ISML stream" OnClick="btnView_Click" /></asp:TableCell>            
        </asp:TableRow>        
    </asp:Table>

    <asp:Table ID="tblPlayer" runat="server" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell>
            <asp:Literal ID="litPlayer" runat="server"></asp:Literal>
            </asp:TableCell>
        </asp:TableRow>    
    </asp:Table>
</asp:Content>
