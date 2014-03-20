<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.videos.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        ul {
            list-style: none;
            margin: 0;
            margin-left: 15px;
            padding: 0;
            font-size: 16pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">    
    <div class="searchBox">
        <div class="searchTitle">Search Videos</div>
        <asp:TextBox CssClass="searchTextBox" ID="txtSearchTerms" runat="server" AutoPostBack="true" OnTextChanged="btnSearch_Click"></asp:TextBox>
        <asp:Button CssClass="searchButton" ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
    </div>
    <h2 runat="server" id="searchResultsTitle" visible="false">Search Results</h2>
    <div style="margin-left: 10px;">
    <asp:Literal ID="litSearchResults" runat="server" Visible="false"></asp:Literal>
    </div>
    <div style="margin-left: 10px;">
        <h2>Video Categories</h2>
        <asp:Literal ID="litCategories" runat="server"></asp:Literal>
        <asp:Literal ID="litVideos" runat="server"></asp:Literal>       
        
    </div>
    

</asp:Content>
