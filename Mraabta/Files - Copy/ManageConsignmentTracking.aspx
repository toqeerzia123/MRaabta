<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/BtsMasterPage.master" CodeBehind="ManageConsignmentTracking.aspx.cs" Inherits="MRaabta.Files.ManageConsignmentTracking" %>

<%@ Register TagPrefix="Ajax1" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style>
        .input-field
        {
            float: left;
            width: 20%;
        }
        
        .input-field > input
        {
            padding: 4px;
            width: 94%;
            color: #000;
        }
        
        .input-form .field
        {
            float: left;
            font-weight: bold;
            line-height: 16px;
            padding: 0 2px 0 0;
            text-align: left;
            width: 12%;
        }
        
        .boxbg
        {
            background: #eee none repeat scroll 0 0;
            width: 100%;
        }
        
        legend
        {
            font-size: 15px;
            font-weight: bold;
            left: 20px;
            position: relative;
            width: 50%;
        }
        
        .mGrid
        {
            border: 0 !important;
        }
    </style>
    <script type="text/javascript">
        function loader() {
            // document.getElementById('<%=loader.ClientID %>').style.display = "";
        }
    </script>
    <div runat="server" id="loader" style="float: left; opacity: 0.7; position: absolute;
        text-align: center; display: none; top: 50%; width: 84% !important;">
        <div class="loader">
            <img src="../images/Loading_Movie-02.gif" />
        </div>
    </div>
    <div>
        <table cellpadding="0" cellspacing="0" width='100%' class='mGrid_Table'>
            <tr>
                <td colspan="12" align="center" class="head_column">
                    <h3>
                        Manage Consignment Tracking</h3>
                </td>
            </tr>
        </table>
        <table class="input-form boxbg" style="width: 95%">
            <tr>
                <td class="field">Consignment Number:
                </td>
                <td class="input-field">
                    <asp:TextBox ID="txt_consignmentno" runat="server" CssClass="med-field"></asp:TextBox>
                </td>
                <td style="float: left; margin: 0px 0px 0px 20px;"></td>
                <td>
                    <asp:Button ID="submit" runat="server" Text="Search" CssClass="button1" OnClick="Btn_Load_Click"
                        OnClientClick="loader()" />
                </td>
            </tr>
        </table>
        <div id="detail_div" runat="server">
            <legend>Consignment Tracking Header</legend>
            <asp:Label ID="lbl_datetime" runat="server" Style="font-weight: bold; text-align: center; width: 100%; display: block;"></asp:Label>
            <table class="input-form" style="width: 95%; margin-top: 5px;">
                <tr>
                    <td class="field">CN:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_con_num" runat="server"></asp:Label>
                    </td>
                    <td class="field">Booking Date:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="bdate" runat="server"></asp:Label>
                    </td>
                    <td class="field">Receiving Date:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_ReceivingDate" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Origin:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_orign" runat="server"></asp:Label>
                    </td>
                    <td class="field">Destination:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_destination" runat="server"></asp:Label>
                    </td>
                    <td class="field">Int. Destination:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_IntCountryCode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Shipper:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_shipper" runat="server"></asp:Label>
                    </td>
                    <td class="field">Consignee:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_consignee" runat="server"></asp:Label>
                    </td>
                    <td class="field">Consignee Address:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_address" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Accout No:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_account" runat="server"></asp:Label>
                    </td>
                    <td class="field">Weight:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_weight" runat="server"></asp:Label>
                    </td>
                    <td class="field">Pieces:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_piece" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Delivery Date & Time:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_delivery_time" runat="server"></asp:Label>
                    </td>
                    <td class="field">Delivery Rider:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_delivery_rider" runat="server"></asp:Label>
                    </td>
                    <td class="field">Current Status:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_status" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Relation:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_relation" runat="server"></asp:Label>
                    </td>
                    <td class="field">Received By:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_received" runat="server"></asp:Label>
                    </td>
                    <td class="field">Receiver CNIC:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_nic" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Special Instruction:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_special" runat="server"></asp:Label>
                    </td>
                    <td class="field">VOID CN:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_void" runat="server"></asp:Label>
                    </td>
                    <td class="field">Service Type:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_serive_type" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">Comments:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_comment" runat="server"></asp:Label>
                    </td>
                    <td class="field">Dimension:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_dimension" runat="server"></asp:Label>
                    </td>
                    <td class="field">DeBriefing:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="link" runat="server"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="field">Consignee Phone No:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_consigneephone" runat="server"></asp:Label>
                    </td>
                    
                    <td class="field">RR Number:
                    </td>
                    <td class="input-field">
                        <asp:Label ID="lbl_rrno" runat="server"></asp:Label>
                    </td>

                    <td class="field"><b>RR User:</b></td>
                    <td class="input-field">
                        <asp:Label ID="lbl_rruser" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="field"><b>RR Date:</b></td>
                    <td class="input-field">
                        <asp:Label ID="lbl_rrdate" runat="server"></asp:Label></td>
                    <td class="field"><b>COD Amount:</b></td>
                    <td class="input-field">
                       <asp:Label ID="lbl_codamount" runat="server"></asp:Label></td>
                </tr>
            </table>
            <legend>Consignment Tracking Details</legend><span id="Table_1" class="tbl-large">
                <asp:GridView ID="gridview" runat="server" AutoGenerateColumns="false" CssClass="mGrid floatgrid"
                    AlternatingRowStyle-CssClass="alt">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No." ItemStyle-Width="2%">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Transaction Time" DataField="transactionTime" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="5%"></asp:BoundField>
                        <asp:BoundField HeaderText="Event" DataField="TrackingStatus" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="10%"></asp:BoundField>
                        <asp:BoundField HeaderText="Location" DataField="currentLocation" ItemStyle-HorizontalAlign="Center"
                            ItemStyle-Width="5%"></asp:BoundField>
                        <asp:BoundField HeaderText="Message" DataField="Detail" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="50%"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </span>
        </div>
    </div>
</asp:Content>
