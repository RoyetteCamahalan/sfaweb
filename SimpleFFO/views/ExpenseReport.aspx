<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="ExpenseReport.aspx.cs" Inherits="SimpleFFO.views.ExpenseReport" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<%@ Register TagName="FundingComponent" Src="components/FundingComponent.ascx" TagPrefix="ucfunding"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
    <webopt:BundleReference runat="server" Path="~/Content/dropzone" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Expense Report</h1>
                    </div>
                </div>
            </div>
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Details</h3>
                    </div>
                    <div class="card-body">

                        <%--DETAILS START--%>
                        <div class="row">
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label>Employee</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_fullname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6 col-xs-12">
                                <div class="form-group">
                                    <label>Role</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_role" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Sunday Date</label>
                                    <asp:TextBox runat="server" ID="txtbox_startdate" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Thru Date</label>
                                    <asp:TextBox runat="server" ID="txtbox_thrudate" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Plate No</label>
                                    <asp:TextBox runat="server" ID="txtbox_plateno" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-12">
                                <div class="form-group">
                                    <label>Last Odometer</label>
                                    <asp:TextBox runat="server" ID="txtbox_lastodometer" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <%--DETAILS END--%>
                    </div>
                </div>



                <%--DAILY ENTRIES START--%>
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Daily Entries</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="panelmaingrid" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="dgvDailyExpense"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline dgvDailyExpense"
                                    HeaderStyle-CssClass="gvHeader"
                                    AlternatingRowStyle-CssClass="gvAltRow"
                                    CellPadding="0"
                                    ShowFooter="true"
                                    ShowHeader="true"
                                    OnRowDataBound="dgvDailyExpense_RowDataBound">
                                    <RowStyle Height="80px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <tr class="gvHeader">
                                                    <th></th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Date</th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Point to Point Destination / Location</th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Work With</th>
                                                    <th colspan="6" class="text-center">Transportation</th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Meals</th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Miscellaneous</th>
                                                    <th rowspan="2" style="vertical-align: middle; text-align: center;">Daily Total</th>
                                                </tr>
                                                <tr class="gvHeader">
                                                    <th></th>
                                                    <th class="text-center">Medium</th>
                                                    <th class="text-center">Image</th>
                                                    <th class="text-center">Odometer</th>
                                                    <th class="text-center">Personal</th>
                                                    <th class="text-center">Business</th>
                                                    <th class="text-center">Amount</th>
                                                </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <td style="width: 10%" class="td-center">
                                                    <asp:TextBox runat="server" ReadOnly="true" CssClass="form-control rounded-0" Font-Size="Small" Text='<%# Convert.ToDateTime(Eval("date")).ToString("ddd") + "-" +Convert.ToDateTime(Eval("date")).ToString(AppModels.dateformat) %>'></asp:TextBox>
                                                </td>
                                                <td style="width: 15%">
                                                    <div class="cellfix-80">
                                                        <asp:TextBox ID="txtroute" runat="server" CssClass="form-control rounded-0 cell-fill" Font-Size="Small" Text='<%# Eval("route") %>'>
                                                        </asp:TextBox>
                                                    </div>
                                                </td>
                                                <td style="width: 10%">
                                                    <div class="cellfix-80">
                                                        <asp:TextBox ID="txtbox_workwith" runat="server" CssClass="form-control rounded-0 cell-fill" Font-Size="Small" Text='<%# Eval("workwith") %>'>
                                                        </asp:TextBox>
                                                    </div>
                                                </td>
                                                <td style="width: 12%" class="td-center">
                                                    <asp:DropDownList ID="cmbmediumtranspo" Font-Size="Small" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                        <asp:ListItem Value="0" Selected="True">--Select--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="td-center">
                                                    <div style="text-align: center">
                                                        <asp:Literal ID="literalimagefilename" runat="server" Text='<%# Eval("imgloc") %>' Visible="false"></asp:Literal>
                                                        <asp:Button ID="btnsaveimagemisc" runat="server" CssClass='<%# "btnsaveimage_" + Container.DataItemIndex %>' CausesValidation="false" UseSubmitBehavior="false" OnClick="btnsaveimages_Click" Style="display: none;" CommandArgument='<%# Container.DataItemIndex %>' />
                                                        <asp:Image ID="imgpreview" runat="server" Height="50" Width="50" ImageUrl="#" Style="border-width: 0px;" Visible="false" CssClass="imgpreview" data-toggle="modal" data-target=".imagepreviewmodal"/>
                                                        <asp:FileUpload ID="fuodometer" Style="display: none;" onchange=<%# "showpreview('.btnsaveimage_" + Container.DataItemIndex + "');" %> CssClass='<%# "fileupload_" + Container.DataItemIndex %>' runat="server" accept="image/png, image/jpeg"/>
                                                        <asp:LinkButton ID="btnupload" runat="server" CommandArgument='<%# ".fileupload_" + Container.DataItemIndex %>' CausesValidation="false" OnClick="btnupload_Click">
                                                        <i class="fa fa-upload"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_odometer" runat="server" Font-Size="Small" Text='<%# Eval("totalkm") %>' CssClass="form-control rounded-0 text-center"
                                                        AutoPostBack="true" OnTextChanged="textboxtextChange">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_personal" runat="server" Font-Size="Small" Text='<%# Eval("personal") %>' CssClass="form-control rounded-0 text-center" AutoPostBack="true" OnTextChanged="textboxtextChange">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_business" runat="server" ReadOnly="true" Font-Size="Small" CssClass="form-control rounded-0 text-center" Text='<%# Eval("business") %>'>
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_amount" runat="server" Font-Size="Small" Text='<%# Convert.ToDecimal(Eval("amount") ?? 0).ToString(AppModels.moneyformat) %>' CssClass="form-control rounded-0 text-center" AutoPostBack="true" OnTextChanged="textboxtextChange">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_meal" runat="server" Font-Size="Small" Text='<%# Convert.ToDecimal(Eval("meals") ?? 0).ToString(AppModels.moneyformat) %>' CssClass="form-control rounded-0 text-center" AutoPostBack="true" OnTextChanged="textboxtextChange">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_miscellaneous" runat="server" Font-Size="Small" Text='<%# Convert.ToDecimal(Eval("miscellaneous") ?? 0).ToString(AppModels.moneyformat) %>' CssClass="form-control rounded-0 text-center" AutoPostBack="true" OnTextChanged="textboxtextChange">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_total" runat="server" ReadOnly="true" Font-Size="Small" CssClass="form-control rounded-0 text-center" Text='<%# Convert.ToDecimal(Eval("totaldaily") ?? 0).ToString(AppModels.moneyformat) %>'>
                                                    </asp:TextBox>
                                                </td>
                                            </ItemTemplate>
                                            <FooterTemplate>

                                                <td colspan="6">
                                                    <label class="col-form-label float-right">Weekly Totals</label></td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_totalPersonal" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_totalbusiness" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_totalamount" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_totalmeals" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_miscellanous" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                                <td class="td-center">
                                                    <asp:TextBox ID="txtbox_dailytotal" runat="server" Font-Size="Small" ReadOnly="true"
                                                        CssClass="form-control rounded-0 text-center">
                                                    </asp:TextBox>
                                                </td>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--DAILY ENTRIES END--%>
                    </div>
                </div>



                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Expenses Breakdown</h3>
                    </div>
                    <div class="card-body">

                        <asp:UpdatePanel ID="UpdatepnlExpenseBreak" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="form-group float-right">
                                    <asp:Button ID="btnAddExpenseBreak" runat="server" Text="Add New" CssClass="btn btn-primary" CausesValidation="false" UseSubmitBehavior="false" TabIndex="-1"
                                        OnClick="AddExpenseClicked" />
                                </div>

                                <br />

                                <asp:GridView runat="server" ID="GvexpenseBreak"
                                    AutoGenerateColumns="false"
                                    ShowFooter="true"
                                    ShowHeaderWhenEmpty="true"
                                    EmptyDataText="No record added"
                                    GridLines="None"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline dgvDailyExpense"
                                    HeaderStyle-CssClass="gvHeader"
                                    AlternatingRowStyle-CssClass="gvAltRow"
                                    OnRowCommand="GvexpenseBreak_RowCommand"
                                    OnRowDataBound="GvexpenseBreak_DataBound">
                                    <RowStyle Height="80px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <tr class="gvHeader">
                                                    <th></th>
                                                    <th style="vertical-align: middle; text-align: center;">Date</th>
                                                    <th style="vertical-align: middle; text-align: center;">Type</th>
                                                    <th style="vertical-align: middle; text-align: center;">Vendor</th>
                                                    <th style="vertical-align: middle; text-align: center;">Details</th>
                                                    <th style="vertical-align: middle; text-align: center;">Vat</th>
                                                    <th style="vertical-align: middle; text-align: center;">Receipt Image</th>
                                                    <th style="vertical-align: middle; text-align: center;">Ref No.</th>
                                                    <th style="vertical-align: middle; text-align: center;">Amount</th>
                                                    <% if (!this.isPageView)
                                                        { %>
                                                    <th style="vertical-align: middle; text-align: center;"></th>
                                                    <% } %>
                                                </tr>
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <td style="width: 15%" class="td-center">
                                                    <div class="input-group date" id="reservationdatetime" data-target-input="nearest">
                                                        <asp:TextBox ID="dtpDate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpDate" aria-describedby="inputSuccess2Status3"
                                                            Text='<%# Eval("expensedate")==null ? "" : Convert.ToDateTime(Eval("expensedate")).ToString(AppModels.dateformat) %>' Font-Size="Small"></asp:TextBox>
                                                        <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                            <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                        </div>
                                                        <span id="inputSuccess2Status3" class="sr-only">(success)</span>
                                                    </div>
                                                </td>
                                                <td style="width: 15%" class="td-center">
                                                    <asp:DropDownList runat="server" Font-Size="Small" ID="combomisc" CssClass="custom-select rounded-0"></asp:DropDownList>
                                                </td>

                                                <td style="width: 15%" class="td-center">
                                                    <asp:DropDownList runat="server" ID="combovendor" CssClass="form-control rounded-0 cell fill" Font-Size="Small"></asp:DropDownList>
                                                </td>

                                                <td style="width: 10%" class="td-center">
                                                    <div class="cellfix-80">
                                                        <asp:TextBox ID="txtbox_particulars" runat="server" CssClass="form-control rounded-0 cell-fill" Font-Size="Small" Text='<%# Eval("particulars") %>'>
                                                        </asp:TextBox>
                                                    </div>
                                                </td>
                                                <td style="width: 10%" class="td-center">
                                                       <asp:DropDownList runat="server" ID="combovat" CssClass="form-control rounded-0 cell fill" Font-Size="Small">
                                                           <asp:ListItem Selected="False" Value="0" Text="Non-VAT"></asp:ListItem>
                                                           <asp:ListItem Selected="True" Value="1" Text="VAT"></asp:ListItem>
                                                        </asp:DropDownList>
                                                </td>

                                                <td style="width: 10%" class="td-center"><%-- Reciept Image--%>
                                                    <div style="text-align: center">
                                                        <div class="row" style="text-align: center">
                                                            <asp:Literal ID="literalimagefilename" runat="server" Text='<%# Eval("imgloc") %>' Visible="false"></asp:Literal>
                                                            <asp:Button ID="btnsaveimagemisc" runat="server" CssClass='<%# "btnsaveimagemisc_" + Container.DataItemIndex %>' CausesValidation="false" UseSubmitBehavior="false" OnClick="btnsaveimagemisc_Click" Style="display: none;" CommandArgument='<%# Container.DataItemIndex %>' />
                                                            <asp:Image ID="imgpreview" runat="server" Height="50" Width="50" ImageUrl="#" Style="border-width: 0px; margin: auto;" Visible="false" CssClass="imgpreview" data-toggle="modal" data-target=".imagepreviewmodal"/>
                                                        </div>
                                                        <asp:FileUpload ID="fuodometer" Style="display: none;" onchange=<%# "showpreview('.btnsaveimagemisc_" + Container.DataItemIndex + "');" %> CssClass='<%# "fileuploadmisc_" + Container.DataItemIndex %>' runat="server" accept="image/png, image/jpeg" />
                                                        <asp:LinkButton ID="btnupload" runat="server" CommandArgument='<%# ".fileuploadmisc_" + Container.DataItemIndex %>' CssClass="btn-upload" CausesValidation="false" OnClick="btnupload_Click">
                                                        <i class="fa fa-upload"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </td>

                                                <td style="width: 10%" class="td-center">
                                                    <asp:TextBox ID="txtbox_referenceno" runat="server" Font-Size="Small" Text='<%# Eval("referenceno") %>' CssClass="form-control rounded-0 cell-fill">
                                                         
                                                    </asp:TextBox>
                                                </td>

                                                <td style="width: 15%" class="td-center">
                                                    <asp:TextBox ID="txtbox_amountEB" runat="server" Font-Size="Small"
                                                        CssClass="form-control rounded-0 text-center"
                                                        AutoPostBack="true" OnTextChanged="textboxtextChangeExpenseBreak" Text='<%# Convert.ToDecimal(Eval("amount")).ToString(AppModels.moneyformat) %>'>
                                                    </asp:TextBox>
                                                </td>
                                                <% if (!this.isPageView)
                                                    { %>
                                                <td style="width: 5%" class="td-center">
                                                    <div align="center">
                                                        <asp:LinkButton ID="btnRemoveEB" CssClass="btn btn-danger btn-sm btn-flat" runat="server" CausesValidation="false" CommandName="removeitem" CommandArgument='<%# Container.DataItemIndex %>'><i class="far fa-trash-alt"></i></asp:LinkButton>
                                                    </div>
                                                </td>
                                                <% } %>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <td colspan="6"></td>
                                                <td>
                                                    <label class="col-form-label float-right">Totals</label></td>

                                                <td style="width: 15%" class="td-center">
                                                    <asp:TextBox ID="txtbox_TotalamountEB" runat="server" Font-Size="Small"
                                                        CssClass="form-control rounded-0 text-center" AutoPostBack="true" ReadOnly="true">
                                                    </asp:TextBox>
                                                </td>
                                                <% if (!this.isPageView)
                                                    { %>
                                                <td></td>
                                                <% } %>
                                            </FooterTemplate>

                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddExpenseBreak" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                

                <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>
                <ucfunding:FundingComponent ID="ctlFunding" runat="server"></ucfunding:FundingComponent>

                <div class="row">
                    <div class="col-lg-12 form-group">
                        <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default btn-flat float-right" UseSubmitBehavior="false" Text="Cancel" OnClick="btnCancel_Click" />
                        <asp:Button runat="server" ID="btnSaveDraft" CssClass="btn btn-primary btn-flat float-right" UseSubmitBehavior="false" Style="margin-right: 5px;" Text="Save Draft" OnClick="btnSaveDraft_Click" />
                        <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success btn-flat float-right" UseSubmitBehavior="false" Style="margin-right: 5px;" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <%: Scripts.Render("~/bundles/dropzone") %>
    <%--<%: Scripts.Render("~/bundles/dropzonejs") %>--%>

    <script type="text/javascript">
        function showpreview(buttontarget) {
            $(buttontarget).trigger('click');
        }

        function launchOpenDialog(buttontarget) {
            $(buttontarget).trigger('click');
        }
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {

            $('.dtpDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
            });

        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>
