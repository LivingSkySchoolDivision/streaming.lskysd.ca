<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.player.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:Table ID="tblContainer" CssClass="player_container" runat="server" Visible="false">
        <asp:TableRow>
            <asp:TableCell><asp:Literal ID="litPlayer" runat="server"></asp:Literal></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Literal ID="litVideoInfo" runat="server"></asp:Literal>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblNotFound" runat="server" HorizontalAlign="Center" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <div class="large_infobox">Video not found</div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    
</asp:Content>
