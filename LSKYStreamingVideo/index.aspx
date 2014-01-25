<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    
    <div runat="server" id="title_live" class="front_page_heading green" visible="false">Live broadcasts</div>
    <div style=" border-radius: 4px;">
    <asp:Literal ID="litLiveStreams" Visible="false" runat="server"></asp:Literal>
    </div>
    <br /><br />
    <div runat="server" id="title_upcoming" class="front_page_heading" visible="false">Upcoming live broadcasts</div>
    <asp:Literal ID="litUpcomingStreams" Visible="false" runat="server"></asp:Literal>
    <br /><br />
    <div runat="server" id="title_featured" class="front_page_heading" visible="false">Featured videos</div>
    <asp:Literal ID="litFeaturedVideos" Visible="false" runat="server"></asp:Literal>
    <br /><br />
    <div runat="server" id="title_newest" class="front_page_heading" visible="false">Newest videos</div>
    <asp:Literal ID="litNewestVideos" Visible="false" runat="server"></asp:Literal>
    
</asp:Content>
