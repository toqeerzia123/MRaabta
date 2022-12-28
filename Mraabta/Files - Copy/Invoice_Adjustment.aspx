<%@ Page Title="" Language="C#" MasterPageFile="~/BtsMasterPage.Master" AutoEventWireup="true" CodeBehind="Invoice_Adjustment.aspx.cs" Inherits="MRaabta.Files.Invoice_Adjustment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function loader() {
            document.getElementById('<%=div2.ClientID %>').style.display = "";
        }
    </script>
    <style>
        .input-form tr {
            float: none;
            margin: 0 0 10px;
            width: 100%;
        }

        .outer_box {
            background: #444 none repeat scroll 0 0;
            height: 101%;
            left: 0;
            opacity: 0.9;
            position: absolute;
            top: -1%;
            width: 100%;
        }


        .pop_div {
            background: #eee none repeat scroll 0 0;
            border-radius: 10px;
            height: 100px;
            left: 48%;
            position: relative;
            top: 40%;
            width: 235px;
        }

        .btn_ok {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: -18px;
            padding: 1px 14px;
            position: relative;
            top: 67%;
        }

        .btn_cancel {
            background: #000 none repeat scroll 0 0;
            border: 0 none;
            color: #fff;
            left: 22%;
            padding: 1px 14px;
            position: relative;
            top: 42%;
        }

        .pop_div > span {
            float: left;
            line-height: 40px;
            text-align: center;
            width: 100%;
        }

        .tbl-large div {
            position: static;
        }

        .outer_box img {
            left: 42%;
            position: relative;
            top: 40%;
        }

        .style1 {
            border-radius: 5px !important;
            color: White;
            font-family: Calibri;
            font-size: small;
            cursor: pointer;
            border-style: none;
            border-color: inherit;
            border-width: 0;
            padding: 3px 20px;
            background-color: #5f5a8d;
        }
    </style>
    <div id="div2" runat="server" class="outer_box" style="display: none; background: #FFFFFF none repeat scroll 0 0;">
        <img src="../images/Loading_Movie-02.gif" />
    </div>
    <div style="width: 100% !important; text-align: center;">
        <asp:Label ID="Errorid" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Large"></asp:Label>
    </div>
    <table style="font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 97%"
        class="input-form">
        <tr style="float: none !important;">
            <td style="float: none !important; font-variant: small-caps !important; width: 200px; font-size: large; text-align: center;"
                colspan="8">
                <b>Invoice Adjustment Screen</b>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Invoice#
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_invoiceNo" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td colspan="1" class="input-field" style="width: 15% !important;">
                <asp:Button ID="btn_getConsignment" runat="server" CssClass="style1" Width="100%"
                    Text="Get Details" OnClick="btn_getConsignment_Click" OnClientClick="loader()"
                    UseSubmitBehavior="false" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Reason
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:DropDownList ID="dd_Reason" runat="server" CssClass="dropdown" AppendDataBoundItems="true">
                    <asp:ListItem Value="0"> Select Resaon</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Consingment #
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td colspan="1" class="input-field" style="width: 15% !important;">
                <asp:Button ID="Button1" runat="server" CssClass="style1" Width="100%" Text="Add"
                    OnClick="btn_getConsignment_Click_" OnClientClick="loader()" UseSubmitBehavior="false" />
            </td>
        </tr>
        <tr>
            <td class="field" style="width: 10% !important;">Account No #
            </td>
            <td class="input-field" style="width: 15% !important;">
                <asp:TextBox ID="txt_AccountNo" runat="server" OnTextChanged="txt_AccountNo_TextChanged"
                    AutoPostBack="true"></asp:TextBox>
            </td>
            <td class="space" style="width: 8% !important; margin: 0px 0px 0px 0px !important;"></td>
            <td colspan="1" class="input-field" style="width: 15% !important;"></td>
        </tr>
        <tr>
            <td class="space" colspan="2" style="width: 8% !important; margin: 0px 0px 0px 0px !important;">
                <asp:RadioButtonList ID="rb_1" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                    Width="200px" OnSelectedIndexChanged="rb_1_SelectedIndexChanged">
                    <asp:ListItem Value="D" Selected="True"> Debit </asp:ListItem>
                    <asp:ListItem Value="C"> Credit </asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
    <div>
        <table id="Head_1" style="border-style: solid; border-width: thin; font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 100%"
            class="input-form">
            <tr>
                <td class="field">Invoice No :
                </td>
                <td class="field">
                    <asp:Label ID="lbl_InvoiceNo" runat="server"></asp:Label>
                </td>
                <td class="field">Client A/C #
                </td>
                <td class="field">
                    <asp:Label ID="txt_AC_Info" runat="server"></asp:Label>
                </td>
                <td class="field" style="width: 10% !important;">Invoice Date
                </td>
                <td class="field">
                    <asp:Label ID="txt_InvoiceDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="field">Client Name
                </td>
                <td class="field" colspan="3" nowrap>
                    <asp:Label ID="txt_ClientName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="field">Start Date :
                </td>
                <td class="field">
                    <asp:Label ID="txt_StartDate" runat="server"></asp:Label>
                </td>
                <td class="field">End Date :
                </td>
                <td class="field">
                    <asp:Label ID="txt_EndDate" runat="server"></asp:Label>
                </td>
                <td class="field" colspan="4" nowrap>Domestic Discount :
                    <asp:Label ID="lbl_DiscountOnDomestic" runat="server"></asp:Label>
                    || Document Discount :
                    <asp:Label ID="lbl_DomesticDocument" runat="server"></asp:Label>
                    || Sample Discount :
                    <asp:Label ID="lbl_SampleDocument" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="field">Calculation Base
                </td>
                <td class="field">
                    <asp:Label ID="lbl_Base" runat="server"></asp:Label>
                </td>
                <td class="field">Modified Value
                </td>
                <td class="field">
                    <asp:Label ID="lbl_ModifiedValue" runat="server"></asp:Label>
                </td>
                <td class="field">Consignments :
                    <asp:Label ID="lbl_Consignment" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; height: 250px; overflow: scroll; text-align: center;">
        <span id="Span1" class="tbl-large">
            <asp:Label ID="Label1" runat="server" CssClass="error_msg"></asp:Label>
            <asp:GridView ID="gv_cns" runat="server" AutoGenerateColumns="false" CssClass="mGrid"
                AlternatingRowStyle-CssClass="alt" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"
                Width="97%" EmptyDataText="No Data Available" OnRowDataBound="gv_cns_RowDataBound"
                OnRowCommand="gv_cns_RowCommand" OnSelectedIndexChanged="gv_cns_SelectedIndexChanged">
                <RowStyle Font-Bold="false" />
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="bookingDate" />
                    <asp:BoundField HeaderText="Invoice No" DataField="invoiceNumber" />
                    <asp:BoundField HeaderText="Consignment #" DataField="ConsignmentNumber" />
                    <asp:TemplateField HeaderText="Accountno">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_Accountno" runat="server" Enabled="false" OnTextChanged="txt_cnNumber_TextChanged"
                                Width="50%" Text='<%# DataBinder.Eval(Container.DataItem,"Account_no") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Origin Brch" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_origin" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dest Brch">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_Destination" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Service Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_ServiceType" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Weight">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_Weight" runat="server" Width="20px" Text='<%# DataBinder.Eval(Container.DataItem,"Weight") %>'></asp:TextBox>
                            <asp:HiddenField ID="hd_Wieght" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Weight") %>' />
                            <asp:HiddenField ID="hd_Amount" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"totalAmount") %>' />
                            <asp:HiddenField ID="hd_gst" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Gst") %>' />
                            <asp:HiddenField ID="hd_Consignment" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ConsignmentNumber") %>' />
                            <asp:HiddenField ID="hd_origin" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Origin") %>' />
                            <asp:HiddenField ID="hd_destination" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Destination") %>' />
                            <asp:HiddenField ID="hd_servicetypeName" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"Service_Type") %>' />
                            <asp:HiddenField ID="hd_ConsignmentTypeID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"ConsignmentType_id") %>' />
                            <asp:HiddenField ID="hd_type" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"IsIntl") %>' />
                            <asp:HiddenField ID="hd_PriceModifier" runat="server" Value='<%# DataBinder.Eval(Container.DataItem,"IsIntl") %>' />

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CN Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="dd_ConsignmentType" runat="server" CssClass="dropdown">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Amount" DataField="totalAmount" />
                    <asp:BoundField HeaderText="Gst" DataField="Gst" />
                    <asp:TemplateField HeaderText="New Amount">
                        <ItemTemplate>
                            <asp:Label ID="txt_Amount" runat="server" Width="20px" Text="0"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="New Gst">
                        <ItemTemplate>
                            <asp:Label ID="txt_Gst" runat="server" Width="20px" Text="0"></asp:Label>
                            <asp:HiddenField ID="lbl_NoteType" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P Modifier">
                        <ItemTemplate>
                            <asp:DropDownList ID="Dd_priceModifier" runat="server" CssClass="dropdown" AutoPostBack="true"
                                OnSelectedIndexChanged="Dd_priceModifier_SelectedIndexChanged" AppendDataBoundItems="true">
                                <asp:ListItem Value="0"> Select Price Modifier</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CM Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_CM_Amount" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container.DataItem,"CM_value") %>'
                                AutoPostBack="true" OnTextChanged="txt_CM_Amount_TextChanged"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="CM Gst">
                        <ItemTemplate>
                            <asp:TextBox ID="txt_CM_Gst" runat="server" Width="40px" Text='<%# DataBinder.Eval(Container.DataItem,"CM_gst") %>'
                                Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_Recompute" runat="server" Text="ReCompute" CommandName="Re_compute"
                                UseSubmitBehavior="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btn_Remove" runat="server" Text="Remove" CommandName="Remove" UseSubmitBehavior="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </span>
    </div>
    <div style="width: 100%; overflow: scroll; text-align: center;">
        <table id="Table1" style="border-style: solid; border-width: thin; font-family: Calibri; font-size: medium; padding-bottom: 0px !important; padding-top: 0px !important; width: 100%"
            class="input-form">
            <tr>
                <td class="field" align="left">DCN Amount :
                </td>
                <td class="field">
                    <asp:Label ID="lbl_InvoiceAmount" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">DCN Gst
                </td>
                <td class="field">
                    <asp:Label ID="lbl_gst" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">DiscountOnDomentic
                </td>
                <td class="field">
                    <asp:Label ID="lbl_domesticDiscount2" runat="server"></asp:Label>
                </td>
                <td class="field"></td>
                <td class="field"></td>
            </tr>
            <tr>
                <td class="field" align="left">ICN Amount
                </td>
                <td class="field" colspan="1" nowrap>
                    <asp:Label ID="lbl_IcnAmount" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">ICN Gst
                </td>
                <td class="field" colspan="1" nowrap>
                    <asp:Label ID="lbl_IcnGst" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">Discount Document
                </td>
                <td class="field">
                    <asp:Label ID="lbl_discountdocument" runat="server"></asp:Label>
                </td>
                <td class="field"></td>
                <td class="field"></td>
            </tr>
            <tr>
                <td class="field" align="left">Fuel Surcharge
                </td>
                <td class="field" colspan="1" nowrap>
                    <asp:Label ID="lbl_FuelSurcharge" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">Fuel Surcharge Gst
                </td>
                <td class="field" colspan="1" nowrap>
                    <asp:Label ID="lbl_FuelSurchargeGst" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">Sample Discount
                </td>
                <td class="field">
                    <asp:Label ID="lbl_SampleDiscount2" runat="server"></asp:Label>
                </td>
                <td class="field"></td>
                <td class="field"></td>
            </tr>
            <tr>
                <td class="field" align="left">Total Amount
                </td>
                <td class="field">
                    <asp:Label ID="lbl_TotalAmount" runat="server"></asp:Label>
                </td>
                <td class="field" align="left">Total Gst
                </td>
                <td class="field">
                    <asp:Label ID="lbl_TotalGst" runat="server"></asp:Label>
                </td>
                <td class="field">
                    <asp:Label ID="Label10" runat="server"></asp:Label>
                </td>
                <td class="field"></td>
                <td class="field"></td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; text-align: center;">
        <asp:Button ID="btn_save" runat="server" Text="Save" CssClass="button" OnClick="btn_save_Click"
            UseSubmitBehavior="false" />
    </div>
</asp:Content>
