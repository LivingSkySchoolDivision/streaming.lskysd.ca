<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.videos.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        ul {
            list-style: none;
            margin: 0;
            margin-left: 5px;
            padding: 0;
            font-size: 10pt;
        }

        .video_category_meta {
            font-size: 10pt;
            color: #666666;
            display: inline;
        }

        .sidebar {
            padding: 5px;
            height: 100%;
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
    <br /><br /><br />
    <div style="margin-left: 10px;">

        <table border="0" width="100%">
            <tr>
                <td width="30%" style="vertical-align: top;"><div class="sidebar"><asp:Literal ID="litCategories" runat="server"></asp:Literal></div></td>
                <td width="70%" style="vertical-align: top;"><div class="content"><asp:Literal ID="litVideos" runat="server"></asp:Literal></div></td>
            </tr>
        </table>
            
              
        
    </div>
    

</asp:Content>
