<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.about.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        h2 {
            border: 0;
            border-bottom: 1px solid #C0C0C0;
        }

        h3 {
            margin-left: 5px;
            margin-bottom: 0;
        }

        p {
            margin-left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h1>What do we use to broadcast video?</h1>
    
    <h2>Manpower</h2>
    <img src="uchs_j20_logo_small.png" style="float: right;"/>
    <p>
        Our streaming equipment can be operated with as few as one staff member, however we try to involve students whenever we can. Most of our live broadcast events have students running the streaming equipment on site, updating titles and scoreboards, handling camera work, conducting interviews, and more.
    </p>
    <p>
        Out TriCaster allows us to capture live video wirelessly from iPads and wireless enabled cameras, allowing students to conduct interviews on the sidelines of events, and broadcast that footage in real time to our streaming equipment.
    </p>
    <p>
        We often work closely with students enrolled in Journalism or Communications Media courses within our school division.
    </p>
    <br />
    <h2>Hardware: Our "Streaming Video Kit"</h2>
    <img src="streaming_equipment_uchsindia.png" style="float: right;"/>
    
    <h3><a href="http://newtek.com/products/tricaster-40.html">NewTek TriCaster 40 (DCXD40)</a></h3>
    <p>
        This easy to use appliance allows us to switch between multiple audio/video sources (including wireless capable devices including iPads), set up chroma-keys and virtual sets, add titles and scoreboards, and generally improve the quality of our broadcasts. While we stream directly to our own servers, the TriCaster can be used to broadcast directly to popular streaming sites such as Livestream.com, Ustream, Justin.TV, and many more. The TriCaster 40's small form factor makes it very portable, so it is usually set up with the rest of our equipment on the sidelines of an event.
    </p>
        
    <h3>Dedicated Title Station</h3>
    <p>
        This is a standard Dell workstation NewTek LiveText software installed on it. The title station allows us to prepare and update titles which the TriCaster can dynamically apply to a video stream. We use this primarily for scoreboards at sporting events. The system requirements for this program are not high, so the model of this computer was chosen mostly for it's portability.
    </p>
    <p>
        The computer use for a title station is a Dell Optiplex 390 workstation with an Intel Core i5 3.1GHz processor, 8GB of RAM, and Windows 7 Professional 64-bit. We attach touch-enabled monitor (a 22" Dell ST2220Tc) to the title station for easier use (and to limit the keyboards and mice we need to have on-site).
    </p>

    <h3>Dedicated Encoding Station</h3>
    <p>
        This is a standard Dell workstation with Microsoft Expression Encoder 4 Pro software installed on it. This computer is responsible for taking the video and audio output from the TriCaster, and streaming it from the event location to our main streaming video servers in our datacenter.
    </p>
    <p>
        The computer use for a title station is a Dell Optiplex 390 workstation with an Intel Core i5 3.3GHz processor, 4GB of RAM, Windows 7 Professional 64-bit, and an <a href="http://www.avermedia-usa.com/AVerTV/product/ProductDetail.aspx?Id=482&">AVerTV HD DVR capture card</a>
    </p>
    <p>
        We often do two simultaneous streams for school sporting events (where two simultaneous games in the same gym is common), and so we have several of these encoding stations ready for different situations. Each live broadcast requires one encoding station, so two simultanous streams requires two encoding stations.
    </p>
    <h3>Cameras</h3>
    <p>
        Our cameras include a <a href="http://www.panasonic.com/business/provideo/AG-AC7.asp">Panasonic AG-AC7P</a> and a Sony HDR-CX460. We have also borrowed several cameras for certain events, including a <a href="http://gopro.com/cameras">GoPro HERO3</a>, a Sony HDR-CX260V, and a Sony HDR-CX360.
    </p>
    <p>
        The TriCaster allows us to wirelessly capture video from iPads, which we have used to capture interviews and other "roaming" content.
    </p>
    <h3>Microphones and Audio equipment</h3>
    <p>
        We primarily use our camera's built-in microphones or various USB microphones, but are looking to expand our audio equipment in the future.
    </p>

    <h3>Other Equipment</h3>
    <p>
        We also use a variety of miscellaneous equipment that doesn't fit into the above categories, including:
        <ul>
            <li>Several tripods</li>
            <li>A portable green screen</li>
            <li>Wireless HDMI transmitters and receivers</li>
            <li>Gigabit switches</li>
        </ul>
    </p>
    
    <br />
    <h2>Software</h2>    
    <h3>NewTek LiveText</h3>
    <p>
        Paired with the TriCaster, this program allows us to easily create on-screen titles, crawls, and lower thirds. This software is installed on our dedicated title station, which usually accompanies the TriCaster wherever it goes.
    </p>
    <p> 
        We use several custom developed-in-house applications designed to work alongside LiveText to create scoreboards for sporting events which can be updated with scores and timers on the fly.
    </p>
    <div style="text-align:center;">
        <img src="livetext.jpg" />
        <img src="title_helper.png" />
    </div>

    <h3>Microsoft IIS Smooth Streaming</h3>        
    <p>
        Microsoft's "Smooth Streaming" technology broadcasts several different versions of a streaming video at different levels of quality. The video player in a user's web browser will initially start the stream at the lowest quality video, and it will then dynamically adjust the quality of the video based on available bandwidth. This way the video stream starts faster, and works better for users with limited Internet speeds.
    </p>
    <p>
        Our encoding stations use Microsoft Expression Encoder to transmit the audio and video to this server, which then serves the video to the general public using our streaming website. 
    </p>
    <h3>Microsoft Expression Encoder 4 Pro</h3>
    <p>
        Our dedicated encoding stations transmit video from a source directly to our streaming server, and from there it gets served out to the public. Usually the encoding machines are set up to capture video and audio directly from our TriCaster, but we can also directly connect cameras and microphones to the encoding station using a <a href="http://www.hauppauge.com/">capture card</a> (when a second stream is required, for example).
    </p>

    <h3>Streaming Website</h3>
    <img src="stream_player.png" style="float: right;"/>
    <p>
        Our streaming video website was developed in-house, and was originally only designed to display our live broadcasts. The site has since evolved to support playing back recorded live streams, and is now capable of playing back any video file we add to it (student projects, for example).
    </p>
    <p>
        Streaming video is delivered to viewers using a custom Smooth Streaming player, developed in Microsoft Silverlight in-house. There are two silverlight players - one for pre-recorded video, and one for live broadcasts.
    </p>
    <p>
        We are also able to deliver a stream in HTML5, however due to the encoding that MS Smooth Streaming uses, it does not work properly on many popular browsers. Our live HTML5 player is primarily for Apple users (on MacOS or iOS devices), where Silverlight is not an option. Our pre-recorded video is available using HTML5, and should work on any popular web browser on any platform. It is our goal to exclusively use HTML5 video on this website in the future.
    </p>
    <br /><br /><br /><br />
    <br /><br /><br /><br />
</asp:Content>
