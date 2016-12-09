<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="EditVideo.aspx.cs" Inherits="LSKYStreamingManager.Videos.EditVideo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>Edit video</h2>  
    <a href="index.aspx" style="font-size: 8pt;"><< Back to video list</a><br/><br/>
    <div style="text-align: center;">
        <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
    </div>
    <asp:Table ID="tblControls" runat="server" Width="100%" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell Width="350" VerticalAlign="Top">
                <b>ID</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:Label ID="lblID" runat="server" Text="???"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
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
                www.youtube.com/watch?v=<asp:TextBox ID="txtYoutubeURL" runat="server" Width="400"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>  
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Youtube start time (in seconds)</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtYoutubeStartTime" runat="server" Width="20"></asp:TextBox>
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
                /Video_files/<asp:TextBox ID="txtDownloadURL" runat="server" Width="400"></asp:TextBox>
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
            <asp:TableCell HorizontalAlign="Center">
                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_OnClick">Delete video</asp:LinkButton>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Center">
                <asp:Button ID="btnEdit" runat="server" Text="Update Video" OnClick="btnEdit_OnClick" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
