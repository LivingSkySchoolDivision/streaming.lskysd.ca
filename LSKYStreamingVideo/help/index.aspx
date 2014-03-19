<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.help.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>Live stream ended prematurely?</h2>
    <p>
        If our "on-location" streaming equipment fails for any reason (if it gets hit by a basketball, or if someone unplugs it to charge their cell phone, for example), the stream may get interrupted prematurely. This can cause the video player in your browser to assume that the event has finished, when in fact it hasn't. If you think this may have happened, try waiting about 10 seconds and reloading the page in your browser, and the video should resume playback.
    </p>
    <p>
        <b style="color: darkred;">When in doubt, try reloading the page.</b>
    </p>
    <br /><br />

    <h2>Missed part of a stream?</h2>
    <p>
        In most cases, we make the recordings of our live broadcasts available in the "videos" section of this website within a few days, and they are often made available for direct download at this time as well.
    </p>

    <br /><br />
    <h2>Video player won't load in your browser?</h2>
    <p>
        We use two types of video players on this website (Silverlight and HTML5), and we do our best to automatically determine which will work best for you. 
    </p>
    <p>
        Underneath each video player, there is a link that you can use to switch from our Silverlight player to our HTML5 player. If you are experiencing issues getting a video or live broadcast to play in one player, try switching to the other one.
    </p>
    <p>
        <b>Our live broadcast player currently only works on the following platforms:</b>
        <ul>
            <li>Windows computers with Microsoft Silverlight 4 or above (use Silverlight player)</li>
            <li>Windows computers using the Google Chrome browser (use HTML5 player)</li>
            <li>Apple computers using the Google Chrome browser (use HTML5 player)</li>
            <li>Apple computers with Safari (use HTML5 player)</li>
            <li>iOS devices (iPad/iPod/iPhone) with Safari (use HTML5 player)</li>
            <li>Android browser on Android devices(use HTML5 player)</li>
        </ul>   
    </p>
    <p>
        We would like to see our live broadcasts available to everyone, using any browser or player, but unfortunately the technology that we are using to stream isn't quite there yet.
    </p>
    <p>
        Our pre-recorded videos are available in almost every browser and platform, however. If you are unable to view a broadcast live, in most cases you can watch the pre-recorded video once we have edited it and encoded it (usually this takes about a day). Pre-recorded videos of our streams are available in the <a href="/Videos">Videos section</a>.               
    </p>
</asp:Content>
