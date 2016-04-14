<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">

        <div runat="server" id="title_live" class="front_page_heading green" visible="false">Live broadcasts</div>
        <div class="liveWrap" style=" border-radius: 4px;">
        <asp:Literal ID="litLiveStreams" Visible="false" runat="server"></asp:Literal>
        </div>
    
    <!-- @todo -->

    <!--

        example:

    <li class="upcomingLive">
                <div class="front_page_heading" id="Body_title_upcoming">Upcoming live broadcasts</div>
                <div class="index_date_display">Friday, June 03, 2016</div><div class="upcomingStream"><ul class="upcomingStreamUL"><li><ul valign="top" width="25%"><li class="upcoming_stream_time"><b>7:00 PM</b></li></ul><ul valign="top"><div class="upcoming_stream_name">Macklin Graduation</div><div class="upcoming_stream_info">Expected duration: 3 hours</div><div class="upcoming_stream_info">Streaming from: Macklin</div><br></ul></li></ul><br>
        </div></li>
  -->

    <!--featuredWrap-->
    <div class="featuredWrap">
        <ul>
            <li class="featuredLive">
                    <div runat="server" id="title_featured" class="front_page_heading" visible="false">Featured videos</div>
                    <div style="margin-left: 5px;margin-right: 5px;">
                        <asp:Literal ID="litFeaturedVideos" Visible="false" runat="server"></asp:Literal>                        
                    </div>                    
            </li>
            <li class="upcomingLive">
                    <div runat="server" id="title_upcoming" class="front_page_heading" visible="false">Upcoming live broadcasts</div>
                    <asp:Literal ID="litUpcomingStreams" Visible="false" runat="server"></asp:Literal>
            </li>
        </ul>
    </div>


    <div class="newestWrapOuter">
        <div runat="server" id="title_newest" class="front_page_heading" visible="false">Newest videos</div>
        <div class="newestWrapInner">
            <asp:Literal ID="litNewestVideos" Visible="false" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
