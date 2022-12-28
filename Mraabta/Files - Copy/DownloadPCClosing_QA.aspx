<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadPCClosing_QA.aspx.cs" Inherits="MRaabta.Files.DownloadPCClosing_QA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function CloseThisWindow() {
            debugger;
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <div style="display: none;">
            <asp:GridView ID="gv_pc" runat="server" AutoGenerateColumns="true">
            </asp:GridView>
            <asp:GridView ID="gv_cih" runat="server" AutoGenerateColumns="true">
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
