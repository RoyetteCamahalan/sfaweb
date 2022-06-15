<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LOA.aspx.cs" Inherits="SimpleFFO.Views.LOA" %>

<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
    <link href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css"
        rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>Leave of Absence</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Employee Information</h3>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtbox_fullname">Name</label>
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text"><i class="fa fa-user"></i>
                                    </span>
                                </div>
                                <asp:TextBox ID="txtbox_fullname" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="txtbox_fullname">Date Filed</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_datefiled" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="txtbox_fullname">Date Hired</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_datehired" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="txtbox_fullname">Role/Position</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-user"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_role" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label for="txtbox_fullname">Branch</label>
                                    <div class="input-group">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fa fa-building"></i>
                                            </span>
                                        </div>
                                        <asp:TextBox ID="txtbox_branch" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Leave Credits</h3>
                    </div>
                    <div class="card-body">
                        <table id="tablebalance" class="table table-bordered table-hover dataTable dtr-inline" style="text-align: center;">
                            <thead>
                                <tr>
                                    <td>
                                        <label>Total Days</label></td>
                                    <td>
                                        <label>Days Approved</label></td>
                                    <td>
                                        <label>Days Pending</label></td>
                                    <td>
                                        <label>Leave Balance</label></td>
                                </tr>
                            </thead>

                            <tr>
                                <td>
                                    <asp:TextBox ID="txttotaldaysapplied" runat="server" CssClass="form-control text-center" ReadOnly="true"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txttotalapproveddays" runat="server" CssClass="form-control text-center" ReadOnly="true"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txttotalpendingdays" runat="server" CssClass="form-control text-center" ReadOnly="true"></asp:TextBox></td>
                                <td>
                                    <asp:TextBox ID="txtleavebalance" runat="server" CssClass="form-control text-center" ReadOnly="true"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                </div>


                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Leave Details</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:GridView ID="tblLeave"
                                    runat="server"
                                    AutoGenerateColumns="false"
                                    UseAccessibleHeader="true"
                                    ShowHeaderWhenEmpty="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%">

                                    <Columns>

                                        <asp:TemplateField HeaderText="Leave Date" HeaderStyle-Font-Bold="true"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label runat="server" Text='<%# Convert.ToDateTime(Eval("dayfrom")).ToString(AppModels.dateformat) %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Leave Type"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <a href="#" onclick="return window.open('<%# getElementRoute(Eval("leaveid").ToString()) %>', '_blank');">
                                                        <%# Eval("leavetype.leavetypename") %>
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="No. of Days"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label runat="server" Text='<%# Eval("noofdays") %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%"
                                            ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label ID="lbl_status" runat="server" Text='<%# getStatus(Convert.ToInt32(Eval("status"))) %>'
                                                        CssClass='<%# getStatusBadge(Convert.ToInt32(Eval("status"))) %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>


                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Leave Application</h3>
                    </div>
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelmain" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-3 col-sm-6 col-xs-12">

                                        <div class="form-group">
                                            <label>From <span class="required">*</span></label>
                                            <div class="input-group date" data-target-input="nearest">
                                                <asp:TextBox ID="dtpFromDate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpFromDate"
                                                    aria-describedby="inputSuccess2Status3" OnTextChanged="dtpDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                                <span id="inputSuccess2Status3" class="sr-only">(success)</span>
                                                <span class="error invalid-feedback">
                                                    <asp:Literal ID="errordtpFromDate" runat="server"></asp:Literal></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>To <span class="required">*</span></label>
                                            <div class="input-group date" data-target-input="nearest">
                                                <asp:TextBox ID="dtpToDate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpToDate"
                                                    aria-describedby="inputSuccess2Status4" OnTextChanged="dtpDate_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                                <span id="inputSuccess2Status4" class="sr-only">(success)</span>
                                                <span class="error invalid-feedback">
                                                    <asp:Literal ID="errordtpToDate" runat="server"></asp:Literal></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Leave Days</label>
                                            <asp:TextBox ID="txtboxLeaveDays" runat="server" Enabled="false" CssClass="form-control rounded-0"></asp:TextBox>
                                            <span class="error invalid-feedback">
                                                <asp:Literal ID="errortxtboxLeaveDays" runat="server"></asp:Literal></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Add Half Day</label>
                                            <asp:DropDownList ID="dropdownHalfDays" runat="server" CssClass="custom-select rounded-0" AutoPostBack="true" OnTextChanged="dtpDate_TextChanged">
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Type Of Leave Filed *</label>
                                            <asp:DropDownList ID="cmbTypeofLeave" runat="server" CssClass="custom-select rounded-0" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Selected="True" Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Back To Work On *</label>
                                            <div class="input-group date" data-target-input="nearest">
                                                <asp:TextBox ID="dtpToWorkDate" runat="server" ReadOnly="true" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpToWorkDate"
                                                    aria-describedby="inputSuccess2Status5"></asp:TextBox>
                                                <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                                <span id="inputSuccess2Status5" class="sr-only">(success)</span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Covered Day-Off</label>
                                            <asp:TextBox ID="txtboxCoveredDayoff" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Covered Holidays</label>
                                            <asp:TextBox ID="txbox_coveredHolidays" runat="server" AutoPostBack="true" ReadOnly="true" CssClass="form-control rounded-0"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6 col-sm-6 col-xs-12">

                                        <div class="form-group">
                                            <label>Reason For Leave *</label>
                                            <asp:TextBox ID="txtbox_reason" runat="server" CssClass="form-control rounded-0">                                                                                   
                                            </asp:TextBox>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>

                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Contact Address And Number *</label>
                                            <asp:TextBox ID="txtbox_contact" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-sm-6 col-xs-12">
                                        <div class="form-group">
                                            <label>Reliever</label>
                                            <asp:TextBox ID="txtReliever" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            
            <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>

            <div class="row">
                <div class="col-lg-12 form-group">
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default btn-flat float-right" UseSubmitBehavior="false" CausesValidation="false" Text="Close" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary btn-flat float-right" Text="Submit" CausesValidation="false" OnClick="btnSubmit_Click" Style="margin-right: 5px;" />
                    <asp:Button runat="server" ID="btnTagasCancelled" CssClass="btn btn-danger btn-flat float-right" Text="Cancel Leave" CausesValidation="false" OnClick="btnTagasCancelled_Click" Style="margin-right: 5px;" />
                </div>
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <script type="text/javascript">
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {

            if ($.fn.dataTable.isDataTable('#tablebalance')) {
                $('#tablebalance').DataTable();
            }
            else {
                $('#tablebalance').DataTable({
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": false,
                    "info": false,
                    "autoWidth": false,
                    "responsive": true,
                });
            }
            if ($.fn.dataTable.isDataTable('#MainContent_tblLeave')) {
                $('#MainContent_tblLeave').DataTable();
            }
            else {
                $('#MainContent_tblLeave').DataTable({
                    "paging": false,
                    "lengthChange": false,
                    "searching": false,
                    "ordering": false,
                    "info": false,
                    "autoWidth": false,
                    "responsive": true,
                });
            }
            $('.dtpFromDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                $('.dtpFromDate').trigger('change');
            });

            $('.dtpToDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                $('.dtpToDate').trigger('change');
            });

            /*$('.dtpToWorkDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                $('.dtpToWorkDate').trigger('change');
            });*/

        };

        function ConfirmYesNo() {
            if (confirm("Are you sure?") == true) {
                return true;
            } else {
                alert("Cancelled");
                return false;
            }
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>
