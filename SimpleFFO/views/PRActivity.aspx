<%@ Page Title="PR Activity" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeBehind="PRActivity.aspx.cs" Inherits="SimpleFFO.PRActivity" %>

<%@ Register Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register TagName="StatusTrailComponent" Src="components/StatusTrailComponent.ascx" TagPrefix="uccom"%>
<%@ Register TagName="FundingComponent" Src="components/FundingComponent.ascx" TagPrefix="ucfunding"%>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentAdditionalCSS" runat="server">
    <webopt:BundleReference runat="server" Path="~/Content/tablecss" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%@ Import Namespace="SimpleFFO.Model" %>
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1>PR Activity</h1>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </section>

        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Proposed Activity</h3>
                    </div>
                    <!-- /.card-header -->
                    <!-- form start -->
                    <div class="card-body">


                        <asp:UpdatePanel ID="upanelprdetails" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-sm-8">
                                        <div class="form-group">
                                            <label for="cmbActivityType">Proposed Activity <span class="required">*</span></label>
                                            <asp:DropDownList ID="cmbActivityType" runat="server" CssClass="custom-select rounded-0" TabIndex="-1" AppendDataBoundItems="true">
                                                <asp:ListItem Text="--Select--" Selected="True" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="form-group">
                                            <label for="txtdatefiled">Date Submitted</label>
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fa fa-calendar"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtdatefiled" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-2">
                                        <div class="form-check">
                                            <asp:RadioButton ID="rdInstitutions" runat="server" CssClass="form-check-input" GroupName="rdchoices" OnCheckedChanged="RadioButton1_CheckedChanged" AutoPostBack="True" />
                                            <label class="form-check-label">Institution</label>
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="form-check">
                                            <asp:RadioButton ID="rdDoctors" runat="server" Checked="true" CssClass="form-check-input" GroupName="rdchoices" OnCheckedChanged="RadioButton1_CheckedChanged" AutoPostBack="True" />
                                            <label class="form-check-label">Doctor / Key Person</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                        <div class="form-group">
                                            <label for="drplstChoice">
                                                <asp:Literal ID="lbldropdown" runat="server" Text="Customer "></asp:Literal><span class="required">*</span></label>
                                            <asp:DropDownList ID="drplstChoice" runat="server" TabIndex="-1" CssClass="custom-select rounded-0" AutoPostBack="true" OnSelectedIndexChanged="drplstChoice_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label for="txtClassification">Classification</label>
                                            <asp:TextBox ID="txtClassification" runat="server" CssClass="form-control rounded-0" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                        <div class="form-group">
                                            <label>Location<span class="required">*</span></label>

                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text"><i class="fas fa-map-marker"></i>
                                                    </span>
                                                </div>
                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control rounded-0"></asp:TextBox>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Date and time:</label>
                                            <div class="input-group date" id="reservationdatetime" data-target-input="nearest">
                                                <asp:TextBox ID="dtpDate" runat="server" autocomplete="off" CssClass="form-control datetimepicker-input rounded-0 dtpdatetime" aria-describedby="inputSuccess2Status3" data-toggle="datetimepicker"></asp:TextBox>
                                                <div class="input-group-append" data-target="#reservationdatetime" data-toggle="datetimepicker">
                                                    <div class="input-group-text"><i class="fa fa-calendar"></i></div>
                                                </div>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="drplstChoice" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rdInstitutions" EventName="CheckedChanged" />
                                <asp:AsyncPostBackTrigger ControlID="rdDoctors" EventName="CheckedChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.card-body -->
                </div>
            </div>
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">List of Attendees</h3>
                    </div>
                    <!-- /.card-header -->
                    <!-- form start -->
                    <div class="card-body">
                        <asp:UpdatePanel ID="upanelattendee" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                            <ContentTemplate>
                                <div class="form-group float-right">
                                    <asp:Button ID="btnAddAttendee" runat="server" Text="Add New" CssClass="btn btn-primary" OnClick="btnAddAttendee_Click" CausesValidation="false" />
                                </div>
                                <asp:GridView ID="tblListOFAtendees" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" OnRowCommand="tblListOFAtendees_RowCommand" OnRowDataBound="tblExpectedOutput_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="MD/Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDocName" runat="server" Text='<%# Eval("doc_name") %>' CssClass="form-control rounded-0 txtDocName"></asp:TextBox>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.required %></span>
                                                <asp:HiddenField ID="hfdoc_id" Value='<%# Eval("doc_id") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Specialty">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtSpecialty" runat="server" Text='<%# Eval("specialization") %>' CssClass="form-control rounded-0 txtSpecialty"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Listed On TP" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" Enabled="false" CssClass="chkListed" Checked='<%# Convert.ToInt32(Eval("doc_id"))>0 ? true : false %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Notes">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control rounded-0" Text='<%# Eval("Remarks") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="td-center">
                                            <ItemTemplate>
                                                <div align="center">
                                                    <asp:LinkButton ID="btnRemoveAttendee" CssClass="btn btn-danger btn-sm" runat="server" CommandName="removeitem" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"><i class="far fa-trash-alt"></i></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddAttendee" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.card-body -->
                </div>
            </div>
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Expected Outcome</h3>
                    </div>
                    <!-- /.card-header -->
                    <!-- form start -->
                    <div class="card-body">
                        <asp:UpdatePanel ID="upaneloutcome" UpdateMode="Conditional" ChildrenAsTriggers="false" runat="server">
                            <ContentTemplate>
                                <div class="form-group float-right">
                                    <asp:Button ID="btnAddOutcome" runat="server" Text="Add New" CssClass="btn btn-primary" OnClick="btnAddAttendee_Click" CausesValidation="false" />
                                </div>
                                <br />
                                <asp:GridView ID="tblExpectedOutput" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" OnRowCommand="tblListOFAtendees_RowCommand" OnRowDataBound="tblExpectedOutput_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="cmbProducts" CssClass="custom-select rounded-0 cmbProducts"></asp:DropDownList>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.duplicateentry %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Projected Monthly Sales">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtOutcome" runat="server" CssClass="form-control rounded-0 txtOutcome" TextMode="Number" Text='<%# Eval("monthlysales") ?? 0 %>' />
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.invalidinput %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <div align="center">
                                                    <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger btn-sm" runat="server" CommandName="removeitem" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"><i class="far fa-trash-alt"></i></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddOutcome" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.card-body -->
                </div>
            </div>
            <div class="container-fluid">
                <div class="card card-primary">
                    <div class="card-header">
                        <h3 class="card-title">Proposed Budget</h3>
                    </div>
                    <!-- /.card-header -->
                    <!-- form start -->
                    <div class="card-body">
                        <asp:UpdatePanel UpdateMode="Conditional" ID="upanelbudget" runat="server" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <div class="form-group float-right">
                                    <asp:Button ID="btnAddBudget" runat="server" Text="Add New" CssClass="btn btn-primary" OnClick="btnAddAttendee_Click" CausesValidation="false" />
                                </div>
                                <asp:GridView ID="tblProposedBudget" runat="server" AutoGenerateColumns="false" UseAccessibleHeader="true"
                                    CssClass="table table-bordered table-hover dataTable dtr-inline" CellSpacing="0" Width="100%" OnRowCommand="tblListOFAtendees_RowCommand" OnRowDataBound="tblExpectedOutput_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Budget Type">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="cmbBudgetTypes" CssClass="custom-select rounded-0 cmbBudgetTypes"></asp:DropDownList>
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.duplicateentry %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtAmount" runat="server" TextMode="Number" CssClass="form-control rounded-0 txtBudgetAmount" Text='<%# Eval("amount") ?? 0 %>' OnTextChanged="txtAmount_TextChanged" AutoPostBack="true" />
                                                <span class="error invalid-feedback"><%= AppModels.ErrorMessage.invalidinput %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="5%">
                                            <ItemTemplate>
                                                <div align="center">
                                                    <asp:LinkButton ID="btnRemoveBudget" CssClass="btn btn-danger btn-sm" runat="server" CommandName="removeitem" CommandArgument='<%# Container.DataItemIndex %>' CausesValidation="false"><i class="far fa-trash-alt"></i></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="col-sm-4 col-xs-12 float-right">
                                    <div class="form-group row">
                                        <label class="col-sm-3 col-form-label" for="txtTotalBudget">Total Budget</label>
                                        <div class="col-sm-9">
                                            <asp:TextBox ID="txtTotalBudget" runat="server" CssClass="txtTotalBudget form-control rounded-0 " Enabled="true" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="ln_solid"></div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddBudget" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /.card-body -->
                </div>
            </div>

            <uccom:StatusTrailComponent ID="ctlStatusTrail" runat="server"></uccom:StatusTrailComponent>
            <ucfunding:FundingComponent ID="ctlFunding" runat="server"></ucfunding:FundingComponent>
            
            <!-- /.container-fluid -->
            <div class="row">
                <div class="col-lg-12 form-group">
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default btn-flat float-right" CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
                    <asp:Button runat="server" ID="btnSaveDraft" CssClass="btn btn-primary btn-flat float-right" CausesValidation="false" Style="margin-right: 5px;" Text="Save Draft" OnClick="btnSaveDraft_Click" />
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success btn-flat float-right" CausesValidation="false" Style="margin-right: 5px;" Text="Submit" OnClick="btnSubmit_Click" />
                </div>
            </div>
        </section>
        <!-- /.content -->
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentAdditionalJS" runat="server">
    <%: Scripts.Render("~/bundles/tables") %>
    <script type="text/javascript">
        $(document).ready(function () {
            bindJs();
        });

        function bindJs() {
            $('.dtpdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });
            $('.dtpDate').daterangepicker({
                singleDatePicker: true,
                calender_style: "picker_3"
            }, function (start, end, label) {
                console.log(start.toISOString(), end.toISOString(), label);
            });
            $(".txtDocName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: '<%= ResolveUrl("~/views/PRActivity.aspx/GetCompletionList")  %>',
                        data: "{ 'warehouseid': '<%= this.warehouseid  %>','prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.name,
                                    val: item.doc_id,
                                    specialization: item.specialization
                                }
                            }))
                        },
                        error: function (response) {
                        },
                        failure: function (response) {

                        }
                    });
                },
                select: function (e, i) {
                    $(this).parent().find("input[type=hidden]").val(i.item.val);
                    $(this).parent().parent().find('.txtSpecialty').val(i.item.specialization);
                    $(this).parent().parent().find('.txtSpecialty').val(i.item.specialization);
                    $(this).parent().parent().find('input[type=checkbox]').prop("checked", true);
                },
                minLength: 1
            }).focus(function () {
                $(this).autocomplete("search");
            });
        };

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            // re-bind your jQuery events here
            bindJs()
        });
    </script>
</asp:Content>




