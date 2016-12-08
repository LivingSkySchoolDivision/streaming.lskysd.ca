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
</asp:Content>
