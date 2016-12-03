<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="NewVideo.aspx.cs" Inherits="LSKYStreamingManager.Videos.NewVideo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        p {
            font-size: 8pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>Add new video</h2>    
    <div style="text-align: center;">
        <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
    </div>
    <asp:Table ID="tblControls" runat="server" Width="100%" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell Width="350" VerticalAlign="Top">
                <b>Title</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtTitle" runat="server" Width="300">New video</asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Author</b>
                <p>Be as general or specific as you'd like here - can be a person's name, or simply "Living Sky School Division". If left blank, it won't be displayed</p>   
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtAuthor" runat="server" Width="300"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Location</b>
                <p>What physical location was this video recorded at? You can be as specific or generic as you'd like, or if you leave this blank it won't be displayed.</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtLocation" runat="server" Width="300"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Description</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Width="100%" Height="100"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>                
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Tags</b>
                <p>These are not displayed on the site, but are used with the search function to find files. Enter a few words that describe this video (example: football uchs 2011 warriors sports)</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtTags" runat="server" TextMode="MultiLine" Width="100%" Height="100"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Video dimensions</b>
                <p>The player on the website will be set to these dimensions. You should not exceed 1000 pixels wide, and it should not be smaller than 640x480.</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtWidth" runat="server" Width="75" Text="720"></asp:TextBox> x
                <asp:TextBox ID="txtHeight" runat="server" Width="75" Text="480"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Thumbnail</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Image ID="imgThumbnail" ImageUrl="/thumbnails/videos/none.png" CssClass="thumbnail_editor_video" runat="server" /><br />
                <asp:DropDownList ID="drpThumbnail" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpThumbnail_SelectedIndexChanged"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>    
        
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Duration (in seconds)</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtDuration" runat="server" Width="75"></asp:TextBox> seconds
            </asp:TableCell>
        </asp:TableRow>

        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Original Air Date</b>
                <p>Only displayed if the option "Display airdate" is selected below</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpAirDateYear" runat="server"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="drpAirDateMonth" runat="server"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtAirDateDay" runat="server" MaxLength="2" Width="25"></asp:TextBox> &nbsp;&nbsp;
                <asp:TextBox ID="txtAirDateHour" runat="server" MaxLength="2" Width="25"></asp:TextBox>:
                <asp:TextBox ID="txtAirDateMin" runat="server" MaxLength="2" Width="25"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>                   
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
                <br /><br />
                <b>File Formats</b><br />
                <p>You don't have to specify all file formats, but you should specify at least one. To accomodate most browsers you will need at least an ISM file and an H.264 transcoded MP4 file.</p>
                <p>Video files can be transferred to the streaming server using the "Video Files" windows file share. Sorry, no easy way to upload here yet!</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Youtube URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtYoutubeURL" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>    
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>ISM URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                /Video_files/<asp:TextBox ID="txtISMURL" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>        
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>MP4/H.264 URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
               /Video_files/<asp:TextBox ID="txtMP4URL" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>        
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>OGV/THEORA URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                /Video_files/<asp:TextBox ID="txtOGVURL" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>        
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>WEBM/VP8 URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                /Video_files/<asp:TextBox ID="txtWEBM" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>       
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Download URL</b>
                <p>If left blank, download will not be available. This does not have to be a video file - you can make this a ZIP file or something else if you wish.</p>                
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                /Video_files/<asp:TextBox ID="TextBox1" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>        
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Available from</b>
                <p>If you wish to "prestage" a video, and have it show up on the site at a later date, put that date in here. If you want the video to show up immediately, this should be set to today's date, or a date in the past.</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpAvailFromYear" runat="server" Enabled="false"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="drpAvailFromMonth" runat="server" Enabled="false"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtAvailFromDay" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox> &nbsp;&nbsp;
                <asp:TextBox ID="txtAvailFromHour" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox>:
                <asp:TextBox ID="txtAvailFromMin" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>                       
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Available to</b>
                <p>If you wish for a video to "expire" and disappear from the site, enter the date here that you want the video to disappear.</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpAvailToYear" runat="server" Enabled="false"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="drpAvailToMonth" runat="server" Enabled="false"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtAvailToDay" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox> &nbsp;&nbsp;
                <asp:TextBox ID="txtAvailToHour" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox>:
                <asp:TextBox ID="txtAvailToMin" runat="server" MaxLength="2" Width="25" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Legacy Video ID</b>
                <p>If this video existed before the streaming site was updated and changed the ID scheme, enter it's original ID here. Otherwise this should be blank.</p>   
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtLegacyID" runat="server" Width="300"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Category</b>                
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpCategory" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br /><br />
    <asp:Table ID="tblControls2" runat="server" Width="100%" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkhidden" runat="server" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Hidden</b>
                <p>Hidden videos can still be accessed directly via their ID, but won't be listed on the website anywhere.</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkPrivate" runat="server" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Hide from outside viewers</b>
                <p>This video is only available to viewers within the school division's network</p>                
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkWasOriginallyLiveStream" runat="server" Checked="true" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Was originally a live broadcast on this site</b>
                <p>Not currently implemented, but might be useful later on.</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkShowAirDate" runat="server" Checked="true" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Display original air date</b>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkAlwaysAvailable" runat="server" Checked="true" OnCheckedChanged="chkAlwaysAvailable_CheckedChanged" AutoPostBack="true" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Video is always available</b>
                <p>Uncheck this if you would like a video to be available for a specific time period only</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkAllowEmbed" runat="server" Checked="true" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Allow this video to be embedded in other sites</b>                
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                <asp:Button ID="btnAdd" runat="server" Text="Add new stream" OnClick="btnAdd_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
