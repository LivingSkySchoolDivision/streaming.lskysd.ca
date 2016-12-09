<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

        <div class="liveWrap" style=" border-radius: 4px;">
        <asp:Literal ID="litLiveStreams" Visible="false" runat="server"></asp:Literal></div>
    
        <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Literal ID="litPlayer" runat="server"></asp:Literal>
            <div style="text-align: left">
                <asp:Literal ID="litStreamInfo" runat="server"></asp:Literal>
            </div>
        </div>
    
    <div runat="server" id="title_upcoming" class="front_page_heading" visible="false">Upcoming live broadcasts</div> 
    <asp:Literal ID="litUpcomingStreams" Visible="false" runat="server"></asp:Literal>
    
    <div class="newestWrapOuter">
        <div class="newestWrapInner">
            <asp:Literal ID="litNewestVideos" Visible="false" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
