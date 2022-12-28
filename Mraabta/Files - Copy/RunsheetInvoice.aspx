<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RunsheetInvoice.aspx.cs" Inherits="MRaabta.Files.RunsheetInvoice" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    

   <%-- <asp:Panel ID="tbl_invoice" runat="server">--%>


<%--
    <div style="vertical-align: top; position: relative; width: 100%; font-size: 12px; font-family: verdana; left: 0%;">

        <div class="left" style="float: left; height: 985px; position: relative; top: 14px; width: 50%; overflow:hidden">
            
            <div class="top_left" style="position: relative; left: 10%">
                <div class="dd_date" style="position: relative; left: 20%;">
                    <asp:Label ID="date" runat="server"></asp:Label>
                </div>
                <div class="sheet_no" style="position: relative; left: 20%;">
                    <asp:Label ID="runsheet" runat="server"></asp:Label>
                </div>
                <div class="branch" style="position: relative; left: 65%;">
                    <asp:Label ID="orign" runat="server"></asp:Label>
                </div>
            </div>


            <div class="body_left" style="position: relative; top: 6%; left: 5%;">

                <div class="box_1" style="height:80px">
                    <div class="sr" style="float: left; ">
                        <asp:Label ID="sn" runat="server"></asp:Label>
                    </div>
                    <div class="cn_detail" style="float: left; position: relative; left: 3%; width: 50%;">
                        <asp:Label ID="cn" runat="server"></asp:Label>
                    </div>
                    <div class="re_name" style="float: left; position: relative; left: 5%; width: 50%;">
                        <asp:Label ID="consigneer" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; clear: both; position: relative; left: 5%; top: 25px;">
                        <asp:Label ID="destination" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; position: relative; left: 17%;top: 25px;">
                    <asp:Label ID="pieces" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; position: relative; left: 36%;top: 25px;">
                    <asp:Label ID="weight" runat="server"></asp:Label>
                    </div>
                </div>

            </div>







        </div>  


        <div class="right" style="float: left; height: 985px; position: relative; top: 14px; width: 45%; overflow:hidden">
            
            <div style="float: left; width: 47%; position: relative; left: 10%">
                <div class="route_name" style="position: relative; left: 20%;">ALLAMA IQBAL TOWN</div>
                
                <div class="courier_name" style="position: relative; left: 19%;">SNS Walton Seven Street</div>      
                
            </div>
           
          <div style="float: left; width: 47%; position: relative; left: 30%;">
                  <div class="code" style="position: relative; left: 20%;">1510</div>
                  
            <div class="code" style="position: relative; left: 20%;">1510</div>
           
           </div>
        </div>  

    </div>
--%>

<%--

<div class="box_1" style="height:80px">
                    <div class="sr" style="float: left; ">1.</div>
                    <div class="cn_detail" style="float: left; position: relative; left: 3%; width: 50%;">
                        <asp:Label ID="cn" runat="server"></asp:Label>
                    </div>
                    <div class="re_name" style="float: left; position: relative; left: 5%; width: 50%;">
                        <asp:Label ID="consigneer" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; clear: both; position: relative; left: 5%; top: 25px;">
                        <asp:Label ID="destination" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; position: relative; left: 17%;top: 25px;">
                    <asp:Label ID="pieces" runat="server"></asp:Label>
                    </div>
                    <div class="sign" style="float: left; position: relative; left: 36%;top: 25px;">
                    <asp:Label ID="weight" runat="server"></asp:Label>
                    </div>
                </div>
--%>


<asp:PlaceHolder ID="ph1" runat="server"></asp:PlaceHolder>
<%--<img src="../images/mnpLogo.png" />--%>
    <%--</asp:Panel>--%>
</body>
</html>
