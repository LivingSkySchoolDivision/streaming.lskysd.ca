<%@ Page Title="" Language="C#" MasterPageFile="~/Template.Master" AutoEventWireup="true" CodeBehind="NewStream.aspx.cs" Inherits="LSKYStreamingManager.Streams.NewStream" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        p {
            font-size: 8pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <h2>Add new stream</h2>    
    <div style="text-align: center;">
        <asp:Label ID="lblError" runat="server" Text="" Font-Bold="true" Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
    </div>
    <asp:Table ID="tblControls" runat="server" Width="100%" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell Width="350" VerticalAlign="Top">
                <b>Title</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtTitle" runat="server" Width="300"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Stream location</b>
                <p>What physical location is the stream happening at? You can be as specific or generic as you'd like, or if you leave this blank it won't be displayed.</p>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtStreamLocation" runat="server" Width="300"></asp:TextBox>
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
                <asp:Image ID="imgThumbnail" ImageUrl="/thumbnails/small/blank.png" CssClass="thumbnail_editor" runat="server" /><br />
                <asp:DropDownList ID="drpThumbnail" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpThumbnail_SelectedIndexChanged"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>ISML URL</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpISML" runat="server"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Expected start (24-hour time)</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpStartYear" runat="server"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="drpStartMonth" runat="server"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtStartDay" runat="server" MaxLength="2" Width="25"></asp:TextBox> &nbsp;&nbsp;
                <asp:TextBox ID="txtStartHour" runat="server" MaxLength="2" Width="25"></asp:TextBox>:
                <asp:TextBox ID="txtStartMinute" runat="server" MaxLength="2" Width="25"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Expected finish (24-hour time)</b>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:DropDownList ID="drpEndYear" runat="server"></asp:DropDownList>&nbsp;
                <asp:DropDownList ID="drpEndMonth" runat="server"></asp:DropDownList>&nbsp;
                <asp:TextBox ID="txtEndDay" runat="server" MaxLength="2" Width="25"></asp:TextBox> &nbsp;&nbsp;
                <asp:TextBox ID="txtEndHour" runat="server" MaxLength="2" Width="25"></asp:TextBox>:
                <asp:TextBox ID="txtEndMinute" runat="server" MaxLength="2" Width="25"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <b>Sidebar content</b>  
                <p>Note: Not currently implemented</p>              
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <asp:TextBox ID="txtSidebar" runat="server" TextMode="MultiLine" Width="100%" Height="100" Enabled="false"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
        <br /><br />
    <asp:Table ID="tblControls2" runat="server" Width="100%" Font-Size="Small">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkForce" runat="server" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Force online</b>
                <p>
                    This option causes the site to ignore the start and end times, and assume that this stream is currently broadcasting.
                </p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkHidden" runat="server" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Hide from front page</b>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkPrivate" runat="server" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Hide from outside viewers</b>
                <p>Only advertise this broadcast to viewers within the school division's network.</p>
                <p>Note: Not currently implemented</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Right">
                <asp:CheckBox ID="chkSidebar" runat="server" Enabled="false" />
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top">
                <b>Display sidebar</b>
                <p>Note: Sidebars are not currently implemented.</p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                <asp:Button ID="btnAdd" runat="server" Text="Add new stream" OnClick="btnAdd_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>
