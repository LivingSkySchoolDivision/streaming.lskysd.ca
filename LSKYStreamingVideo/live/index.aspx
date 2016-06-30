<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.live.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    #silverlightControlHost {
      height: 100%;
      text-align:center;
    }
    </style>
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            }

            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;

            if (errorType == "ImageError" || errorType == "MediaError") {
                return;
            }

            var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

            errMsg += "Code: " + iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError") {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError") {
                if (args.lineNumber != 0) {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " + args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            //alert(errMsg);

            throw new Error(errMsg);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">    
    <asp:Table ID="tblContainer" CssClass="player_container" runat="server" Visible="false" Width="1300">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><asp:Literal ID="litPlayer" runat="server"></asp:Literal></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Literal ID="litStreamInfo" runat="server"></asp:Literal>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <p class="large_infobox" style="margin-left: auto; margin-right: auto; width: 720px; font-size: 8pt; font-weight: normal; padding: 5px; color: #444444;">
                    <b>Miss part of the stream? Experiencing technical difficulties? </b><br />
                    In most cases, we make the recordings of our live broadcasts available in the "videos" section of this website within a few days, and they are often made available for download at this time as well.
                </p>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <div class="large_infobox" style="margin-left: auto; margin-right: auto; width: 720px; font-size: 8pt; font-weight: normal; padding: 5px; color: #444444;">
                    <b>A note regarding browser compatibility</b><br />  
                    Live streaming currently only works on the following platforms:
                    <ul>
                        <li>Windows computers with Microsoft Silverlight 4 or above (use Silverlight player)</li>
                        <li>Windows computers using the Google Chrome browser (use HTML5 player)</li>
                        <li>Apple computers using the Google Chrome browser (use HTML5 player)</li>
                        <li>Apple computers with Safari (use HTML5 player)</li>
                        <li>iOS devices (iPad/iPod/iPhone) with Safari (use HTML5 player)</li>
                        <li>Android browser on Android devices(use HTML5 player)</li>
                    </ul>
                    Other non-live videos on this website contain browser compatibility in their descriptions, and in most cases will work on any browser and platform. If you are unable to view a live stream due to browser incompatibility issues, check back in a few days to view the recorded video in the "Videos" section of this website, or download a copy of the video to view offline if available.                                                                    
                </div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    <asp:Table ID="tblNotFound" runat="server" HorizontalAlign="Center" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <div class="large_infobox">Stream not found</div>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
       

</asp:Content>
