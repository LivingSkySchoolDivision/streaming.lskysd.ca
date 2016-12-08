<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="LSKYStreamingVideo.player.index" %>
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

            alert(errMsg);

            throw new Error(errMsg);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="server">
    <asp:Table ID="tblContainer" CssClass="player_container" runat="server" Visible="false">
        <asp:TableRow>
            <asp:TableCell><asp:Literal ID="litPlayer" runat="server"></asp:Literal></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Literal ID="litVideoInfo" runat="server"></asp:Literal>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    <asp:Table ID="tblErrorMessage" runat="server" HorizontalAlign="Center" Visible="false">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Literal ID="litErrorMessage" runat="server"></asp:Literal>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>

    
</asp:Content>
